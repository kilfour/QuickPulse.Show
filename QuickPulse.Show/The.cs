using System.Collections;
using System.Reflection;
using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class The
{
    private readonly static Flow<Unit> LeftBrace = Pulse.Trace("{ ");
    private readonly static Flow<Unit> RightBrace = Pulse.Trace(" }");
    private readonly static Flow<Unit> LeftBracket = Pulse.Trace("[ ");
    private readonly static Flow<Unit> RightBracket = Pulse.Trace(" ]");
    private readonly static Flow<Unit> Colon = Pulse.Trace(": ");
    private readonly static string Seperator = ", ";
    private readonly static string Null = "null";

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        let formatFunction = Registry.Get(input.GetType()) ?? (x => x!.ToString()!)
        from _ in Pulse.Trace(formatFunction(input))
        select input;

    private readonly static Flow<object> Interspersed =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        let start = context.Value.StartOfCollection
        from pulseSeperator in Pulse.TraceIf(!context.Value.StartOfCollection, Seperator)
        from inner in Pulse.ToFlow(Flow!, input)
        from setStart in Pulse.Effect(() => context.Value.StartOfCollection = false)
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from context in Pulse.Gather<FlowContext>()
        from setStartTrue in Pulse.Effect(() => context.Value.StartOfCollection = true)
        from leftBracket in LeftBracket
        from inner in Pulse.ToFlow(Interspersed, input.Cast<object>())
        from righttBracket in RightBracket
        select input;

    private readonly static Flow<IDictionary> Dictionary =
        from input in Pulse.Start<IDictionary>()
        from context in Pulse.Gather<FlowContext>()
        from setStartTrue in Pulse.Effect(() => context.Value.StartOfCollection = true)
        from leftBrace in LeftBrace
        from inner in Pulse.ToFlow(Interspersed, input.Cast<object>())
        from righttBrace in RightBrace
        select input;

    private readonly static Flow<(object, object)> LabelledValue =
        from input in Pulse.Start<(object Label, object Value)>()
        from key in Pulse.ToFlow(Flow!, input.Label)
        from colon in Colon
        from value in Pulse.ToFlow(Flow!, input.Value)
        select input;

    private readonly static Flow<object> KeyValuePair =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        let key = input.GetType().GetProperty("Key")?.GetValue(input)
        let value = input.GetType().GetProperty("Value")?.GetValue(input)
        from labeledValue in Pulse.ToFlow(LabelledValue, (key, value))
        select input;

    private readonly static Flow<ObjectProperty> Property =
        from input in Pulse.Start<ObjectProperty>()
        from key in Pulse.Trace(input.Name)
        from colon in Colon
        from value in Pulse.ToFlow(Flow!, input.Value)
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        from setStartTrue in Pulse.Effect(() => context.Value.StartOfCollection = true)
        let props = input.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
        let labeledValues = props.Select(a => new ObjectProperty(a.Name, a.GetValue(input)!))
        from leftBrace in LeftBrace
        from inner in Pulse.ToFlow(Interspersed, labeledValues)
        from righttBrace in RightBrace
        select input;

    public readonly static Flow<object> Flow =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather(new FlowContext())
        from _ in Pulse.FirstOf(
            (() => input == null, /*                      */ () => Pulse.Trace(Null)),
            (() => Is.Primitive(input), /*                */ () => Pulse.ToFlow(Primitive, input)),
            (() => context.Value.AlreadyVisited(input), /**/ () => Pulse.Trace("<cycle>")),
            (() => Is.ObjectProperty(input), /*           */ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => Is.Dictionary(input), /*               */ () => Pulse.ToFlow(Dictionary, (IDictionary)input)),
            (() => Is.KeyValuePair(input), /*             */ () => Pulse.ToFlow(KeyValuePair, input)),
            (() => Is.Collection(input), /*               */ () => Pulse.ToFlow(Collection, (IEnumerable)input)),
            (() => Is.Object(input), /*                   */ () => Pulse.ToFlow(Object, input)))
        select input;
}
