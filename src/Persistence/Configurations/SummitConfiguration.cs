﻿using Domain.CatalogueContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class SummitConfiguration : IEntityTypeConfiguration<Summit>
{
    public void Configure(EntityTypeBuilder<Summit> builder)
    {
        builder.ToTable("Summits");

        builder.HasKey(s => s.Id);
        builder.HasIndex(dl => dl.Name).IsUnique();

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(s => s.Altitude).HasColumnName("Altitude");
        builder.Property(s => s.Location).HasColumnName("Location");
        builder.Property(s => s.Name).HasColumnName("Name");
    }
}
