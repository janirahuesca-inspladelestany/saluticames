using Contracts.DTO.Catalogue;
using SharedKernel.Common;

namespace Application.Content.Services;

public interface ICatalogueService
{
    Task<EmptyResult<Error>> AddNewSummitIdsInCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, ListCatalogueDetailDto>, Error>> ListCatalogues(ListCataloguesFilterDto filterDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> ListSummitIdsFromCatalogueAsync(Guid catalogueId, CancellationToken cancellationToken = default);
    Task<EmptyResult<Error>> RemoveSummitIdsFromCatalogueAsync(Guid catalogueId, IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);
}