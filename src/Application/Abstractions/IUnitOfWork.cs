using Application.Challenge.Repositories;
using Application.Content.Repositories;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    ICatalogueRepository CatalogueRepository { get; }
    ISummitRepository SummitRepository { get; }
    IHikerRepository HikerRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}