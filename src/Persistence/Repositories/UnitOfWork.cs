using Application.Abstractions;
using Application.Catalogues.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class UnitOfWork(CatalogueDbContext _catalogueDbContext) : IUnitOfWork
{
    private CatalogueRepository _catalogueRepository = null!;

    public ICatalogueRepository CatalogueRepository
    {
        get
        {
            _catalogueRepository ??= new CatalogueRepository(_catalogueDbContext);
            return _catalogueRepository;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _catalogueDbContext.SaveChangesAsync(cancellationToken);
    }
}
