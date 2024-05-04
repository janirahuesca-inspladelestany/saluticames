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

    public async Task<Result<IEnumerable<Guid>, Error>> CreateClimbsAsync(Guid hikerId, IEnumerable<CreateClimbDetailDto> climbDetailsToCreate, CancellationToken cancellationToken = default)
    {
        // Recuperar el diari
        var diary = await _unitOfWork.DiaryRepository.GetByHikerId(hikerId, cancellationToken);
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

    public async Task<Dictionary<Guid, GetStatisticsDto>> GetStatisticsAsync(Guid hikerId, CancellationToken cancellationToken = default)
    {
        var climbs = await _unitOfWork.DiaryRepository.GetClimbsByHikerIdAsync(hikerId);
        var catalogues = await _unitOfWork.CatalogueRepository.ListAsync(includeProperties: "Summits", cancellationToken: cancellationToken);

        var stats = catalogues.ToDictionary(catalogue => catalogue.Id, catalogue =>
        {
            var reachedSummits = climbs.Where(climb => catalogue.Summits.Any(s => s.Id == climb.SummitId));
            var pendingSummits = catalogue.Summits.Where(summit => !IsReachedSummit(summit, reachedSummits));

            return new GetStatisticsDto(reachedSummits.Count(), pendingSummits.Count());
        });

        return stats;
    }

    public async Task<GetStatisticsDto> GetStatisticsAsync(Guid hikerId, Guid catalogueId, CancellationToken cancellationToken = default)
    {
        var climbs = await _unitOfWork.DiaryRepository.GetClimbsByHikerIdAsync(hikerId);
        var summits = await _unitOfWork.CatalogueRepository.GetSummitsAsync(catalogueId, cancellationToken: cancellationToken);

        var reachedSummits = climbs.Where(climb => summits.Any(summit => summit.Id == climb.SummitId));
        var pendingSummits = summits.Where(summit => !IsReachedSummit(summit, reachedSummits));

        var stats = new GetStatisticsDto(reachedSummits.Count(), pendingSummits.Count());
        
        return stats;
    }
    
    private bool IsReachedSummit(Summit summit, IEnumerable<Climb> reachedSummits)
    {
        return reachedSummits.Any(s => s.SummitId == summit.Id);
    }
}
