using Application.Abstractions;
using Domain.Challenge.Entities;
using System.Linq.Expressions;

namespace Application.Challenge.Repositories;

public interface IHikerRepository : IRepository<Hiker, string>
{
    Task Add(Hiker hiker, CancellationToken cancellationToken = default);
    Task<IEnumerable<Hiker>> ListAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<Hiker?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Diary>> ListDiariesAsync(string id, Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<IDictionary<string, IEnumerable<Diary>>> ListDiariesAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default);
    Task<IEnumerable<Climb>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default);
}
