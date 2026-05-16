using System.Runtime.CompilerServices;
using QuickPulse.Arteries;
using QuickPulse.Show.Bolts;
using QuickPulse.Show.Bolts.State;

namespace QuickPulse.Show;


public static class Introduce
{
    public static string This(object obj, bool prettyPrint = true) =>
        Signal.From<object>(a => The.Tsar(new Ministers(), prettyPrint, a))
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

    public static T PulseToQuickLog<T>(
        this T item,
        [CallerMemberName] string testName = "",
        [CallerFilePath] string callerPath = "")
    {
        var dir = Path.GetDirectoryName(callerPath)!;
        var fullPath = Path.Combine(dir, $"{testName}.log");
        Signal.From<string>(a => Pulse.Trace(a))
           .SetArtery(FileLog.Append(fullPath))
           .Pulse(This(item!));
        return item;
    }
}
