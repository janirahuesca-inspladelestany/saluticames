using Application.Catalogues.Repositories;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    ICatalogueRepository CatalogueRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}