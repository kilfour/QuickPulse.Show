using QuickPulse.Arteries;

namespace QuickPulse.Show.Tests._tools;

public abstract class AbstractFlowTests
{
    private Signal<object> signal;
    private Holden catcher;

    protected AbstractFlowTests()
    {
        catcher = TheString.Catcher();
        signal = Signal.From(The.Start(new FlowContext())).SetArtery(catcher);
    }

    protected string Pulse(object obj)
    {
        signal.Pulse(obj);
        return catcher.Whispers();
    }
}
