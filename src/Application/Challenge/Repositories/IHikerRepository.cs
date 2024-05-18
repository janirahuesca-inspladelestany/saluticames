using Application.Abstractions;
using Domain.Challenge.Entities;
using System.Linq.Expressions;

namespace Application.Challenge.Repositories;

public interface IHikerRepository : IRepository<HikerAggregate, string>
{
    Task Add(HikerAggregate hiker, CancellationToken cancellationToken = default);
    Task<IEnumerable<HikerAggregate>> ListAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);
    Task<HikerAggregate?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DiaryEntity>> ListDiariesAsync(string id, Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);
    Task<IDictionary<string, IEnumerable<DiaryEntity>>> ListDiariesAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClimbEntity>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default);
}
