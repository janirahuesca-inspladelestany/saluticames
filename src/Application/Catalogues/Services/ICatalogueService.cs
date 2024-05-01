using Contracts.DTO.Catalogues;
using SharedKernel.Common;
using System.Threading;

namespace Application.Catalogues.Services;

public interface ICatalogueService
{
    Task<Result<IEnumerable<Guid>, Error>> CreateSummitsAsync(Guid catalogueId, IEnumerable<SummitDto> summitsToCreate, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(Guid catalogueId, IDictionary<Guid, SummitDto> summitsToReplace, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(Guid catalogueId, IEnumerable<Guid> summitIdsToRemove, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, SummitDto>, Error>> GetSummitsAsync(Guid catalogueId, GetSummitsFilterDto filter, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, CatalogueDto>, Error>> GetCatalogues(GetCataloguesFilterDto filter, CancellationToken cancellationToken = default);
}