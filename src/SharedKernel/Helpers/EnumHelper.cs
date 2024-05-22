using System.ComponentModel;
using System.Reflection;

namespace SharedKernel.Helpers;

// Classe estàtica EnumHelper que conté mètodes auxiliars per treballar amb enums i les seves descripcions
public static class EnumHelper
{
    /// <summary>
    /// // Mètode per comprovar si un valor d'enum està definit per una descripció específica
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="description"></param>
    /// <returns></returns>
    public static bool IsDefinedByDescription<T>(string description) where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Any(value =>
            {
                var descriptionAttribute = value.GetType().GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                var valueDescription = descriptionAttribute?.Description ?? value.ToString();

                return string.Equals(valueDescription, description, StringComparison.InvariantCultureIgnoreCase);
            });
    }

    /// <summary>
    /// Mètode per obtenir un valor d'enum a partir de la seva descripció
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="description"></param>
    /// <param name="region"></param>
    /// <returns></returns>
    public static bool TryGetEnumValueByDescription<T>(string? description, out T? region) where T : Enum
    {
        region = default;

        if (string.IsNullOrEmpty(description)) return false;

        var enumValues = Enum.GetValues(typeof(T)).Cast<T>();

        foreach (var value in enumValues)
        {
            var descriptionAttribute = value.GetType().GetField(value.ToString())?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            var valueDescription = descriptionAttribute?.Description ?? value.ToString();

            if (string.Equals(valueDescription, description, StringComparison.InvariantCultureIgnoreCase))
            {
                region = value;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Mètode per obtenir la descripció d'un valor d'enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription<T>(T value) where T : Enum
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
        return attribute != null ? attribute.Description : value.ToString();
    }
}