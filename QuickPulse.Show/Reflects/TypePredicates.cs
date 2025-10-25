
using System.Reflection;

namespace QuickPulse.Show.Reflects;

public static class TypePredicates
{
    public static bool HasProperty(this Type type, Func<PropertyInfo, bool> predicate)
        => type.GetProperties().Any(predicate);

    public static bool HasPropertyNamed<T>(this Type type, string propertyName)
        => type.HasProperty(b => b.PropertyNamed<T>(propertyName));

    public static object GetValueFor(this object target, string propertyName)
    {
        return target.GetType()
            .GetProperty(propertyName)!
            .GetValue(target)!;
    }
}