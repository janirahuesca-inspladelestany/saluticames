using System.ComponentModel;
using System.Reflection;

namespace SharedKernel.Helpers;

public static class EnumHelper
{
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

    public static string GetDescription<T>(T value) where T : Enum
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
        return attribute != null ? attribute.Description : value.ToString();
    }
}