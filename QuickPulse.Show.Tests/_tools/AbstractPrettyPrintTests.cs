using QuickPulse.Arteries;
using QuickPulse.Bolts;

namespace QuickPulse.Show.Tests._tools;

public abstract class AbstractPrettyPrintTests
{
    private Signal<object> signal;
    private Holden catcher;

    protected AbstractPrettyPrintTests()
    {
        catcher = new();
        signal = Signal.From(The.Start(new FlowContext() { PrettyPrint = true })).SetArtery(catcher);
    }

    protected string Introduce(object obj)
    {
        signal.Pulse(obj);
        return catcher.Whispers();
    }

    protected static void AsCodeToFile(object obj)
    {
        var stringsToCodeflow =
            from input in Pulse.Start<string>()
            from _ in Pulse.Trace($"        Assert.Equal(\"{input}\", reader.NextLine());")
            select input;
        var introduction =
            Signal.From(The.Start(new FlowContext() { PrettyPrint = true }))
                    .SetArtery(TheString.Catcher())
                    .Pulse(obj)
                    .GetArtery<Holden>()
                    .Whispers();
        Signal.From(stringsToCodeflow).SetArtery(WriteData.ToFile())
            .Pulse(introduction.Split(Environment.NewLine));
    }

    protected string NewLine = Environment.NewLine;
}

public class Pipeline<T> : IArtery
{
    private readonly Signal<T> signal;

    public Pipeline(Signal<T> signal)
    {
        this.signal = signal;
    }
    public void Flow(params object[] data)
    {
        signal.Pulse(data.Cast<T>());
    }
}
