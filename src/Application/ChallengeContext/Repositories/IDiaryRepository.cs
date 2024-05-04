using Application.Abstractions;
using Domain.ChallengeContext.Entities;

namespace Application.ChallengeContext.Repositories;

public interface IDiaryRepository : IRepository<Diary, Guid>
{
    Task<Diary?> GetByHikerId(Guid hikerId, CancellationToken cancellation = default);
    Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(Guid hikerId, CancellationToken cancellationToken = default);
}
