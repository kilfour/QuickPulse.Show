

using System.Reflection;

namespace QuickPulse.Show;

public record Ministers
{
    public PrimitivesRegistry Registry { get; init; } = new PrimitivesRegistry();
    public Func<object?, string> GetFormatFunction(object obj) =>
        Registry.Get(obj.GetType()) ?? (x => x!.ToString()!);

    public Dictionary<Type, Func<object, string>> TypeRegistry { get; init; } = new();
    public Func<object, string> GetObjectFormatFunction(object obj)
    {
        if (!TypeRegistry.ContainsKey(obj.GetType())) return null!;
        return TypeRegistry[obj.GetType()];
    }

    public bool NeedsIndent { get; init; } = false;
    public Ministers EnableIndent() => this with { NeedsIndent = true };
    public Ministers DisableIndent() => this with { NeedsIndent = false };
    public bool PrettyPrint { get; init; } = false;
    public bool DoINeedToIndentThis() => PrettyPrint && NeedsIndent;


    public int Level { get; init; } = 0;
    public Ministers IncreaseLevel() => this with { Level = Level + 1 };


    public Valve StartOfCollection { get; init; } = Valve.Closed();
    public Ministers PrimeStartOfCollection() => this with { StartOfCollection = Valve.Install() };


    public Dictionary<Type, List<FieldInfo>> FieldsToIgnore { get; init; } = [];
    public Dictionary<Type, List<PropertyInfo>> PropertiesToIgnore { get; init; } = [];
    public bool ShouldNotBeIgnored(Type type, PropertyInfo prop)
    {
        if (!PropertiesToIgnore.ContainsKey(type)) return true;
        return !PropertiesToIgnore[type].Contains(prop);
    }
    public bool ShouldNotBeIgnored(Type type, FieldInfo field)
    {
        if (!FieldsToIgnore.ContainsKey(type)) return true;
        return !FieldsToIgnore[type].Contains(field);
    }

    private readonly HashSet<object> visited = new(ReferenceEqualityComparer.Instance);
    public bool AlreadyVisited(object obj)
    {
        if (obj == null || obj.GetType().IsValueType) return false; // just to be on the safe side
        if (visited.Contains(obj)) return true;
        visited.Add(obj);
        return false;
    }
}


