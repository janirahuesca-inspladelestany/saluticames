using Application.Content.Repositories;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public sealed class CatalogueRepository : ICatalogueRepository
{
    private readonly DbSet<CatalogueAggregate> _catalogues;

    // Constructor que rep una instància de DbContext per accedir a la base de dades
    public CatalogueRepository(SalutICamesDbContext salutICamesDbContext)
    {
        // Inicialitza el DbSet per interactuar amb la taula de catàlegs a la base de dades
        _catalogues = salutICamesDbContext.Set<CatalogueAggregate>();
    }

    /// <summary>
    /// Mètode per obtenir una llista de catàlegs amb opcions de filtrat i ordenació
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna una llista d'objectes CatalogueAggregate basada en els criteris de filtrat i ordenació proporcionats</returns>
    public async Task<IEnumerable<CatalogueAggregate>> ListAsync(Expression<Func<CatalogueAggregate, bool>>? filter = null,
        Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        // Inicialitza la consulta amb inclusió de les relacions necessàries
        IQueryable<CatalogueAggregate> query = _catalogues.Include(c => c.CatalogueSummits);

        // Aplica el filtre si és proporcionat
        if (filter is not null) query = query.Where(filter);

        // Ordena la consulta si es proporciona una funció d'ordenació
        var catalogues = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return catalogues;
    }

    /// <summary>
    /// Mètode per trobar un catàleg pel seu identificador
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna l'objecte CatalogueAggregate si es troba un catàleg amb l'identificador proporcionat, sinó es retorna null</returns>
    public async Task<CatalogueAggregate?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Utilitza Include per incloure les relacions necessàries i busca el catàleg pel seu identificador
        return await _catalogues.Include(c => c.CatalogueSummits).SingleOrDefaultAsync(catalogue => catalogue.Id == id, cancellationToken);
    }
}
