using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence.Data;

public sealed class SalutICamesDbContext : DbContext
{
    public SalutICamesDbContext(DbContextOptions<SalutICamesDbContext> options)
        : base(options)
    {

    }

    public DbSet<Catalogue> Catalogues { get; set; } = null!;
    public DbSet<CatalogueSummit> CatalogueSummits { get; set; } = null!;
    public DbSet<Summit> Summits { get; set; } = null!;
    public DbSet<Hiker> Hikers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalutICamesDbContext).Assembly);
        modelBuilder.CreateEnumLookupTable(createForeignKeys: true);
    }
}
