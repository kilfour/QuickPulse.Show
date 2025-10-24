

using System.Reflection;
using System.Runtime.CompilerServices;

namespace QuickPulse.Show.Bolts;

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
    public bool DoINeedToIndentThis() => PrettyPrint && NeedsIndent && !Inlined;

    public bool Inlined { get; init; } = false;
    public HashSet<Type> InlinedTypes { get; init; } = [];
    public bool NeedsInlining(object input) => InlinedTypes.Contains(input.GetType());

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

    // ---------- NEW: recursion path (identity) ----------
    public HashSet<object> Path { get; init; } = new(ReferenceEqualityComparer.Instance);

    private static bool IsLeaf(object? x)
        => x is null || x is string || x.GetType().IsValueType;

    /// Is the node already on the current recursion path?
    public bool IsOnPath(object? x) => !IsLeaf(x) && Path.Contains(x!);

    /// Push: returns a new Ministers with x added to Path (no-op for leaves).
    public Ministers Enter(object? x)
    {
        if (IsLeaf(x)) return this;
        var next = new HashSet<object>(Path, ReferenceEqualityComparer.Instance) { x! };
        return this with { Path = next };
    }

    /// Pop: returns a new Ministers with x removed from Path (no-op for leaves).
    public Ministers Exit(object? x)
    {
        if (IsLeaf(x)) return this;
        if (!Path.Contains(x!)) return this;
        var next = new HashSet<object>(Path, ReferenceEqualityComparer.Instance);
        next.Remove(x!);
        return this with { Path = next };
    }

    public bool WithClass { get; set; } = false;
    public Dictionary<Type, Func<object, string>> SelfReferencingRegistry { get; init; } = [];
    public Func<object, string> GetReferencingFormatFunction(object obj)
    {
        if (!SelfReferencingRegistry.ContainsKey(obj.GetType())) return null!;
        return SelfReferencingRegistry[obj.GetType()];
    }
}


public sealed class CycleGuard
{
    private readonly HashSet<object> _stack = new(ReferenceEqualityComparer.Instance);

    public bool TryEnter(object? node)
    {
        if (node is null || node is string) return true;
        var t = node.GetType();
        if (t.IsValueType) return true;
        return _stack.Add(node);
    }

    public void Exit(object? node)
    {
        if (node is null || node is string) return;
        var t = node.GetType();
        if (t.IsValueType) return;
        _stack.Remove(node);
    }
}
