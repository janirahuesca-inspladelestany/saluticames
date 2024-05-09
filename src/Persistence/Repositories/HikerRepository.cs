using Application.ChallengeContext.Repositories;
using Domain.ChallengeContext.Entities;
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

    public async Task<Hiker?> GetByIdAsync(string hikerId, CancellationToken cancellation = default)
    {
        return await _hikers.FirstOrDefaultAsync(hiker => hiker.Id == hikerId, cancellation);
    }

    public async Task<Hiker?> FindByIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        return await _hikers.Include(h => h.Diaries).SingleOrDefaultAsync(hiker => hiker.Id.Equals(hikerId), cancellationToken);
    }

    public async Task Add(Hiker hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }

    public async Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        var hiker = await _hikers
            .Include(hiker => hiker.Diaries)
            .ThenInclude(diary => diary.Climbs)
            .FirstOrDefaultAsync(hiker => hiker.Id == hikerId);

        var climbs = hiker?.Diaries.SelectMany(d => d.Climbs).ToList();

        return climbs ?? Enumerable.Empty<Climb>();
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

    public async Task<IEnumerable<Diary>> ListDiariesByHikerIdAsync(string hikerId, Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Hiker> query = _hikers.Include(h => h.Diaries).AsQueryable();

        if (filter is not null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var hiker = orderBy is not null
            ? await orderBy(query).SingleOrDefaultAsync(h => h.Id.Equals(hikerId), cancellationToken)
            : await query.SingleOrDefaultAsync(h => h.Id.Equals(hikerId), cancellationToken);

        return hiker?.Diaries ?? Enumerable.Empty<Diary>();
    }

    public async Task<IDictionary<string, IEnumerable<Diary>>> ListDiariesAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Hiker> query = _hikers.Include(h => h.Diaries).AsQueryable();

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
}
