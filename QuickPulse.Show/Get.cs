using System.Reflection;
using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class Get
{
    public static IEnumerable<PropertyInfo> Properties(object input, Ministers ministers) =>
        input.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ministers.ShouldNotBeIgnored(input.GetType(), a));

    public static IEnumerable<FieldInfo> Fields(object input, Ministers ministers) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ministers.ShouldNotBeIgnored(input.GetType(), a));

    public static IEnumerable<ObjectProperty> ObjectProperties(object input, Ministers ministers) =>
        Properties(input, ministers).Select(a => new ObjectProperty(a.Name, a.GetValue(input)!)).Union(
        Fields(input, ministers).Select(a => new ObjectProperty(a.Name, a.GetValue(input)!)));

    public static IEnumerable<object> FieldValues(object input, Ministers ministers) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ministers.ShouldNotBeIgnored(input.GetType(), a))
            .Select(a => a.GetValue(input)!);

    public static (object, object) KeyValueAsTuple(object input) =>
        (input.GetType().GetProperty("Key")?.GetValue(input)!,
        input.GetType().GetProperty("Value")?.GetValue(input)!);
}