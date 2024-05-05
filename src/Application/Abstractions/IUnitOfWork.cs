using Application.CatalogueContext.Repositories;
using Application.ChallengeContext.Repositories;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    ICatalogueRepository CatalogueRepository { get; }
    IHikerRepository HikerRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}