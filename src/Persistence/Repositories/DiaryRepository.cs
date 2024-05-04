using Application.ChallengeContext.Repositories;
using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public class DiaryRepository : IDiaryRepository
{
    private readonly DbSet<Diary> _diaries;
    private readonly DbSet<Climb> _climbs;

    public DiaryRepository(SalutICamesDbContext salutICamesDbContext)
    {
        _diaries = salutICamesDbContext.Set<Diary>();
        _climbs = salutICamesDbContext.Set<Climb>();
    }

    public Task<Diary?> GetByHikerId(Guid hikerId, CancellationToken cancellation = default)
    {
        return _diaries.FirstOrDefaultAsync(diary => diary.Hiker.Id == hikerId, cancellation);
    }

    public async Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(Guid hikerId, CancellationToken cancellationToken = default)
    {
        var query = _climbs.Where(climb => climb.HikerId == hikerId);

        var climbs = await query.ToListAsync();

        return climbs;
    }
}
