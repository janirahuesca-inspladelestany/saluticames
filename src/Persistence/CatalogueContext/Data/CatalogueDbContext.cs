using Microsoft.EntityFrameworkCore;

namespace Persistence.CatalogueContext.Data;

public class CatalogueDbContext : DbContext
{
    public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options)
        : base(options)
    {

    }

    public DbSet<CatalogueEntity> Catalogue { get; set; }
    public DbSet<SummitEntity> Summit { get; set; }
    public DbSet<RegionEntity> Region { get; set; }
    public DbSet<DifficultyEntity> Difficulty { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SummitEntity>().HasOne(s => s.Catalogue).WithMany(c => c.Summits).HasForeignKey(s => s.CatalogueId);
        modelBuilder.Entity<SummitEntity>().HasOne(s => s.Region).WithMany().HasForeignKey(s => s.RegionId);
        modelBuilder.Entity<SummitEntity>().HasOne(s => s.Difficulty).WithMany().HasForeignKey(s => s.DifficultyId);
    }
}
