namespace QuickPulse.Show;

public class FlowContext
{
    public bool StartOfCollection { get; set; } = true;
    private readonly HashSet<object> visited = new(ReferenceEqualityComparer.Instance);
    public bool AlreadyVisited(object obj)
    {
        if (visited.Contains(obj)) return true;
        visited.Add(obj);
        return false;
    }
}
