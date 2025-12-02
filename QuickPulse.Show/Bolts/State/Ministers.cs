

using System.Collections;
using System.Reflection;
using QuickPulse.Show.Bolts.State;

namespace QuickPulse.Show.Bolts;

public record Ministers
{
    public HashSet<Type> InlinedTypes { get; init; } = [];
    public bool NeedsInlining(object input) => InlinedTypes.Contains(input.GetType());
    public bool NeedsInlining(IEnumerable input) =>
        InlinedTypes.Contains(input.GetType()) || input.Count() == 0;

    public PrimitivesRegistry Registry { get; init; } = new PrimitivesRegistry();
    public Func<object?, string> GetFormatFunction(object obj) =>
        Registry.Get(obj.GetType()) ?? (x => x!.ToString()!);


    public Dictionary<Type, Func<object, string>> TypeRegistry { get; init; } = new();
    public Func<object, string> GetObjectFormatFunction(object obj)
    {
        if (!TypeRegistry.ContainsKey(obj.GetType())) return null!;
        return TypeRegistry[obj.GetType()];
    }

    public SystemTypeRegistry SystemTypeRegistry { get; init; } = new SystemTypeRegistry();
    public Func<object, string> GetSystemTypeFormatFunction(object obj) =>
        SystemTypeRegistry.Get(obj.GetType()) ?? (x => x!.ToString()!);

    public Dictionary<Type, List<FieldInfo>> FieldsToIgnore { get; init; } = [];
    public bool ShouldNotBeIgnored(Type type, FieldInfo field)
    {
        if (!FieldsToIgnore.ContainsKey(type)) return true;
        return !FieldsToIgnore[type].Contains(field);
    }
    public Dictionary<Type, List<PropertyInfo>> PropertiesToIgnore { get; init; } = [];
    public bool ShouldNotBeIgnored(Type type, PropertyInfo prop)
    {
        if (!PropertiesToIgnore.ContainsKey(type)) return true;
        return !PropertiesToIgnore[type].Contains(prop);
    }

    public bool WithClass { get; set; } = false;

    public Dictionary<Type, Func<object, string>> SelfReferencingRegistry { get; init; } = [];
    public Func<object, string> GetReferencingFormatFunction(object obj)
    {
        if (!SelfReferencingRegistry.ContainsKey(obj.GetType())) return null!;
        return SelfReferencingRegistry[obj.GetType()];
    }

    public Dictionary<Func<Type, bool>, Func<object, object>> Formatters { get; init; } = [];
    public bool HasFormatter(object input)
    {
        return Formatters.Any(a => a.Key(input.GetType()));
    }
    public object? GetFormattedObject(object input)
    {
        var formatter = Formatters.FirstOrDefault(a => a.Key(input.GetType()));
        if (formatter.Value is not null)
            return formatter.Value(input);
        return null;
    }

    public IEnumerable<PropertyInfo> Properties(object input) =>
        input.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ShouldNotBeIgnored(input.GetType(), a));

    public IEnumerable<FieldInfo> Fields(object input) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ShouldNotBeIgnored(input.GetType(), a));

    public IEnumerable<ObjectProperty> ObjectProperties(object input) =>
        Properties(input).Select(a => new ObjectProperty(a.Name, a.GetValue(input)!)).Union(
        Fields(input).Select(a => new ObjectProperty(a.Name, a.GetValue(input)!)));

    public IEnumerable<object> FieldValues(object input) =>
        input.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(a => ShouldNotBeIgnored(input.GetType(), a))
            .Select(a => a.GetValue(input)!);
}
