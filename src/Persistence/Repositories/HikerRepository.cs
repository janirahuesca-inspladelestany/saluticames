using Application.Challenge.Repositories;
using Domain.Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class HikerRepository : IHikerRepository
{
    private readonly DbSet<HikerAggregate> _hikers;

    // Constructor que rep una instància de DbContext per accedir a la base de dades
    public HikerRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _hikers = salutICamesDbContext.Set<HikerAggregate>();
    }

    /// <summary>
    /// Mètode per afegir un nou excursionista
    /// </summary>
    /// <param name="hiker"></param>
    /// <param name="cancellationToken"></param>
    public async Task Add(HikerAggregate hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }

    /// <summary>
    /// Mètode per llistar els excursionistes
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna una llista d'objectes HikerAggregate basada en els criteris de filtrat i ordenació proporcionats</returns>
    public async Task<IEnumerable<HikerAggregate>> ListAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        // Inicia una consulta sobre la taula Hikers incloent la navegació cap a Diaries i Climbs
        IQueryable<HikerAggregate> query = _hikers.Include(h => h.Diaries).ThenInclude(d => d.Climbs);

        // Si hi ha un filtre, s'aplica a la consulta
        if (filter is not null) query = query.Where(filter);

        // Executa la consulta i aplica l'ordre especificat si es proporciona, o simplement converteix la consulta en una llista
        var hikers = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return hikers;
    }

    /// <summary>
    /// Mètode per trobar un excursionista per ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna l'objecte HikerAggregate si es troba un catàleg amb l'identificador proporcionat, sinó es retorna null</returns>
    public async Task<HikerAggregate?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _hikers.Include(hiker => hiker.Diaries).ThenInclude(diary => diary.Climbs).SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Mètode per llistar els diaris d'un hiker específic pel seu id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna els diaris del Hiker si es troba, o una col·lecció buida si no es troba</returns>
    public async Task<IEnumerable<DiaryEntity>> ListDiariesAsync(string id, Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        // Inicia una consulta sobre la taula Hikers incloent la navegació cap a Diaries
        IQueryable<HikerAggregate> query = _hikers
            .Include(hiker => hiker.Diaries)
            .AsNoTracking()
            .AsQueryable();

        // Si hi ha un filtre, s'aplica a la consulta
        if (filter is not null) query = query.Where(filter);

        // Executa la consulta i troba el Hiker per ID amb l'ordre especificat, si es proporciona
        var hiker = orderBy is not null
            ? await orderBy(query).SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken)
            : await query.SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken);

        return hiker?.Diaries ?? Enumerable.Empty<DiaryEntity>();
    }

    /// <summary>
    /// Mètode per llistar els diaris de tots els hikers, agrupats pel seu id
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna una llista de Hikers en un diccionari on la clau és el ID del Hiker i el valor és la seva col·lecció de Diaries</returns>
    public async Task<IDictionary<string, IEnumerable<DiaryEntity>>> ListDiariesAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        // Inicia una consulta sobre la taula Hikers incloent la navegació cap a Diaries
        IQueryable<HikerAggregate> query = _hikers
            .Include(h => h.Diaries)
            .AsNoTracking()
            .AsQueryable();

        // Si hi ha un filtre, s'aplica a la consulta
        if (filter is not null) query = query.Where(filter);

        // Executa la consulta i obté la llista de Hikers amb l'ordre especificat, si es proporciona
        var hikers = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return hikers.ToDictionary(hiker => hiker.Id, hiker => hiker.Diaries);
    }

    /// <summary>
    /// Mètode per llistar les ascensions d'un hiker específic pel seu id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Retorna la llista d'ascensions</returns>
    public async Task<IEnumerable<ClimbEntity>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Troba un Hiker específic pel seu ID, incloent la navegació cap a Diaries i Climbs
        var hiker = await _hikers
            .Include(hiker => hiker.Diaries)
            .ThenInclude(diary => diary.Climbs)
            .AsNoTracking()
            .SingleOrDefaultAsync(hiker => hiker.Id == id);

        // Si el Hiker no es troba, retorna una col·lecció buida
        if (hiker is null) return Enumerable.Empty<ClimbEntity>();

        // Agrega totes les ascensions(Climbs) de tots els diaris del Hiker en una sola llista
        var climbs = hiker.Diaries.SelectMany(diary => diary.Climbs).ToList();

        return climbs;
    }
}
