using QuickPulse.Arteries;
using QuickPulse.Show.Bolts;

namespace QuickPulse.Show;

public static class Introduce
{
    public static string This(object obj, bool prettyPrint = true) =>
        Signal.From(The.Tsar(new Ministers() { PrettyPrint = prettyPrint }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj)
            .GetArtery<Holden>()
            .Whispers();

    public static T PulseToLog<T>(T item)
    {
        Signal.Tracing<string>()
            .SetArtery(WriteData.ToFile("_introducing.log"))
            .Pulse(This(item!));
        return item;
    }
}
