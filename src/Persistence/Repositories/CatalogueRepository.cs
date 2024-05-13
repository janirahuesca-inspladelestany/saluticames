using Application.Content.Repositories;
using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class CatalogueRepository : ICatalogueRepository
{
    private readonly DbSet<CatalogueAggregate> _catalogues;

    public CatalogueRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _catalogues = salutICamesDbContext.Set<CatalogueAggregate>();
    }

    public async Task<IEnumerable<CatalogueAggregate>> ListAsync(Expression<Func<CatalogueAggregate, bool>>? filter = null,
        Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        IQueryable<CatalogueAggregate> query = _catalogues;

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var catalogues = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return catalogues;
    }

    public async Task<CatalogueAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _catalogues.Include(c => c.CatalogueSummits).SingleOrDefaultAsync(catalogue => catalogue.Id == id, cancellationToken);
    }
}
