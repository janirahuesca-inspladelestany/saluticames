using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ClimbConfiguration : IEntityTypeConfiguration<ClimbEntity>
{
    public void Configure(EntityTypeBuilder<ClimbEntity> builder)
    {
        // Configurar la taula a la base de dades
        builder.ToTable("Climbs");

        // Definir la clau primària
        builder.HasKey(c => c.Id);

        // Configurar propietats
        builder.Property(c => c.Id).ValueGeneratedNever();

        // Configurar relacions amb altres entitats
        builder.HasOne<DiaryEntity>().WithMany(d => d.Climbs).HasForeignKey("DiaryId").OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<SummitAggregate>().WithMany().HasForeignKey(c => c.SummitId).OnDelete(DeleteBehavior.Cascade);
    }
}
