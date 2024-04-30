using System.ComponentModel;

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

    public static T GetEnumValueByDescription<T>(string description) where T : Enum
    {
        var enumValues = Enum.GetValues(typeof(T)).Cast<T>();

        foreach (var value in enumValues)
        {
            var descriptionAttribute = value.GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            var valueDescription = descriptionAttribute?.Description ?? value.ToString();

            if (string.Equals(valueDescription, description, StringComparison.InvariantCultureIgnoreCase))
                return value;
        }

        throw new ArgumentException($"No enum value found with description: {description}", nameof(description));
    }
}