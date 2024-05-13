using Application.Abstractions;
using Contracts.DTO.Content;
using Domain.Content.Errors;
using SharedKernel.Common;

namespace Application.Content.Services;

public class CatalogueService(IUnitOfWork _unitOfWork) : ICatalogueService
{
    public async Task<EmptyResult<Error>> AddNewSummitIdsInCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default)
    {
        // Recuperar el catalogue
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        if (catalogue.SummitIds.Any(summitId => summitIds.Contains(summitId))) return CatalogueErrors.SummitIdAlreadyExists;

        // Afegir summits al catalogue
        var addSummitIdsResult = catalogue.RegisterSummitIds(summitIds.Select(summitId => summitId));
        if (addSummitIdsResult.IsFailure()) return addSummitIdsResult.Error;

        // Persistir el catalogue
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return EmptyResult<Error>.Success();
    }

    public async Task<Result<IDictionary<Guid, ListCatalogueDetailDto>, Error>> ListCatalogues(ListCataloguesFilterDto filterDto, CancellationToken cancellationToken = default)
    {
        // Recuperar els catalogues
        var catalogues = await _unitOfWork.CatalogueRepository.ListAsync(
            filter: catalogue =>
                (filterDto.Id != null ? catalogue.Id == filterDto.Id : true) &&
                (!string.IsNullOrEmpty(filterDto.Name) ? catalogue.Name.Contains(filterDto.Name) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = catalogues.ToDictionary(catalogue => catalogue.Id, catalogue =>
            new ListCatalogueDetailDto(Name: catalogue.Name));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ListSummitIdsFromCatalogueAsync(Guid catalogueId, CancellationToken cancellationToken = default)
    {
        // Recuperar els summits
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Retornar el resultat
        return catalogue.SummitIds.Select(summitId => summitId).ToList();
    }

    public async Task<EmptyResult<Error>> RemoveSummitIdsFromCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default)
    {
        // Recuperar el catalogue
        var catalogue = await _unitOfWork.CatalogueRepository.FindByIdAsync(catalogueId, cancellationToken);
        if (catalogue is null) return CatalogueErrors.CatalogueIdNotFound;

        // Eliminar els summits del catalogue
        var removedSummitsResult = catalogue.RemoveSummitIds(summitIds.Select(summitId => summitId));
        if (removedSummitsResult.IsFailure()) return removedSummitsResult.Error;

        // Persistir el catalogue
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el resultat
        return EmptyResult<Error>.Success();
    }
}
