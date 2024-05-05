using Application.ChallengeContext.Repositories;
using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class HikerRepository : IHikerRepository
{
    private readonly DbSet<Climb> _climbs;
    private readonly DbSet<Diary> _diaries;
    private readonly DbSet<Hiker> _hikers;

    public HikerRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _climbs = salutICamesDbContext.Set<Climb>();
        _diaries = salutICamesDbContext.Set<Diary>();
        _hikers = salutICamesDbContext.Set<Hiker>();
    }

    public async Task<Hiker?> GetByIdAsync(string hikerId, CancellationToken cancellation = default)
    {
        return await _hikers.FirstOrDefaultAsync(hiker => hiker.Id == hikerId, cancellation);
    }

    public async Task<Hiker?> FindByIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        return await _hikers.SingleOrDefaultAsync(hiker => hiker.Id.Equals(hikerId), cancellationToken);
    }

    public async Task Add(Hiker hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }

    public async Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        var query = _climbs
            .Include(c => c.Diary)
            .ThenInclude(d => d.Hiker)
            .Where(climb => climb.Diary.Hiker.Id == hikerId);

        var climbs = await query.ToListAsync();

        return climbs;
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

    public async Task<IEnumerable<Diary>> ListDiariesAsync(Expression<Func<Diary, bool>>? filter = null, Func<IQueryable<Diary>, IOrderedQueryable<Diary>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
    {
        IQueryable<Diary> query = _diaries;

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
}
