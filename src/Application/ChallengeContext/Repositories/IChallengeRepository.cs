using Application.Abstractions;
using Domain.ChallengeContext.Entities;

namespace Application.ChallengeContext.Repositories;

public interface IChallengeRepository : IRepository<Diary, Guid>
{
    Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(Guid hikerId, CancellationToken cancellationToken = default);
}
