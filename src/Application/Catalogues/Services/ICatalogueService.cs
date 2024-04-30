using Contracts.Catalogues.Models;
using SharedKernel.Common;

namespace Application.Catalogues.Services;

public interface ICatalogueService
{
    Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId, IEnumerable<SummitDto> summitsToCreate, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId, IDictionary<Guid, SummitDto> summitsToReplace, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(Guid catalogueId, IEnumerable<Guid> summitIdsToRemove, CancellationToken cancellationToken);
    Task<Result<IDictionary<Guid, SummitDto>, Error>> GetSummitsAsync(Guid catalogueId, CancellationToken cancellationToken);
    Task<Result<IDictionary<Guid, CatalogueDto>, Error>> GetCataloguesAsync(CancellationToken cancellationToken);
}