namespace QuickPulse.Show.Bolts;

public record IndentControl(bool PrettyPrint)
{
    public int Level { get; init; } = 0;
    public IndentControl IncreaseLevel() => this with { Level = Level + 1 };
    private bool needsIndent = false;
    public IndentControl EnableIndent() => this with { needsIndent = true };
    public IndentControl DisableIndent() => this with { needsIndent = false };
    public bool NeedsIndent() => PrettyPrint && needsIndent && !needsInlining;
    private bool needsInlining = false;
    public bool NeedsInlining => needsInlining;
    public IndentControl Inline(bool needsInlining)
        => this with { needsInlining = needsInlining };

    private bool onNewLine = false;
    public IndentControl OnNewLine(bool onNewLine)
        => this with { onNewLine = onNewLine };
    public bool IsNewLine()
        => onNewLine;
}
