using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ClimbConfiguration : IEntityTypeConfiguration<ClimbEntity>
{
    public void Configure(EntityTypeBuilder<ClimbEntity> builder)
    {
        builder.ToTable("Climbs");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne<DiaryEntity>().WithMany(d => d.Climbs).HasForeignKey("DiaryId").OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<SummitAggregate>().WithMany().HasForeignKey(c => c.SummitId).OnDelete(DeleteBehavior.Cascade);
    }
}
