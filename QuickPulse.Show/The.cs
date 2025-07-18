using System.Collections;
using System.Reflection;
using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class The
{
    private readonly static Flow<Unit> Spacing =
        from context in Pulse.Gather<FlowContext>()
        from _ in context.Value.PrettyPrint ? Pulse.Trace(Environment.NewLine) : Pulse.Trace(" ")
        select Unit.Instance;

    private readonly static Flow<Unit> Indent =
        from context in Pulse.Gather<FlowContext>()
        from trace in Pulse.TraceIf(context.Value.DoINeedToIndentThis(), new string(' ', context.Value.Level * 4))
        select Unit.Instance;

    private static Flow<Unit> Indented(string str) => Indent.Then(Pulse.Trace(str));

    private readonly static Flow<Unit> LeftBrace = Indented("{");
    private readonly static Flow<Unit> RightBrace = Indented("}");
    private readonly static Flow<Unit> LeftBracket = Indented("(");
    private readonly static Flow<Unit> RightBracket = Indented(")");
    private readonly static Flow<Unit> LeftSquareBracket = Indented("[");
    private readonly static Flow<Unit> RightSquareBracket = Indented("]");
    private readonly static Flow<Unit> Seperator = Pulse.Trace(",");
    private readonly static Flow<Unit> Colon = Pulse.Trace(": ");
    private readonly static Flow<Unit> Null = Indented("null");

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        from context in Pulse.Gather<FlowContext>()
        from indent in Pulse.When(context.Value.NeedsIndent, Indent)
        let formatFunction = Registry.Get(input.GetType()) ?? (x => x!.ToString()!)
        from _ in Pulse.Trace(formatFunction(input))
        select input;

    private readonly static Flow<object> Interspersed =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        from seperator in Pulse.When(!context.Value.StartOfCollection.Spring(), Seperator)
        from spacing in Spacing
        from inner in Pulse.ToFlow(Flow!, input)
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from context in Pulse.Gather<FlowContext>()
        from leftBracket in LeftSquareBracket
        from _ in Pulse.Scoped<FlowContext>(
            a => a.IncreaseLevel().EnableIndent().PrimeStartOfCollection(),
            Pulse.ToFlow(Interspersed, input.Cast<object>()))
        from spacing in Spacing
        from __ in Pulse.Scoped<FlowContext>(
            a => a.EnableIndent(), RightSquareBracket)
        select input;

    private readonly static Flow<(object, object)> LabelledValue =
        from input in Pulse.Start<(object Label, object Value)>()
        from context in Pulse.Gather<FlowContext>()
        from key in Pulse.ToFlow(Flow!, input.Label)
        from colon in Colon
        from _ in Pulse.Scoped<FlowContext>(
           a => a.DisableIndent(),
           Pulse.ToFlow(Flow!, input.Value))
        select input;

    private readonly static Flow<object> KeyValuePair =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        let key = input.GetType().GetProperty("Key")?.GetValue(input)
        let value = input.GetType().GetProperty("Value")?.GetValue(input)
        from _ in Pulse.Scoped<FlowContext>(
            a => a.IncreaseLevel().EnableIndent(),
            Pulse.ToFlow(LabelledValue, (key, value)))
        select input;

    private readonly static Flow<IDictionary> Dictionary =
        from input in Pulse.Start<IDictionary>()
        from context in Pulse.Gather<FlowContext>()
        from leftBrace in LeftBrace
        from prime in Pulse.Scoped<FlowContext>(
            a => a.PrimeStartOfCollection(),
            Pulse.ToFlow(Interspersed, input.Cast<object>()))
        from spacing in Spacing
        from _ in Pulse.Scoped<FlowContext>(
            a => a.EnableIndent(), RightBrace)
        select input;

    private readonly static Flow<ObjectProperty> Property =
        from input in Pulse.Start<ObjectProperty>()
        from context in Pulse.Gather<FlowContext>()
        from indent in Indent
        from key in Pulse.Trace(input.Name)
        from colon in Colon
        from _ in Pulse.Scoped<FlowContext>(
            a => a.DisableIndent(),
            Pulse.ToFlow(Flow!, input.Value))
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<FlowContext>()
        let props = input.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
        let labeledProps = props.Select(a => new ObjectProperty(a.Name, a.GetValue(input)!))
        let fields = input.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
        let labeledFields = fields.Select(a => new ObjectProperty(a.Name, a.GetValue(input)!))
        let labeledValues = labeledProps.Union(labeledFields)
        let isTuple = Is.Tuple(input)
        let left = isTuple ? LeftBracket : LeftBrace
        let right = isTuple ? RightBracket : RightBrace
        from _ in left
        from __ in Pulse.Scoped<FlowContext>(
            a => a.IncreaseLevel().EnableIndent().PrimeStartOfCollection(),
            Pulse.ToFlow(Interspersed, labeledValues))
        from spacing in Spacing
        from ___ in Pulse.Scoped<FlowContext>(
            a => a.EnableIndent(), right)
        select input;

    private readonly static Flow<object> Flow =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather(new FlowContext())
        from _ in Pulse.FirstOf(
            (() => input == null, /*                      */ () => Null),
            (() => Is.Primitive(input), /*                */ () => Pulse.ToFlow(Primitive, input)),
            (() => context.Value.AlreadyVisited(input), /**/ () => Pulse.Trace("<cycle>")),
            (() => Is.ObjectProperty(input), /*           */ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => Is.Dictionary(input), /*               */ () => Pulse.ToFlow(Dictionary, (IDictionary)input)),
            (() => Is.KeyValuePair(input), /*             */ () => Pulse.ToFlow(KeyValuePair, input)),
            (() => Is.Collection(input), /*               */ () => Pulse.ToFlow(Collection, (IEnumerable)input)),
            (() => Is.Tuple(input), /*                    */ () => Pulse.ToFlow(Object, input)),
            (() => Is.Object(input), /*                   */ () => Pulse.ToFlow(Object, input)))
        select input;

    public static Flow<object> Start(FlowContext context) =>
        from input in Pulse.Start<object>()
        from _ in Pulse.Gather(context)
        from __ in Pulse.ToFlow(Flow, input)
        select input;
}
