using QuickPulse.Arteries;

namespace QuickPulse.Show;

public static class Introduce
{
    public static string This(object obj, bool prettyPrint = true) =>
        Signal.From(The.Start(new FlowContext() { PrettyPrint = prettyPrint }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj)
            .GetArtery<Holden>()
            .Whispers();
}
