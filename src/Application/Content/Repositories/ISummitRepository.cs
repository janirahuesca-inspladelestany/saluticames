using Application.Abstractions;
using Domain.Content.Entities;
using System.Linq.Expressions;

namespace Application.Content.Repositories;

// Interfície ISummitRepository que declara les operacions específiques per a la manipulació d'entitats de cim
public interface ISummitRepository : IRepository<SummitAggregate, Guid>
{
    // Mètode per afegir una gamma d'entitats de cim de manera asíncrona
    Task AddRangeAsync(IEnumerable<SummitAggregate> summits, CancellationToken cancellationToken = default);

    // Mètode per afegir una entitat de cim de manera asíncrona
    Task AddAsync(SummitAggregate summit, CancellationToken cancellationToken = default);

    // Mètode per obtenir una llista de cims de manera asíncrona amb opcions de filtre i ordenació
    Task<IEnumerable<SummitAggregate>> ListAsync(Expression<Func<SummitAggregate, bool>>? filter = null, Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>? orderBy = null, CancellationToken cancellationToken = default);

    // Mètode per trobar un cim per identificador de manera asíncrona
    Task<SummitAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Mètode per eliminar una gamma d'entitats de cim
    void RemoveRange(IEnumerable<SummitAggregate> summits);

    // Mètode per eliminar una entitat de cim
    void Remove(SummitAggregate summit);
}
