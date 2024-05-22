using Application.Abstractions;
using Application.Challenge.Repositories;
using Application.Content.Repositories;
using Persistence.Data;

namespace Persistence.Repositories;

// Classe que implementa la interfície IUnitOfWork
public sealed class UnitOfWork(SalutICamesDbContext _salutICamesDbContext, 
    ICatalogueRepository _catalogueRepository,
    ISummitRepository _summitRepository,
    IHikerRepository _hikerRepository) : IUnitOfWork
{
    // Retorna el repositori de catàlegs injectat a través del constructor
    public ICatalogueRepository CatalogueRepository => _catalogueRepository;

    // Retorna el repositori de cims injectat a través del constructor
    public ISummitRepository SummitRepository => _summitRepository;

    // Retorna el repositori d'excursionistes injectat a través del constructor
    public IHikerRepository HikerRepository => _hikerRepository;

    // Crida al mètode SaveChangesAsync del context de la base de dades
    // Desa tots els canvis pendents al context de manera segura
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _salutICamesDbContext.SaveChangesAsync(cancellationToken);
    }
}

/*
 * El patró UnitOfWork permet agrupar múltiples operacions de repositori en una única transacció, facilitant la gestió de la coherència i la integritat de les dades
 * Amb aquest patró, es pot garantir que totes les operacions de base de dades realitzades dins d'una unitat de treball es completin amb èxit o es facin revertir en cas d'error
 */