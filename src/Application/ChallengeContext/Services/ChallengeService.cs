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
        // Recuperar el diaries del hiker
        var diaries = await _unitOfWork.HikerRepository.ListDiariesByHikerIdAsync(
            hikerId, cancellationToken: cancellationToken);

        if (!diaries.Any()) return ChallengeErrors.DiaryNotFound;

        // Mapejar de DTO a BO
        var climbsToCreate = climbDetailsToCreate.Select(climbDetailToCreate =>
        {
            return Climb.Create(climbDetailToCreate.SummitId, climbDetailToCreate.AscensionDateTime);
        });

        foreach (var diary in diaries)
        {
            var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(diary.CatalogueId, cancellationToken);
            if(catalogue is null) continue;

            var diaryClimbsToCreate = climbsToCreate.Where(climbToCreate => catalogue.Summits.Any(summit => summit.Id == climbToCreate.SummitId));
            
            // Afegir ascensions al diary
            var addClimbsResult = diary.AddClimbs(diaryClimbsToCreate);
            if (addClimbsResult.IsFailure()) return addClimbsResult.Error;
        }

        if (climbsToCreate.Any())
        {
            // Persistir els diaries
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return climbsToCreate.Select(climb => climb.Id).ToList();
    }

    public async Task<Result<Guid, Error>> CreateDiaryAsync(CreateDiaryDetailDto diaryToCreate, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.HikerRepository.FindByIdAsync(diaryToCreate.HikerId, cancellationToken);
        if (hiker is null) return ChallengeErrors.HikerNotFound;

        // Validar el diari        
        var diary = hiker.Diaries.FirstOrDefault(diary => diary.CatalogueId == diaryToCreate.CatalogueId);
        if (diary is not null) return ChallengeErrors.DiaryAlreadyExists;

        // Mapejar de DTO a BO
        var createDiaryResult = Diary.Create(diaryToCreate.Name, diaryToCreate.CatalogueId);
        if (createDiaryResult.IsFailure()) return createDiaryResult.Error;

        diary = createDiaryResult.Value!;
        var addDiaryResult = hiker.AddDiary(diary);
        if (addDiaryResult.IsFailure()) return createDiaryResult.Error;

        // Persistir el hiker
        await _unitOfWork.SaveChangesAsync();

        return diary.Id;
    }

    public async Task<EmptyResult<Error>> CreateHikerAsync(CreateHikerDetailDto hikerToCreate, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.HikerRepository.FindByIdAsync(hikerToCreate.Id, cancellationToken);
        if (hiker is not null) return ChallengeErrors.HikerAlreadyExists;

        // Mapejar de DTO a BO
        var createHikerResult = Hiker.Create(hikerToCreate.Id, hikerToCreate.Name, hikerToCreate.Surname);
        if (createHikerResult.IsFailure()) return createHikerResult.Error;

        // Persistir el hiker
        await _unitOfWork.HikerRepository.Add(createHikerResult.Value!);
        await _unitOfWork.SaveChangesAsync();

        return EmptyResult<Error>.Success();
    }

    public async Task<Result<Dictionary<Guid, GetClimbDetailDto>, Error>> GetClimbsAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        // Recuperar els climbs
        var climbs = await _unitOfWork.HikerRepository.GetClimbsByHikerIdAsync(hikerId);

        // Mapejar de BO a DTO
        var result = climbs.ToDictionary(climb => climb.Id, climb =>
            new GetClimbDetailDto(
                SummitId: climb.SummitId,
                AscensionDateTime: climb.AscensionDate));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<string, IEnumerable<GetDiaryDetailDto>>, Error>> GetDiariesAsync(GetDiariesFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els diaries
        var diariesByHikerIds = await _unitOfWork.HikerRepository.ListDiariesAsync(
            filter: hiker =>
                (filter.Id != null ? hiker.Diaries.Any(diary => diary.Id == filter.Id) : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? hiker.Diaries.Any(diary => diary.Name.Contains(filter.Name)) : true) &&
                (!string.IsNullOrEmpty(filter.HikerId) ? hiker.Id.Equals(filter.HikerId) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = diariesByHikerIds.ToDictionary(diariesByHikerId => diariesByHikerId.Key, diariesByHikerId =>
            diariesByHikerId.Value.Select(diaryByHikerId =>
                new GetDiaryDetailDto(
                    Id: diaryByHikerId.Id,
                    Name: diaryByHikerId.Name)));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<string, GetHikerDetailDto>, Error>> GetHikersAsync(GetHikersFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els hikers
        var hikers = await _unitOfWork.HikerRepository.ListAsync(
            filter: hiker =>
                (filter.Id != null ? hiker.Id == filter.Id : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? hiker.Name.Contains(filter.Name) : true) &&
                (!string.IsNullOrEmpty(filter.Surname) ? hiker.Surname.Contains(filter.Surname) : true),
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
        var climbs = await _unitOfWork.HikerRepository.GetClimbsByHikerIdAsync(hikerId);

        var catalogues = await _unitOfWork.CatalogueRepository.ListAsync(
            filter: catalogue => catalogueId.HasValue ? catalogue.Id == catalogueId.Value : true,
            includeProperties: nameof(Catalogue.Summits), cancellationToken: cancellationToken);

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
