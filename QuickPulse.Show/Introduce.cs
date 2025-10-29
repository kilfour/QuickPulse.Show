using Microsoft.VisualBasic;
using QuickPulse.Arteries;
using QuickPulse.Show.Bolts;

namespace QuickPulse.Show;


public static class Introduce
{
    public static string This(object obj, bool prettyPrint = true) =>
        Signal.From(The.Tsar(new Ministers() { PrettyPrint = prettyPrint }, prettyPrint))
            .SetArtery(Text.Capture())
            .Pulse(obj)
            .GetArtery<StringSink>()
            .Content();

    public static T PulseToLog<T>(this T item, string filename = null!)
    {
        Signal.From<string>(a => Pulse.Trace(a))
            .SetArtery(FileLog.Append(filename))
            .Pulse(This(item!));
        return item;
    }
}
