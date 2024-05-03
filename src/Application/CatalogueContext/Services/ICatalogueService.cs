using Contracts.DTO.Catalogue;
using SharedKernel.Common;

namespace Application.CatalogueContext.Services;

public interface ICatalogueService
{
    Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId, IEnumerable<CreateSummitDetailDto> summitDetailsToCreate, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId, IDictionary<Guid, ReplaceSummitDetailDto> summitDetailsToReplace, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(Guid catalogueId, IEnumerable<Guid> summitIdsToRemove, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, GetSummitDetailDto>, Error>> GetSummitsAsync(Guid catalogueId, GetSummitsFilterDto filter, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, GetCatalogueDetailDto>, Error>> GetCatalogues(GetCataloguesFilterDto filter, CancellationToken cancellationToken = default);
}