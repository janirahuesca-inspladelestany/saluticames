using Domain.Catalogues.Entities;
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

        builder.HasMany(c => c.Summits).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.HasData(Catalogue.Create("Repte dels 100 Cims de la FEEC"));
    }
}
