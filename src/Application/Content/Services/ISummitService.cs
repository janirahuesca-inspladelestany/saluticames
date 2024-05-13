using Contracts.DTO.Content;
using SharedKernel.Common;

namespace Application.Content.Services;

public interface ISummitService
{
    Task<Result<IEnumerable<Guid>, Error>> AddNewSummitsAsync(IEnumerable<AddNewSummitDto> summitDtos, CancellationToken cancellationToken = default);
    Task<Result<IDictionary<Guid, ListSummitDetailDto>, Error>> ListSummitsAsync(ListSummitsFilterDto filterDto, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(IDictionary<Guid, ReplaceSummitDetailDto> summitDtos, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(IEnumerable<Guid> summitIds, CancellationToken cancellationToken = default);
}