using Contracts.DTO.Catalogue;
using Contracts.DTO.Challenge;
using SharedKernel.Common;

namespace Application.ChallengeContext.Services;

public interface IChallengeService
{
    Task<Result<IEnumerable<Guid>, Error>> CreateClimbsAsync(Guid hikerId, IEnumerable<CreateClimbDetailDto> climbDetailsToCreate, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, GetStatisticsDto>> GetStatisticsAsync(Guid hikerId, CancellationToken cancellationToken = default);
    Task<GetStatisticsDto> GetStatisticsAsync(Guid hikerId, Guid catalogueId, CancellationToken cancellationToken = default);
}
