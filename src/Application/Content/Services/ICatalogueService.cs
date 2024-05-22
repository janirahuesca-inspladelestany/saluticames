using Contracts.DTO.Content;
using SharedKernel.Common;

namespace Application.Content.Services;

// Interfície ICatalogueService que declara els mètodes per a la gestió de catàlegs
public interface ICatalogueService
{
    // Mètode per afegir nous identificadors de cim a un catàleg de manera asíncrona
    Task<EmptyResult<Error>> AddNewSummitIdsInCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);

    // Mètode per llistar els catàlegs amb opcions de filtre
    Task<Result<IDictionary<Guid, ListCatalogueDetailDto>, Error>> ListCatalogues(ListCataloguesFilterDto filterDto, CancellationToken cancellationToken = default);

    // Mètode per llistar els identificadors de cim d'un catàleg de manera asíncrona
    Task<Result<IEnumerable<Guid>, Error>> ListSummitIdsFromCatalogueAsync(Guid catalogueId, CancellationToken cancellationToken = default);

    // Mètode per eliminar identificadors de cim d'un catàleg de manera asíncrona
    Task<EmptyResult<Error>> RemoveSummitIdsFromCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);
}