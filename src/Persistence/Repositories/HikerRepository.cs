using Application.Challenge.Repositories;
using Domain.Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class HikerRepository : IHikerRepository
{
    private readonly DbSet<HikerAggregate> _hikers;

    public HikerRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _hikers = salutICamesDbContext.Set<HikerAggregate>();
    }

    public async Task Add(HikerAggregate hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }
    
    public async Task<IEnumerable<HikerAggregate>> ListAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<HikerAggregate> query = _hikers;

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var hikers = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return hikers;
    }
    
    public async Task<HikerAggregate?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _hikers.Include(hiker => hiker.Diaries).ThenInclude(diary => diary.Climbs).SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken);
    }

    public async Task<IEnumerable<DiaryEntity>> ListDiariesAsync(string id, Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<HikerAggregate> query = _hikers
            .Include(hiker => hiker.Diaries)
            .AsNoTracking()
            .AsQueryable();

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var hiker = orderBy is not null
            ? await orderBy(query).SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken)
            : await query.SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken);

        return hiker?.Diaries ?? Enumerable.Empty<DiaryEntity>();
    }

    public async Task<IDictionary<string, IEnumerable<DiaryEntity>>> ListDiariesAsync(Expression<Func<HikerAggregate, bool>>? filter = null, Func<IQueryable<HikerAggregate>, IOrderedQueryable<HikerAggregate>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<HikerAggregate> query = _hikers
            .Include(h => h.Diaries)
            .AsNoTracking()
            .AsQueryable();

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var hikers = orderBy is not null
            ? await orderBy(query).ToListAsync(cancellationToken)
            : await query.ToListAsync(cancellationToken);

        return hikers.ToDictionary(hiker => hiker.Id, hiker => hiker.Diaries);
    }
    
    public async Task<IEnumerable<ClimbEntity>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var hiker = await _hikers
            .Include(hiker => hiker.Diaries)
            .ThenInclude(diary => diary.Climbs)
            .AsNoTracking()
            .SingleOrDefaultAsync(hiker => hiker.Id == id);

        if (hiker is null) return Enumerable.Empty<ClimbEntity>();

        var climbs = hiker.Diaries.SelectMany(diary => diary.Climbs).ToList();

        return climbs;
    }
}
