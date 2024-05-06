using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Persistence.Extensions;

public static class EnumLookupExtensions
{
    public static void CreateEnumLookupTable(this ModelBuilder modelBuilder, bool createForeignKeys = false)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).ToArray())
        {
            var entityType = property.DeclaringEntityType;
            var propertyType = property.ClrType;

            if (!propertyType.IsEnum)
                continue;

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

            enumLookupBuilder.HasData(data);

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