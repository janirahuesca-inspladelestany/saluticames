using Application.Content.Repositories;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class SummitRepository : ISummitRepository
{
    private readonly DbSet<SummitAggregate> _summits;

    // Constructor que rep una instància de DbContext per accedir a la base de dades
    public SummitRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _summits = salutICamesDbContext.Set<SummitAggregate>();
    }

    /// <summary>
    /// Mètode per afegir una col·lecció d'entitats SummitAggregate a la base de dades
    /// </summary>
    /// <param name="summits"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna una llista d'objectes SummitAggregate</returns>
    public async Task AddRangeAsync(IEnumerable<SummitAggregate> summits, CancellationToken cancellationToken = default)
    {
        await _summits.AddRangeAsync(summits, cancellationToken);
    }

    /// <summary>
    /// Mètode per afegir una sola entitat SummitAggregate a la base de dades
    /// </summary>
    /// <param name="summit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna un objecte SummitAggregate</returns>
    public async Task AddAsync(SummitAggregate summit, CancellationToken cancellationToken = default)
    {
        await _summits.AddAsync(summit, cancellationToken);
    }

    /// <summary>
    /// Mètode per llistar els cims
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna una llista d'objectes SummitAggregate basada en els criteris de filtrat i ordenació proporcionats</returns>
    public async Task<IEnumerable<SummitAggregate>> ListAsync(Expression<Func<SummitAggregate, bool>>? filter = null,
        Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        // Inicialitza la consulta sobre la col·lecció de summits
        IQueryable<SummitAggregate> query = _summits;

        // Aplica el filtre si s'ha proporcionat
        if (filter is not null) query = query.Where(filter);

        // Ordena i executa la consulta asíncronament, retornant els resultats com una llista
        var summits = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return summits;
    }

    /// <summary>
    /// Mètode per trobar un cim per ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna l'objecte SummitAggregate si es troba un cim amb l'identificador proporcionat, sinó es retorna null</returns>
    public async Task<SummitAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _summits.FindAsync(id, cancellationToken);
    }

    /// <summary>
    /// Mètode per eliminar una col·lecció de SummitAggregate de la base de dades
    /// </summary>
    /// <param name="summits"></param>
    public void RemoveRange(IEnumerable<SummitAggregate> summits)
    {
        _summits.RemoveRange(summits);
    }

    /// <summary>
    /// Mètode per eliminar una sola entitat SummitAggregate de la base de dades
    /// </summary>
    /// <param name="summit"></param>
    public void Remove(SummitAggregate summit)
    {
        _summits.Remove(summit);
    }
}
