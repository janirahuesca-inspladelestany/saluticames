using Domain.Challenge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class HikerConfiguration : IEntityTypeConfiguration<HikerAggregate>
{
    public void Configure(EntityTypeBuilder<HikerAggregate> builder)
    {
        // Configurar la taula a la base de dades
        builder.ToTable("Hikers");

        // Definir la clau primària
        builder.HasKey(h => h.Id);

        // Configurar propietats
        builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
        builder.Property(h => h.Surname).HasMaxLength(100);
        builder.Property(h => h.Id).ValueGeneratedNever();

        // Afegir dades inicials a la taula
        builder.HasData(new
        {
            Id = "12345678P",
            Name = "Kilian",
            Surname = "Gordet"
        });
    }
}
