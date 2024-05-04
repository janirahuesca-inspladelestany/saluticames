using Application.ChallengeContext.Repositories;
using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class DiaryRepository : IDiaryRepository
{
    private readonly DbSet<Climb> _climbs;
    private readonly DbSet<Diary> _diaries;
    private readonly DbSet<Hiker> _hikers;

    public DiaryRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _climbs = salutICamesDbContext.Set<Climb>();
        _diaries = salutICamesDbContext.Set<Diary>();
        _hikers = salutICamesDbContext.Set<Hiker>();
    }

    public async Task Add(Diary diary, CancellationToken cancellationToken = default)
    {
        await _diaries.AddAsync(diary, cancellationToken);
    }

    public async Task<Diary?> GetByHikerIdAsync(string hikerId, CancellationToken cancellation = default)
    {
        return await _diaries.FirstOrDefaultAsync(diary => diary.Hiker.Id == hikerId, cancellation);
    }

    public async Task<Hiker?> FindHikerByIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        return await _hikers.SingleOrDefaultAsync(hiker => hiker.Id.Equals(hikerId), cancellationToken);
    }

    public async Task AddHiker(Hiker hiker, CancellationToken cancellationToken = default)
    {
        await _hikers.AddAsync(hiker, cancellationToken);
    }

    public async Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(string hikerId, CancellationToken cancellationToken = default)
    {
        var query = _climbs.Where(climb => climb.HikerId.Equals(hikerId));

        var climbs = await query.ToListAsync();

        return climbs;
    }

    public async Task<IEnumerable<Hiker>> ListHikersAsync(Expression<Func<Hiker, bool>>? filter = null, Func<IQueryable<Hiker>, IOrderedQueryable<Hiker>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<Diary>> ListAsync(Expression<Func<Diary, bool>>? filter = null, Func<IQueryable<Diary>, IOrderedQueryable<Diary>>? orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
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
