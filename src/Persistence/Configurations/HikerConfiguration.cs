﻿using Domain.Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class HikerConfiguration : IEntityTypeConfiguration<Hiker>
{
    public void Configure(EntityTypeBuilder<Hiker> builder)
    {
        builder.ToTable("Hikers");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
        builder.Property(h => h.Surname).HasMaxLength(100);

        builder.Property(h => h.Id).ValueGeneratedNever();

        builder.HasData(new
        {
            Id = "12345678P",
            Name = "Kilian",
            Surname = "Gordet"
        });
    }
}
