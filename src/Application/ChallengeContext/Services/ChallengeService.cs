using Application.Abstractions;
using Contracts.DTO.Catalogue;
using Contracts.DTO.Challenge;
using Domain.CatalogueContext.Entities;
using Domain.ChallengeContext.Entities;
using Domain.ChallengeContext.Errors;
using SharedKernel.Common;

namespace Application.ChallengeContext.Services;

public class ChallengeService : IChallengeService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChallengeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Guid>, Error>> CreateClimbsAsync(string hikerId, IEnumerable<CreateClimbDetailDto> climbDetailsToCreate, CancellationToken cancellationToken = default)
    {
        // Recuperar el diari
        var diary = await _unitOfWork.DiaryRepository.GetByHikerIdAsync(hikerId, cancellationToken);
        if (diary is null) return ChallengeErrors.DiaryNotFound;

        // Mapejar de DTO a BO
        var climbsToCreate = climbDetailsToCreate.Select(climbDetailsToCreate =>
            Climb.Create(hikerId, climbDetailsToCreate.SummitId, climbDetailsToCreate.AscensionDateTime));

        // Afegir ascensions al diari
        diary.AddClimbs(climbsToCreate);

        // Persistir el diari
        if (climbDetailsToCreate.Any())
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return climbsToCreate.Select(climb => climb.Id).ToList();
    }

    public async Task<Result<Guid, Error>> CreateDiaryAsync(CreateDiaryDetailDto diaryToCreate, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.DiaryRepository.FindHikerByIdAsync(diaryToCreate.HikerId, cancellationToken);
        if (hiker is null) return ChallengeErrors.HikerNotFound;

        // Recuperar el diary
        var diary = await _unitOfWork.DiaryRepository.GetByHikerIdAsync(diaryToCreate.HikerId, cancellationToken);
        if (diary is not null) return ChallengeErrors.DiaryAlreadyExists;

        // Mapejar de DTO a BO
        var createDiaryResult = Diary.Create(diaryToCreate.Name, hiker);

        if (createDiaryResult.IsFailure()) return createDiaryResult.Error;
        diary = createDiaryResult.Value!;

        // Persistir el hiker
        await _unitOfWork.DiaryRepository.Add(diary);
        await _unitOfWork.SaveChangesAsync();

        return diary.Id;
    }

    public async Task<EmptyResult<Error>> CreateHikerAsync(CreateHikerDetailDto hikerToCreate, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.DiaryRepository.FindHikerByIdAsync(hikerToCreate.Id, cancellationToken);
        if (hiker is not null) return ChallengeErrors.HikerAlreadyExists;

        // Mapejar de DTO a BO
        var createHikerResult = Hiker.Create(hikerToCreate.Id, hikerToCreate.Name, hikerToCreate.Surname);
        if (createHikerResult.IsFailure()) return createHikerResult.Error;

        // Persistir el hiker
        await _unitOfWork.DiaryRepository.AddHiker(createHikerResult.Value!);
        await _unitOfWork.SaveChangesAsync();

        return EmptyResult<Error>.Success();
    }

    public async Task<Result<Dictionary<Guid, GetClimbDetailDto>, Error>> GetClimbsAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        // Recuperar els climbs
        var climbs = await _unitOfWork.DiaryRepository.GetClimbsByHikerIdAsync(hikerId);

        // Mapejar de BO a DTO
        var result = climbs.ToDictionary(climb => climb.Id, climb =>
            new GetClimbDetailDto(
                SummitId: climb.SummitId,
                AscensionDateTime: climb.AscensionDate));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<Guid, GetDiaryDetailDto>, Error>> GetDiariesAsync(GetDiariesFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els diaries
        var diaries = await _unitOfWork.DiaryRepository.ListAsync(
            filter: d =>
                (filter.Id != null ? d.Id == filter.Id : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? d.Name.Contains(filter.Name) : true) &&
                (!string.IsNullOrEmpty(filter.HikerId) ? d.Hiker.Id ==  filter.HikerId : true),
            includeProperties: nameof(Diary.Hiker),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = diaries.ToDictionary(diary => diary.Id, diary =>
            new GetDiaryDetailDto(
                Name: diary.Name,
                HikerId: diary.Hiker.Id));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<string, GetHikerDetailDto>, Error>> GetHikersAsync(GetHikersFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els hikers
        var hikers = await _unitOfWork.DiaryRepository.ListHikersAsync(
            filter: h =>
                (filter.Id != null ? h.Id == filter.Id : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? h.Name.Contains(filter.Name) : true) &&
                (!string.IsNullOrEmpty(filter.Surname) ? h.Surname.Contains(filter.Surname) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = hikers.ToDictionary(hiker => hiker.Id, hiker =>
            new GetHikerDetailDto(
                Name: hiker.Name,
                Surname: hiker.Surname));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<Dictionary<Guid, GetStatisticsDto>, Error>> GetStatisticsAsync(string hikerId, Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        var climbs = await _unitOfWork.DiaryRepository.GetClimbsByHikerIdAsync(hikerId);
        var catalogues = await _unitOfWork.CatalogueRepository.ListAsync(includeProperties: nameof(Catalogue.Summits), cancellationToken: cancellationToken);

        if (catalogueId.HasValue) 
        {
            catalogues = catalogues.Where(c => c.Id == catalogueId);
        }

        var stats = catalogues.ToDictionary(catalogue => catalogue.Id, catalogue =>
        {
            var reachedSummits = climbs.Where(climb => catalogue.Summits.Any(s => s.Id == climb.SummitId));
            var pendingSummits = catalogue.Summits.Where(summit => !IsReachedSummit(summit, reachedSummits));

            return new GetStatisticsDto(reachedSummits.Count(), pendingSummits.Count());
        });

        return stats;

        bool IsReachedSummit(Summit summit, IEnumerable<Climb> reachedSummits)
        {
            return reachedSummits.Any(s => s.SummitId == summit.Id);
        }
    }
}
