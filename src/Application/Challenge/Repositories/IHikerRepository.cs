using Application.Abstractions;
using Domain.Challenge.Entities;
using System.Linq.Expressions;

namespace Application.Challenge.Repositories;

// Interfície IHikerRepository que declara les operacions específiques per a la manipulació d'entitats Hiker
public interface IHikerRepository : IRepository<HikerAggregate, string>
{
    // Mètode per afegir un excursionista de manera asíncrona
    Task Add(HikerAggregate hiker, CancellationToken cancellationToken = default);

    // Mètode per obtenir una llista d'excursionistes de manera asíncrona amb opcions de filtre i ordenació
    Task<IEnumerable<HikerAggregate>> ListAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);

    // Mètode per trobar un excursionista per identificador de manera asíncrona
    Task<HikerAggregate?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    // Mètode per obtenir una llista de diaris d'un excursionista de manera asíncrona amb opcions de filtre i ordenació
    Task<IEnumerable<DiaryEntity>> ListDiariesAsync(string id, Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);

    // Mètode per obtenir una llista de diaris d'excursionistes de manera asíncrona amb opcions de filtre i ordenació  
    Task<IDictionary<string, IEnumerable<DiaryEntity>>> ListDiariesAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);

    // Mètode per obtenir una llista d'ascensions per identificador d'excursionista de manera asíncrona
    Task<IEnumerable<ClimbEntity>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default);
}
