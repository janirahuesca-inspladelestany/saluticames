using Domain.Catalogues.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence.Data;

public sealed class CatalogueDbContext : DbContext
{
    public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options)
        : base(options)
    {

    }

    public DbSet<Catalogue> Catalogue { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogueDbContext).Assembly);
        modelBuilder.CreateEnumLookupTable(createForeignKeys: true);

        base.OnModelCreating(modelBuilder);
    }
}
