using Domain.Challenge.Entities;
using Domain.Content.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ClimbConfiguration : IEntityTypeConfiguration<Climb>
{
    public void Configure(EntityTypeBuilder<Climb> builder)
    {
        builder.ToTable("Climbs");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne<Diary>().WithMany(d => d.Climbs).HasForeignKey("DiaryId").OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Summit>().WithMany().HasForeignKey(c => c.SummitId).OnDelete(DeleteBehavior.Cascade);
    }
}
