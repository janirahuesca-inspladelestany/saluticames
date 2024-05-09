using Application.Abstractions;
using Domain.ChallengeContext.Entities;
using System.Linq.Expressions;

namespace Application.ChallengeContext.Repositories;

public interface IHikerRepository : IRepository<Hiker, string>
{
    Task Add(Hiker hiker, CancellationToken cancellationToken = default);
    Task<Hiker?> GetByIdAsync(string hikerId, CancellationToken cancellation = default);
    Task<IEnumerable<Hiker>> ListAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<Hiker?> FindByIdAsync(string hikerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Diary>> ListDiariesByHikerIdAsync(string hikerId, Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<IDictionary<string, IEnumerable<Diary>>> ListDiariesAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(string hikerId, CancellationToken cancellationToken = default);
}
