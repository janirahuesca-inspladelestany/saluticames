using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

public interface ISummitRepository : IRepository<Summit, Guid>
{
    Task AddRangeAsync(IEnumerable<Summit> summits, CancellationToken cancellationToken = default);

    Task AddAsync(Summit summit, CancellationToken cancellationToken = default);

    Task<IEnumerable<Summit>> ListAsync(Expression<Func<Summit, bool>>? filter = null,
        Func<IQueryable<Summit>, IOrderedQueryable<Summit>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);

    Task<Summit?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void RemoveRange(IEnumerable<Summit> summits);

    void Remove(Summit summit);
}
