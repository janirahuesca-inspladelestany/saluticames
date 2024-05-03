using Domain.CatalogueContext.Entities;
using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence.Data;

public sealed class SalutICamesDbContext : DbContext
{
    public SalutICamesDbContext(DbContextOptions<SalutICamesDbContext> options)
        : base(options)
    {

    }

    public DbSet<Catalogue> Catalogue { get; set; } = null!;
    public DbSet<Diary> Diary { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalutICamesDbContext).Assembly);
        modelBuilder.CreateEnumLookupTable(createForeignKeys: true);

        base.OnModelCreating(modelBuilder);
    }
}
