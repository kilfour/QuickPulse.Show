
using System.Reflection;

namespace QuickPulse.Show.Reflects;

public static class PropertyPredicates
{
    public static bool PropertyNamed(this Type type, PropertyInfo propertyInfo, string propertyName)
        => propertyInfo.DeclaringType == type
            && string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);

    public static bool PropertyNamed(this PropertyInfo propertyInfo, string propertyName)
        => string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);

    public static bool PropertyNamed<T>(this PropertyInfo propertyInfo, string propertyName)
        => propertyInfo.PropertyType == typeof(T) && PropertyNamed(propertyInfo, propertyName);

    public static bool IsEntityId<T>(this PropertyInfo propertyInfo)
        => propertyInfo.PropertyType is T
        && (propertyInfo.PropertyNamed("id")
        || propertyInfo.PropertyNamed($"{propertyInfo.DeclaringType?.Name}id"));
}
