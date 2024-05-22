using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class CatalogueConfiguration : IEntityTypeConfiguration<CatalogueAggregate>
{
    public void Configure(EntityTypeBuilder<CatalogueAggregate> builder)
    {
        // Configurar la taula a la base de dades
        builder.ToTable("Catalogues");

        // Definir la clau primària
        builder.HasKey(c => c.Id);

        // Configurar les propietats de la taula
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        // Afegir dades inicials a la taula
        builder.HasData(
            new { Id = Guid.Parse("3a711b1c-a40a-48b2-88e9-c1677591d546"), Name = "Repte dels 100 Cims de la FEEC" });
    }
}
