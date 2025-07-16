namespace QuickPulse.Show.Tests._tools;

public abstract class AbstractFlowTests
{
    private Signal<object> signal;
    private TheStringCatcher catcher;

    protected AbstractFlowTests()
    {
        catcher = new();
        signal = Signal.From(The.Flow).SetArtery(catcher);
    }

    protected string Pulse(object obj)
    {
        signal.Pulse(obj);
        return catcher.GetText();
    }
}
