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
    // Constructor que rep les opcions del context de la base de dades
    public SalutICamesDbContext(DbContextOptions<SalutICamesDbContext> options)
        : base(options)
    {

    }

    // DbSet per a les entitats de la base de dades
    public DbSet<CatalogueAggregate> Catalogues { get; set; } = null!;
    public DbSet<CatalogueSummit> CatalogueSummits { get; set; } = null!;
    public DbSet<SummitAggregate> Summits { get; set; } = null!;
    public DbSet<HikerAggregate> Hikers { get; set; } = null!;

    // Mètode que s'executa durant la configuració del model
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuracions de les classes que configuren l'estructura de les taules
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalutICamesDbContext).Assembly);
        
        // Crear taules per a les enumeracions i configurar relacions amb altres entitats si cal
        modelBuilder.CreateEnumLookupTable(createForeignKeys: true);
    }
}
