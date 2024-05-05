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
        // Recuperar el diaris del hiker
        var diaries = await _unitOfWork.HikerRepository.ListDiariesAsync(
            filter: diary => diary.Hiker.Id.Equals(hikerId),
            includeProperties: nameof(Diary.Hiker),
            cancellationToken: cancellationToken);

        if (!diaries.Any()) return ChallengeErrors.DiaryNotFound;

        // Recuperar els cims de les ascensions
        var getDiarySummitsTasks = diaries.Select(diary => _unitOfWork.CatalogueRepository.GetSummitsAsync(diary.CatalogueId));
        var diarySummits = await Task.WhenAll(getDiarySummitsTasks);
        var summits = diarySummits.SelectMany(diarySummy => diarySummy);

        // Mapejar de DTO a BO
        var climbsToCreate = climbDetailsToCreate.Select(climbDetailToCreate =>
        {
            var summit = summits.Single(summit => summit.Id == climbDetailToCreate.SummitId);
            var diary = diaries.Single(diary => diary.CatalogueId == summit.CatalogueId);

            return Climb.Create(diary, climbDetailToCreate.SummitId, climbDetailToCreate.AscensionDateTime);
        });

        var climbsToCreateGrouppingByDiary = climbsToCreate.GroupBy(climbToCreate => climbToCreate.Diary.Id);

        foreach (var climbsToCreateGroup in climbsToCreateGrouppingByDiary)
        {
            var diary = diaries.Single(diary => diary.Id == climbsToCreateGroup.Key);

            // Afegir ascensions al diari
            var addClimbsResult = diary.AddClimbs(climbsToCreateGroup);
            if (addClimbsResult.IsFailure()) return addClimbsResult.Error;
        }

        if (climbsToCreate.Any())
        {
            // Persistir el diari
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

        // Recuperar el diari
        var diaries = await _unitOfWork.HikerRepository.ListDiariesAsync(
            filter: diary => diary.Hiker.Id.Equals(diaryToCreate.HikerId),
            includeProperties: nameof(Diary.Hiker),
            cancellationToken: cancellationToken);
        
        var diary = diaries.FirstOrDefault(diary => diary.CatalogueId == diaryToCreate.CatalogueId);

        if (diary is not null) return ChallengeErrors.DiaryAlreadyExists;

        // Mapejar de DTO a BO
        var createDiaryResult = Diary.Create(diaryToCreate.Name, hiker, diaryToCreate.CatalogueId);
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

    public async Task<Result<IDictionary<Guid, GetDiaryDetailDto>, Error>> GetDiariesAsync(GetDiariesFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Recuperar els diaries
        var diaries = await _unitOfWork.HikerRepository.ListDiariesAsync(
            filter: diary =>
                (filter.Id != null ? diary.Id == filter.Id : true) &&
                (!string.IsNullOrEmpty(filter.Name) ? diary.Name.Contains(filter.Name) : true) &&
                (!string.IsNullOrEmpty(filter.HikerId) ? diary.Hiker.Id == filter.HikerId : true),
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
