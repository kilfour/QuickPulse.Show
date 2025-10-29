namespace QuickPulse.Show.Bolts;

public record CycleGuard
{
    public HashSet<object> Path { get; init; } = new(ReferenceEqualityComparer.Instance);
    private static bool IsLeaf(object? x)
        => x is null || x is string || x.GetType().IsValueType;
    public bool IsOnPath(object? x) => !IsLeaf(x) && Path.Contains(x!);
    public CycleGuard Enter(object? x)
    {
        if (IsLeaf(x)) return this;
        var next = new HashSet<object>(Path, ReferenceEqualityComparer.Instance) { x! };
        return this with { Path = next };
    }
    public CycleGuard Exit(object? x)
    {
        if (IsLeaf(x)) return this;
        if (!Path.Contains(x!)) return this;
        var next = new HashSet<object>(Path, ReferenceEqualityComparer.Instance);
        next.Remove(x!);
        return this with { Path = next };
    }
}