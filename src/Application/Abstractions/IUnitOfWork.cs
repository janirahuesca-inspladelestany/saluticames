using Application.CatalogueContext.Repositories;
using Application.ChallengeContext.Repositories;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    ICatalogueRepository CatalogueRepository { get; }
    IChallengeRepository ChallengeRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}