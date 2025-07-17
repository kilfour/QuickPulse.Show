namespace QuickPulse.Show.Tests._tools;

public abstract class AbstractPrettyPrintTests
{
    private Signal<object> signal;
    private TheStringCatcher catcher;

    protected AbstractPrettyPrintTests()
    {
        catcher = new();
        signal = Signal.From(The.Start(new FlowContext() { PrettyPrint = true })).SetArtery(catcher);
    }

    protected string Pulse(object obj)
    {
        signal.Pulse(obj);
        return catcher.GetText();
    }

    protected string NewLine = Environment.NewLine;
}
