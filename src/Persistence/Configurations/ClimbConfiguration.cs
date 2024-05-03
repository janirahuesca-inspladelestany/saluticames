using Domain.ChallengeContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ClimbConfiguration : IEntityTypeConfiguration<Climb>
{
    public void Configure(EntityTypeBuilder<Climb> builder)
    {
        builder.ToTable("Climbs");

        builder.HasKey(c => c.Id);
        builder.HasAlternateKey(c => new { c.HikerId, c.SummitId });

        builder.Property(d => d.Id).ValueGeneratedNever();
    }
}
