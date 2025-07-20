using System.Collections;
using QuickPulse.Bolts;

namespace QuickPulse.Show;

public static class The
{
    private readonly static Flow<Unit> Spacing =
        from context in Pulse.Gather<Ministers>()
        from _ in context.Value.PrettyPrint ? Pulse.Trace(Environment.NewLine) : Pulse.Trace(" ")
        select Unit.Instance;

    private readonly static Flow<Unit> Indent =
        from context in Pulse.Gather<Ministers>()
        from trace in Pulse.TraceIf(context.Value.DoINeedToIndentThis(), new string(' ', context.Value.Level * 4))
        select Unit.Instance;

    private static Flow<Unit> Indented(string str) => Indent.Then(Pulse.Trace(str));

    private readonly static Flow<Unit> Separator = Pulse.Trace(",");
    private readonly static Flow<Unit> Colon = Pulse.Trace(": ");
    private readonly static Flow<Unit> Null = Indented("null");

    private static Flow<Unit> Enclosed(string left, string right, Flow<Unit> innerFlow) =>
        from leftBracket in Indented(left)
        from _ in Pulse.Scoped<Ministers>(a => a.IncreaseLevel().EnableIndent(), innerFlow)
        from spacing in Spacing
        from __ in Pulse.Scoped<Ministers>(a => a.EnableIndent(), Indented(right))
        select Unit.Instance;

    private static Flow<Unit> Braced(Flow<Unit> innerFlow) =>
       Enclosed("{", "}", innerFlow);

    private static Flow<Unit> Bracketed(Flow<Unit> innerFlow) =>
       Enclosed("(", ")", innerFlow);
    private static Flow<Unit> SquareBracketed(Flow<Unit> innerFlow) =>
       Enclosed("[", "]", innerFlow);

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        from context in Pulse.Gather<Ministers>()
        from indent in Pulse.When(context.Value.NeedsIndent, Indent)
        let formatFunction = Registry.Get(input.GetType()) ?? (x => x!.ToString()!)
        from _ in Pulse.Trace(formatFunction(input))
        select input;


    private readonly static Flow<object> InterspersedPrimed =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<Ministers>()
        from seperator in Pulse.When(context.Value.StartOfCollection.Restricted(), Separator)
        from spacing in Spacing
        from inner in Pulse.ToFlow(Anastasia!, input)
        select input;

    private readonly static Flow<IEnumerable> Interspersed =
        from input in Pulse.Start<IEnumerable>()
        from context in Pulse.Gather<Ministers>()
        from _ in Pulse.Scoped<Ministers>(a => a.PrimeStartOfCollection(), Pulse.ToFlow(InterspersedPrimed, input.Cast<object>()))
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from _ in SquareBracketed(Pulse.ToFlow(Interspersed, input))
        select input;

    private readonly static Flow<(object, object)> LabeledValue =
        from input in Pulse.Start<(object Label, object Value)>()
        from _ in Pulse.ToFlow(Anastasia!, input.Label)
        from colon in Colon
        from __ in Pulse.Scoped<Ministers>(
            a => a.DisableIndent(), Pulse.ToFlow(Anastasia!, input.Value))
        select input;

    private readonly static Flow<object> KeyValuePair =
        from input in Pulse.Start<object>()
        from _ in Pulse.Scoped<Ministers>(
            a => a.EnableIndent(), Pulse.ToFlow(LabeledValue, Get.KeyValueAsTuple(input)))
        select input;

    private readonly static Flow<IDictionary> Dictionary =
        from input in Pulse.Start<IDictionary>()
        from _ in Braced(Pulse.ToFlow(Interspersed, input))
        select input;

    private readonly static Flow<ObjectProperty> Property =
        from input in Pulse.Start<ObjectProperty>()
        from indent in Indent
        from key in Pulse.Trace(input.Name)
        from colon in Colon
        from _ in Pulse.Scoped<Ministers>(
            a => a.DisableIndent(), Pulse.ToFlow(Anastasia!, input.Value))
        select input;

    private readonly static Flow<object> Tuple =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<Ministers>()
        from _ in Bracketed(Pulse.ToFlow(Interspersed, Get.FieldValues(input, context)))
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather<Ministers>()
        from _ in Braced(Pulse.ToFlow(Interspersed, Get.ObjectProperties(input, context)))
        select input;

    private readonly static Flow<object> Anastasia =
        from input in Pulse.Start<object>()
        from context in Pulse.Gather(new Ministers())
        from _ in Pulse.FirstOf(
            (() => input == null, /*                      */ () => Null),
            (() => Is.Primitive(input), /*                */ () => Pulse.ToFlow(Primitive, input)),
            (() => context.Value.AlreadyVisited(input), /**/ () => Pulse.Trace("<cycle>")),
            (() => Is.ObjectProperty(input), /*           */ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => Is.Dictionary(input), /*               */ () => Pulse.ToFlow(Dictionary, (IDictionary)input)),
            (() => Is.KeyValuePair(input), /*             */ () => Pulse.ToFlow(KeyValuePair, input)),
            (() => Is.Collection(input), /*               */ () => Pulse.ToFlow(Collection, (IEnumerable)input)),
            (() => Is.Tuple(input), /*                    */ () => Pulse.ToFlow(Tuple, input)),
            (() => Is.Object(input), /*                   */ () => Pulse.ToFlow(Object, input)))
        select input;

    public static Flow<object> Tsar(Ministers context) =>
        from input in Pulse.Start<object>()
        from _ in Pulse.Gather(context)
        from __ in Pulse.ToFlow(Anastasia, input)
        select input;
}
