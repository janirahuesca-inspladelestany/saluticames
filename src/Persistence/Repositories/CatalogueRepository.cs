using Application.Catalogues.Repositories;
using Domain.Catalogues.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class CatalogueRepository : ICatalogueRepository
{
    private readonly DbSet<Catalogue> _catalogues;
    private readonly DbSet<Summit> _summits;

    public CatalogueRepository(CatalogueDbContext catalogueDbContext)
    {
        _catalogues = catalogueDbContext.Set<Catalogue>();
        _summits = catalogueDbContext.Set<Summit>();
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
            query.Include(includeProperty);
        }

        var catalogues = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return catalogues;
    }

    public async Task<Catalogue?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _catalogues.Include(c => c.Summits).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void RemoveSummitRange(IEnumerable<Summit> summits)
    {
        _summits.RemoveRange(summits);
    }

    public async Task<Catalogue?> GetSummitsAsync(Guid id,
        Expression<Func<Summit, bool>>? filter = null,
        Func<IQueryable<Summit>, IOrderedQueryable<Summit>>? orderBy = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        var catalogue = await _catalogues.Include(c => c.Summits).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (catalogue is null) return null;

        IQueryable<Summit> query = catalogue.Summits.AsQueryable();

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(includeProperty);
        }

        var summits = orderBy is not null
            ? orderBy(query).ToList()
            : query.ToList();

        catalogue.ClearSummits();
        catalogue.AddSummits(summits); 
        
        return catalogue;
    }
}
