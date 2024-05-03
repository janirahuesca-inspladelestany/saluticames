using Application.ChallengeContext.Repositories;
using Domain.CatalogueContext.Entities;
using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories;

public class ChallengeRepository : IChallengeRepository
{
    private readonly DbSet<Climb> _climbs;

    public ChallengeRepository(SalutICamesDbContext catalogueDbContext)
    {
        _climbs = catalogueDbContext.Set<Climb>();
    }

    public async Task<IEnumerable<Climb>> GetClimbsByHikerIdAsync(Guid hikerId, CancellationToken cancellationToken = default)
    {
        var query = _climbs.Where(climb => climb.HikerId == hikerId);

        var climbs = await query.ToListAsync();

        return climbs;
    }
}
