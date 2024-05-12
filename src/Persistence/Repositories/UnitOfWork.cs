using Application.Abstractions;
using Application.Challenge.Repositories;
using Application.Content.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class UnitOfWork(SalutICamesDbContext _salutICamesDbContext, 
    ICatalogueRepository _catalogueRepository,
    ISummitRepository _summitRepository,
    IHikerRepository _hikerRepository) : IUnitOfWork
{
    public ICatalogueRepository CatalogueRepository => _catalogueRepository;
    public ISummitRepository SummitRepository => _summitRepository;
    public IHikerRepository HikerRepository => _hikerRepository;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _salutICamesDbContext.SaveChangesAsync(cancellationToken);
    }
}
