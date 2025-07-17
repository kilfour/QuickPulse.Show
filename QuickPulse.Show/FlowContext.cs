

namespace QuickPulse.Show;

public record Trap
{
    private bool primed;
    public Trap(bool primed) { this.primed = primed; }
    //public void Prime() { primed = true; }
    public bool Spring() { var result = primed; primed = false; return result; }
}

public record FlowContext
{
    public bool PrettyPrint { get; init; } = false;
    public Trap StartOfCollection { get; init; } = new Trap(false);
    public bool NeedsIndent { get; init; } = false;
    public int Level { get; init; } = 0;
    private readonly HashSet<object> visited = new(ReferenceEqualityComparer.Instance);
    public bool AlreadyVisited(object obj)
    {
        if (visited.Contains(obj)) return true;
        visited.Add(obj);
        return false;
    }

    public bool DoINeedToIndentThis()
    {
        return PrettyPrint && NeedsIndent;
    }

    public FlowContext IncreaseLevel()
    {
        return this with { Level = Level + 1 };
    }

    public FlowContext EnableIndent()
    {
        return this with { NeedsIndent = true };
    }

    public FlowContext DisableIndent()
    {
        return this with { NeedsIndent = false };
    }

    public FlowContext PrimeStartOfCollection()
    {
        return this with { StartOfCollection = new Trap(true) };
    }
}


