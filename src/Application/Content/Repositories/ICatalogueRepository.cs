using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

// Interfície ICatalogueRepository que declara les operacions específiques per a la manipulació d'entitats de catàleg
public interface ICatalogueRepository : IRepository<CatalogueAggregate, Guid> // Implementa IRepository amb CatalogueAggregate com a tipus d'entitat i Guid com a tipus d'identificador
{
    // Mètode per obtenir una llista de catàlegs de manera asíncrona amb opcions de filtre i ordenació
    Task<IEnumerable<CatalogueAggregate>> ListAsync(Expression<Func<CatalogueAggregate, bool>>? filter = null, Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>? orderBy = null, CancellationToken cancellationToken = default);

    // Mètode per trobar un catàleg per identificador de manera asíncrona
    Task<CatalogueAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
