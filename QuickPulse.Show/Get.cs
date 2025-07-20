using System.Reflection;
using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class Get
{
    public static IEnumerable<PropertyInfo> Properties(object input, Box<FlowContext> context) =>
        input.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => context.Value.ShouldNotBeIgnored(input.GetType(), a));

    public static IEnumerable<FieldInfo> Fields(object input, Box<FlowContext> context) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => context.Value.ShouldNotBeIgnored(input.GetType(), a));

    public static IEnumerable<object> FieldValues(object input, Box<FlowContext> context) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => context.Value.ShouldNotBeIgnored(input.GetType(), a))
            .Select(a => a.GetValue(input)!);
}