using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class DiaryConfiguration : IEntityTypeConfiguration<Diary>
{
    public void Configure(EntityTypeBuilder<Diary> builder)
    {
        builder.ToTable("Diaries");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).ValueGeneratedNever();
        builder.Property(d => d.Name).HasMaxLength(100);

        builder.HasMany(d => d.Climbs).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.Hiker).WithOne().HasForeignKey<Diary>("HikerId").OnDelete(DeleteBehavior.Cascade);
    }
}
