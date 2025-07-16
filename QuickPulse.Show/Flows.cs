using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class Flows
{
    public readonly static Flow<object> PrimitiveFlow =
        from input in Pulse.Start<object>()
        let formatFunction =
            input == null
                ? new Func<object?, string>(_ => "null")
                : Registry.Get(input.GetType()) ?? (x => x!.ToString()!)
        from _ in Pulse.Trace(formatFunction(input))
        select input;
}