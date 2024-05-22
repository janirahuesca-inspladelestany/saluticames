using Microsoft.EntityFrameworkCore;
using Persistence.Data.Entities;
using System.ComponentModel;

namespace Persistence.Extensions;

public static class EnumLookupExtensions
{
    /// <summary>
    /// // Mètode d'extensió per ajudar a crear taules d'aspecte d'enumeració (enum lookup) per a propietats enum
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="createForeignKeys"></param>
    public static void CreateEnumLookupTable(this ModelBuilder modelBuilder, bool createForeignKeys = false)
    {
        // Iterar sobre les propietats de l'entitat del model
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).ToArray())
        {
            var entityType = property.DeclaringEntityType;
            var propertyType = property.ClrType;

            // Verificar si la propietat és un enum
            if (!propertyType.IsEnum)
                continue;

            // Crear el tipus concret per a EnumLookup<>
            var concreteType = typeof(EnumLookup<>).MakeGenericType(propertyType);
            var enumLookupBuilder = modelBuilder.Entity(concreteType);
            enumLookupBuilder.HasAlternateKey(nameof(EnumLookup<Enum>.Value));

            var data = Enum.GetValues(propertyType).Cast<object>()
                .Select(v =>
                {
                    var enumValue = (Enum)v;
                    var descriptionAttribute = enumValue.GetType().GetField(enumValue.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    var description = descriptionAttribute?.Description ?? enumValue.ToString();

                    return Activator.CreateInstance(concreteType, new object[] { v, description });
                })
                .ToArray();

            // Afegir les dades a la taula d'aspecte d'enumeració (enum lookup)
            enumLookupBuilder.HasData(data);

            // Crear claus foranes si és necessari
            if (createForeignKeys)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnName($"{property.Name}Id");

                modelBuilder.Entity(entityType.Name)
                    .HasOne(concreteType)
                    .WithMany()
                    .HasPrincipalKey(nameof(EnumLookup<Enum>.Value))
                    .HasForeignKey(property.Name);
            }
        }
    }
}