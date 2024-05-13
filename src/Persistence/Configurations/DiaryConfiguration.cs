using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class DiaryConfiguration : IEntityTypeConfiguration<DiaryEntity>
{
    public void Configure(EntityTypeBuilder<DiaryEntity> builder)
    {
        builder.ToTable("Diaries");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).ValueGeneratedNever();
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);

        builder.HasOne<HikerAggregate>().WithMany(h => h.Diaries).HasForeignKey("HikerId").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne<CatalogueAggregate>().WithMany().HasForeignKey(d => d.CatalogueId).OnDelete(DeleteBehavior.NoAction);

        builder.HasData(new
        {
            Id = Guid.NewGuid(),
            Name = "El meu diari dels 100 cims de la FEEC",
            CatalogueId = Guid.Parse("3a711b1c-a40a-48b2-88e9-c1677591d546"),
            HikerId = "12345678P"
        });
    }
}
