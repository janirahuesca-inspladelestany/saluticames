using Application.Challenge.Repositories;
using Domain.Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class HikerRepository : IHikerRepository
{
    private readonly DbSet<Hiker> _hikers;

    public HikerRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _hikers = salutICamesDbContext.Set<Hiker>();
    }

    public async Task Add(Hiker hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }
    
    public async Task<IEnumerable<Hiker>> ListAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Hiker> query = _hikers;

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
    
    public async Task<Hiker?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _hikers.Include(hiker => hiker.Diaries).ThenInclude(diary => diary.Climbs).SingleOrDefaultAsync(hiker => hiker.Id.Equals(id), cancellationToken);
    }

    public async Task<IEnumerable<Diary>> ListDiariesAsync(string id, Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Hiker> query = _hikers
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

        return hiker?.Diaries ?? Enumerable.Empty<Diary>();
    }

    public async Task<IDictionary<string, IEnumerable<Diary>>> ListDiariesAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Hiker> query = _hikers
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
    
    public async Task<IEnumerable<Climb>> ListClimbsByHikerIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var hiker = await _hikers
            .Include(hiker => hiker.Diaries)
            .ThenInclude(diary => diary.Climbs)
            .AsNoTracking()
            .SingleOrDefaultAsync(hiker => hiker.Id == id);

        if (hiker is null) return Enumerable.Empty<Climb>();

        var climbs = hiker.Diaries.SelectMany(diary => diary.Climbs).ToList();

        return climbs;
    }
}
