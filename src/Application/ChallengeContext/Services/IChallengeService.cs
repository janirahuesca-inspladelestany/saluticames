using Contracts.DTO.Challenge;

namespace Application.ChallengeContext.Services;

public interface IChallengeService
{
    Task<Dictionary<Guid, GetStatisticsDto>> GetStatisticsAsync(Guid hikerId, CancellationToken cancellationToken = default);
    Task<GetStatisticsDto> GetStatisticsAsync(Guid hikerId, Guid catalogueId, CancellationToken cancellationToken = default);
}
