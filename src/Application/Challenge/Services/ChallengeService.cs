using Application.Abstractions;
using Contracts.DTO.Challenge;
using Domain.Challenge.Entities;
using Domain.Challenge.Errors;
using Domain.Content.Entities;
using SharedKernel.Common;

namespace Application.Challenge.Services;

public class ChallengeService : IChallengeService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChallengeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmptyResult<Error>> AddNewHikerAsync(AddNewHikerDto hikerDto, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.HikerRepository.FindByIdAsync(hikerDto.Id, cancellationToken);
        if (hiker is not null) return ChallengeErrors.HikerAlreadyExists;

        // Mapejar de DTO a BO
        var createHikerResult = Hiker.Create(hikerDto.Id, hikerDto.Name, hikerDto.Surname);
        if (createHikerResult.IsFailure()) return createHikerResult.Error;

        // Persistir el hiker
        await _unitOfWork.HikerRepository.Add(createHikerResult.Value!);
        await _unitOfWork.SaveChangesAsync();

        // Retornar el resultat
        return EmptyResult<Error>.Success();
    }

    public async Task<Result<Guid, Error>> AddNewDiaryAsync(AddNewDiaryDto diaryDto, CancellationToken cancellationToken)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.HikerRepository.FindByIdAsync(diaryDto.HikerId, cancellationToken);
        if (hiker is null) return ChallengeErrors.HikerNotFound;

        // Validar el diary        
        var diary = hiker.Diaries.FirstOrDefault(diary => diary.CatalogueId == diaryDto.CatalogueId);
        if (diary is not null) return ChallengeErrors.DiaryAlreadyExists;

        // Mapejar de DTO a BO
        var createDiaryResult = Diary.Create(diaryDto.Name, diaryDto.CatalogueId);
        if (createDiaryResult.IsFailure()) return createDiaryResult.Error;

        diary = createDiaryResult.Value!;

        var addDiaryResult = hiker.AddDiary(diary);
        if (addDiaryResult.IsFailure()) return createDiaryResult.Error;

        // Persistir el hiker
        await _unitOfWork.SaveChangesAsync();

        // Retornar el resultat
        return diary.Id;
    }

    public async Task<Result<IEnumerable<Guid>, Error>> AddNewClimbsAsync(string hikerId, IEnumerable<AddNewClimbDetailDto> climbDtos, CancellationToken cancellationToken = default)
    {
        // Recuperar el hiker
        var hiker = await _unitOfWork.HikerRepository.FindByIdAsync(hikerId, cancellationToken);
        if (hiker is null) return ChallengeErrors.HikerNotFound;

        // Validar el diaries
        if (!hiker.Diaries.Any()) return ChallengeErrors.DiaryNotFound;

        // Mapejar de DTO a BO
        var climbsToAdd = new List<Climb>();
        foreach (var climbDto in climbDtos)
        {
            var climbCreateResult = Climb.Create(climbDto.SummitId, climbDto.AscensionDateTime);
            if (climbCreateResult.IsFailure())
            {
                return climbCreateResult.Error;
            }

            climbsToAdd.Add(climbCreateResult.Value!);
        }

        foreach (var diary in hiker.Diaries)
        {
            var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(diary.CatalogueId, cancellationToken);
            if (catalogue is null) return ChallengeErrors.DiaryInvalidCatalogueBadReference;

            var climbsByDiary = climbsToAdd.Where(climbToAdd => catalogue.SummitIds.Any(summitId => summitId == climbToAdd.SummitId));

            // Afegir climbs al diary
            var addClimbsToDiaryResult = hiker.AddClimbsToDiary(diary, climbsByDiary);
            if (addClimbsToDiaryResult.IsFailure()) return addClimbsToDiaryResult.Error;
        }

        if (climbsToAdd.Any())
        {
            // Persistir els diaries
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return climbsToAdd.Select(climb => climb.Id).ToList();
    }

    public async Task<Result<IDictionary<string, ListHikerDetailDto>, Error>> ListHikersAsync(ListHikersFilterDto filterDto, CancellationToken cancellationToken = default)
    {
        // Recuperar els hikers
        var hikers = await _unitOfWork.HikerRepository.ListAsync(
            filter: hiker =>
                (filterDto.Id != null ? hiker.Id == filterDto.Id : true) &&
                (!string.IsNullOrEmpty(filterDto.Name) ? hiker.Name.Contains(filterDto.Name) : true) &&
                (!string.IsNullOrEmpty(filterDto.Surname) ? hiker.Surname.Contains(filterDto.Surname) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = hikers.ToDictionary(hiker => hiker.Id, hiker =>
            new ListHikerDetailDto(
                Name: hiker.Name,
                Surname: hiker.Surname));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<Dictionary<Guid, FindClimbDetailDto>, Error>> FindClimbsAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        // Recuperar els climbs
        var climbs = await _unitOfWork.HikerRepository.ListClimbsByHikerIdAsync(hikerId);

        // Mapejar de BO a DTO
        var result = climbs.ToDictionary(climb => climb.Id, climb =>
            new FindClimbDetailDto(
                SummitId: climb.SummitId,
                AscensionDateTime: climb.AscensionDate));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IDictionary<string, IEnumerable<ListDiaryDetailDto>>, Error>> ListDiariesAsync(ListDiariesFilterDto filterDto, CancellationToken cancellationToken = default)
    {
        // Recuperar els diaries
        var diariesByHikerIds = await _unitOfWork.HikerRepository.ListDiariesAsync(
            filter: hiker =>
                (filterDto.Id != null ? hiker.Diaries.Any(diary => diary.Id == filterDto.Id) : true) &&
                (!string.IsNullOrEmpty(filterDto.Name) ? hiker.Diaries.Any(diary => diary.Name.Contains(filterDto.Name)) : true) &&
                (!string.IsNullOrEmpty(filterDto.HikerId) ? hiker.Id.Equals(filterDto.HikerId) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = diariesByHikerIds.ToDictionary(diariesByHikerId => diariesByHikerId.Key, diariesByHikerId =>
            diariesByHikerId.Value.Select(diaryByHikerId =>
                new ListDiaryDetailDto(
                    Id: diaryByHikerId.Id,
                    Name: diaryByHikerId.Name)));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<Dictionary<Guid, GetStatisticsDto>, Error>> GetStatisticsAsync(string hikerId, Guid? catalogueId = null, CancellationToken cancellationToken = default)
    {
        // Recuperar els climbs i els catalogues
        var climbs = await _unitOfWork.HikerRepository.ListClimbsByHikerIdAsync(hikerId);

        var catalogues = await _unitOfWork.CatalogueRepository.ListAsync(
            filter: catalogue => catalogueId.HasValue ? catalogue.Id == catalogueId.Value : true,
            includeProperties: nameof(Catalogue.SummitIds), cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var statistics = catalogues.ToDictionary(catalogue => catalogue.Id, catalogue =>
        {
            var reachedSummits = climbs.Where(climb => catalogue.SummitIds.Any(summitId => summitId == climb.SummitId));
            var pendingSummits = catalogue.SummitIds.Where(summitId => !IsReachedSummit(summitId, reachedSummits));

            return new GetStatisticsDto(reachedSummits.Count(), pendingSummits.Count());
        });

        // Retornar el resultat
        return statistics;

        bool IsReachedSummit(Guid summitId, IEnumerable<Climb> reachedSummits)
        {
            return reachedSummits.Any(s => s.SummitId == summitId);
        }
    }
}
