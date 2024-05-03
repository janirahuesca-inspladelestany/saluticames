using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class HikerConfiguration : IEntityTypeConfiguration<Hiker>
{
    public void Configure(EntityTypeBuilder<Hiker> builder)
    {
        builder.ToTable("Hikers");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Name).HasMaxLength(100);
        builder.Property(h => h.Surname).HasMaxLength(100);

        builder.Property(d => d.Id).ValueGeneratedNever();
    }
}
