using Application.Abstractions;
using Application.CatalogueContext.Repositories;
using Application.ChallengeContext.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class UnitOfWork(SalutICamesDbContext _salutICamesDbContext, 
    ICatalogueRepository _catalogueRepository,
    IHikerRepository _hikerRepository) : IUnitOfWork
{
    public ICatalogueRepository CatalogueRepository
    {
        get
        {
            return _catalogueRepository;
        }
    }

    public IHikerRepository HikerRepository
    {
        get
        {
            return _hikerRepository;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _salutICamesDbContext.SaveChangesAsync(cancellationToken);
    }
}
