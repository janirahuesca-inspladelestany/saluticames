using Application.Content.Repositories;
using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class CatalogueRepository : ICatalogueRepository
{
    private readonly DbSet<Catalogue> _catalogues;

    public CatalogueRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _catalogues = salutICamesDbContext.Set<Catalogue>();
    }

    public async Task<IEnumerable<Catalogue>> ListAsync(Expression<Func<Catalogue, bool>>? filter = null,
        Func<IQueryable<Catalogue>, IOrderedQueryable<Catalogue>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        IQueryable<Catalogue> query = _catalogues;

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

    public async Task<Catalogue?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _catalogues.Include(c => c.CatalogueSummits).SingleOrDefaultAsync(catalogue => catalogue.Id == id, cancellationToken);
    }
}
