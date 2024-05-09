using Domain.CatalogueContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class CatalogueConfiguration : IEntityTypeConfiguration<Catalogue>
{
    public void Configure(EntityTypeBuilder<Catalogue> builder)
    {
        builder.ToTable("Catalogues");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(100);

        var catalogue = Catalogue.Create(id: Guid.Parse("3a711b1c-a40a-48b2-88e9-c1677591d546"), name: "Repte dels 100 Cims de la FEEC");
        builder.HasData(catalogue);
    }
}
