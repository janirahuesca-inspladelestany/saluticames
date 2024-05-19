using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Domain.Content.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence.Data;

public sealed class SalutICamesDbContext : IdentityDbContext<IdentityUser>
{
    public SalutICamesDbContext(DbContextOptions<SalutICamesDbContext> options)
        : base(options)
    {

    }

    public DbSet<CatalogueAggregate> Catalogues { get; set; } = null!;
    public DbSet<CatalogueSummit> CatalogueSummits { get; set; } = null!;
    public DbSet<SummitAggregate> Summits { get; set; } = null!;
    public DbSet<HikerAggregate> Hikers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalutICamesDbContext).Assembly);
        modelBuilder.CreateEnumLookupTable(createForeignKeys: true);
    }
}
