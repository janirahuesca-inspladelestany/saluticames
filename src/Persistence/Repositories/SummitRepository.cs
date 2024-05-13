using Application.Content.Repositories;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class SummitRepository : ISummitRepository
{
    private readonly DbSet<SummitAggregate> _summits;

    public SummitRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _summits = salutICamesDbContext.Set<SummitAggregate>();
    }

    public async Task AddRangeAsync(IEnumerable<SummitAggregate> summits, CancellationToken cancellationToken = default)
    {
        await _summits.AddRangeAsync(summits, cancellationToken);
    }

    public async Task AddAsync(SummitAggregate summit, CancellationToken cancellationToken = default)
    {
        await _summits.AddAsync(summit, cancellationToken);
    }

    public async Task<IEnumerable<SummitAggregate>> ListAsync(Expression<Func<SummitAggregate, bool>>? filter = null,
        Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        IQueryable<SummitAggregate> query = _summits;

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var summits = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return summits;
    }

    public async Task<SummitAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _summits.FindAsync(id, cancellationToken);
    }

    public void RemoveRange(IEnumerable<SummitAggregate> summits)
    {
        _summits.RemoveRange(summits);
    }

    public void Remove(SummitAggregate summit)
    {
        _summits.Remove(summit);
    }
}
