using Application.Abstractions;
using Application.CatalogueContext.Repositories;
using Application.ChallengeContext.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

public sealed class UnitOfWork(SalutICamesDbContext _salutICamesDbContext, 
    ICatalogueRepository _catalogueRepository,
    IDiaryRepository _diaryRepository) : IUnitOfWork
{
    public ICatalogueRepository CatalogueRepository
    {
        get
        {
            return _catalogueRepository;
        }
    }

    public IDiaryRepository DiaryRepository
    {
        get
        {
            return _diaryRepository;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _salutICamesDbContext.SaveChangesAsync(cancellationToken);
    }
}
