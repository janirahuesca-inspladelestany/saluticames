using Application.Abstractions;
using Domain.ChallengeContext.Entities;
using System.Linq.Expressions;

namespace Application.ChallengeContext.Repositories;

public interface IDiaryRepository : IRepository<Diary, Guid>
{
    Task<IEnumerable<Diary>> ListAsync(Expression<Func<Diary, bool>>? filter = null, Func<IQueryable<Diary>, IOrderedQueryable<Diary>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task Add(Diary diary, CancellationToken cancellationToken = default);
    Task<Diary?> GetByHikerIdAsync(string hikerId, CancellationToken cancellation = default);
    Task<IEnumerable<Hiker>> ListHikersAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<Hiker?> FindHikerByIdAsync(string hikerId, CancellationToken cancellationToken = default);
    Task AddHiker(Hiker hiker, CancellationToken cancellationToken = default);
    Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(string hikerId, CancellationToken cancellationToken = default);
}
