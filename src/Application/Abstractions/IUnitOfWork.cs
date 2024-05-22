using Application.Challenge.Repositories;
using Application.Content.Repositories;

namespace Application.Abstractions;

// Interfície IUnitOfWork que declara les operacions per a una unitat de treball
public interface IUnitOfWork
{
    // Propietat per accedir al repositori de catàlegs
    ICatalogueRepository CatalogueRepository { get; }

    // Propietat per accedir al repositori de cims
    ISummitRepository SummitRepository { get; }

    // Propietat per accedir al repositori de excursionistes
    IHikerRepository HikerRepository { get; }

    // Mètode per desar els canvis a la base de dades de manera asíncrona
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}