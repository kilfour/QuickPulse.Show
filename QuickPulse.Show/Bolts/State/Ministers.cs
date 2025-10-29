

using System.Collections;
using System.Reflection;

namespace QuickPulse.Show.Bolts;

public record IndentControl(bool PrettyPrint)
{
    public int Level { get; init; } = 0;
    public IndentControl IncreaseLevel() => this with { Level = Level + 1 };
    public bool NeedsIndent { get; init; } = false;
    public IndentControl EnableIndent() => this with { NeedsIndent = true };
    public IndentControl DisableIndent() => this with { NeedsIndent = false };
    public bool DoINeedToIndentThis() => PrettyPrint && NeedsIndent;
}

public record Ministers
{
    public int Level { get; init; } = 0;
    public Ministers IncreaseLevel() => this with { Level = Level + 1 };
    public bool NeedsIndent { get; init; } = false;
    public Ministers EnableIndent() => this with { NeedsIndent = true };
    public Ministers DisableIndent() => this with { NeedsIndent = false };
    public bool PrettyPrint { get; init; } = false;
    public bool DoINeedToIndentThis() => PrettyPrint && NeedsIndent && !Inlined;

    public bool Inlined { get; init; } = false;
    public HashSet<Type> InlinedTypes { get; init; } = [];
    public bool NeedsInlining(object input) => InlinedTypes.Contains(input.GetType())
        || (input as IEnumerable)?.Count() == 0;

    public Ministers SetInlineFlag(object input)
        => this with { Inlined = NeedsInlining(input) };


    public PrimitivesRegistry Registry { get; init; } = new PrimitivesRegistry();
    public Func<object?, string> GetFormatFunction(object obj) =>
        Registry.Get(obj.GetType()) ?? (x => x!.ToString()!);


    public Dictionary<Type, Func<object, string>> TypeRegistry { get; init; } = new();
    public Func<object, string> GetObjectFormatFunction(object obj)
    {
        if (!TypeRegistry.ContainsKey(obj.GetType())) return null!;
        return TypeRegistry[obj.GetType()];
    }


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
}
