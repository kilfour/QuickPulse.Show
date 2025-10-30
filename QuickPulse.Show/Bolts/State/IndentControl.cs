namespace QuickPulse.Show.Bolts;

public record IndentControl(bool PrettyPrint)
{
    public int Level { get; init; } = 0;
    public IndentControl IncreaseLevel() => this with { Level = Level + 1 };
    private bool needsIndent = false;
    public IndentControl EnableIndent() => this with { needsIndent = true };
    public IndentControl DisableIndent() => this with { needsIndent = false };
    public bool NeedsIndent() => PrettyPrint && needsIndent;
    public IndentControl Inline(bool needsInlining)
        => this with { PrettyPrint = PrettyPrint && !needsInlining };
}
