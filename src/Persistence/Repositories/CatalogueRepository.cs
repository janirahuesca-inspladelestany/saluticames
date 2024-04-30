using Domain.Catalogues.Entities;
using Domain.Catalogues.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class CatalogueRepository : ICatalogueRepository
{
    private readonly CatalogueDbContext _catalogueDbContext;

    public CatalogueRepository(CatalogueDbContext catalogueDbContext)
    {
        _catalogueDbContext = catalogueDbContext;
    }

    public async Task<Catalogue?> GetByIdWithSummitsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _catalogueDbContext.Set<Catalogue>()
            .Include(c => c.Summits)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Catalogue>> ListAsync(CancellationToken cancellationToken)
    {
        return await _catalogueDbContext.Set<Catalogue>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Summit>> ListSummitsAsync(CancellationToken cancellationToken)
    {
        return await _catalogueDbContext.Set<Summit>().ToListAsync(cancellationToken);
    }

    public async Task AddSummitRangeAsync(IEnumerable<Summit> summits, CancellationToken cancellationToken)
    {
        await _catalogueDbContext.Set<Summit>().AddRangeAsync(summits, cancellationToken);
    }

    public void ReplaceSummitRange(IEnumerable<Summit> summits)
    {
        _catalogueDbContext.Set<Summit>().UpdateRange(summits);
    }

    public void RemoveSummitRange(IEnumerable<Summit> summits)
    {
        _catalogueDbContext.Set<Summit>().RemoveRange(summits);
    }
}
