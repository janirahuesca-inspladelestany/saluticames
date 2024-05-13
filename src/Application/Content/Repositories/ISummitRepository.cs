using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

public interface ISummitRepository : IRepository<SummitAggregate, Guid>
{
    Task AddRangeAsync(IEnumerable<SummitAggregate> summits, CancellationToken cancellationToken = default);

    Task AddAsync(SummitAggregate summit, CancellationToken cancellationToken = default);

    Task<IEnumerable<SummitAggregate>> ListAsync(Expression<Func<SummitAggregate, bool>>? filter = null,
        Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default);

    Task<SummitAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void RemoveRange(IEnumerable<SummitAggregate> summits);

    void Remove(SummitAggregate summit);
}
