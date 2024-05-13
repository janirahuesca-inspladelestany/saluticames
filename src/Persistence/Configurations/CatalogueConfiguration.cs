using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class CatalogueConfiguration : IEntityTypeConfiguration<CatalogueAggregate>
{
    public void Configure(EntityTypeBuilder<CatalogueAggregate> builder)
    {
        builder.ToTable("Catalogues");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            new { Id = Guid.Parse("3a711b1c-a40a-48b2-88e9-c1677591d546"), Name = "Repte dels 100 Cims de la FEEC" });
    }
}
