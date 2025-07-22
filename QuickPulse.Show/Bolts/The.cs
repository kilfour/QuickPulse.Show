using System.Collections;
using QuickPulse.Bolts;

namespace QuickPulse.Show.Bolts;

public static class The
{
    private const string CycleMarker = "<cycle>";

    private readonly static Flow<Unit> Spacing =
        from ministers in Pulse.Gather<Ministers>()
        from _ in ministers.Value.PrettyPrint ? Pulse.Trace(Environment.NewLine) : Pulse.Trace(" ")
        select Unit.Instance;

    private readonly static Flow<Unit> Indent =
        from ministers in Pulse.Gather<Ministers>()
        let indentString = new string(' ', ministers.Value.Level * 4)
        from trace in Pulse.TraceIf<Ministers>(a => a.DoINeedToIndentThis(), () => indentString)
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

    private static Flow<Unit> Braced(Flow<Unit> innerFlow) => Enclosed("{", "}", innerFlow);
    private static Flow<Unit> Bracketed(Flow<Unit> innerFlow) => Enclosed("(", ")", innerFlow);
    private static Flow<Unit> SquareBracketed(Flow<Unit> innerFlow) => Enclosed("[", "]", innerFlow);

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        from ministers in Pulse.Gather<Ministers>()
        from indent in Pulse.When<Ministers>(a => a.NeedsIndent, Indent)
        let formatFunction = ministers.Value.GetFormatFunction(input)
        from _ in Pulse.Trace(formatFunction(input))
        select input;

    private readonly static Flow<object> InterspersedPrimed =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Gather<Ministers>()
        from seperator in Pulse.When<Ministers>(a => a.StartOfCollection.Restricted(), Separator)
        let element = Pulse.ToFlow(Anastasia!, input)
        from _ in Spacing.Then(element)
        select input;

    private readonly static Flow<IEnumerable> Interspersed =
        from input in Pulse.Start<IEnumerable>()
        from ministers in Pulse.Gather<Ministers>()
        let inner = Pulse.ToFlow(InterspersedPrimed, input.Cast<object>())
        from _ in Pulse.Scoped<Ministers>(a => a.PrimeStartOfCollection(), inner)
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from _ in SquareBracketed(Pulse.ToFlow(Interspersed, input))
        select input;

    private readonly static Flow<(object, object)> LabeledValue =
        from input in Pulse.Start<(object Label, object Value)>()
        from label in Pulse.ToFlow(Anastasia!, input.Label)
        from colon in Colon
        let value = Pulse.ToFlow(Anastasia!, input.Value)
        from _ in Pulse.Scoped<Ministers>(a => a.DisableIndent(), value)
        select input;

    private readonly static Flow<object> KeyValuePair =
        from input in Pulse.Start<object>()
        let keyValue = Pulse.ToFlow(LabeledValue, Get.KeyValueAsTuple(input))
        from _ in Pulse.Scoped<Ministers>(a => a.EnableIndent(), keyValue)
        select input;

    private readonly static Flow<IDictionary> Dictionary =
        from input in Pulse.Start<IDictionary>()
        from _ in Braced(Pulse.ToFlow(Interspersed, input))
        select input;

    private readonly static Flow<ObjectProperty> Property =
        from input in Pulse.Start<ObjectProperty>()
        from key in Indent.Then(Pulse.Trace(input.Name))
        from colon in Colon
        let value = Pulse.ToFlow(Anastasia!, input.Value)
        from _ in Pulse.Scoped<Ministers>(a => a.DisableIndent(), value)
        select input;

    private readonly static Flow<object> Tuple =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Gather<Ministers>()
        let items = Get.FieldValues(input, ministers.Value)
        from _ in Bracketed(Pulse.ToFlow(Interspersed, items))
        select input;

    private readonly static Flow<object> DefaultObject =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Gather<Ministers>()
        let items = Get.ObjectProperties(input, ministers.Value)
        from _ in Braced(Pulse.ToFlow(Interspersed, items))
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Gather<Ministers>()
        let formatter = ministers.Value.GetObjectFormatFunction(input)
        from crash in Pulse.Trace(formatter == null ? "" : formatter(input))
        from __ in Pulse.ToFlowIf(formatter == null, DefaultObject, () => input)
        select input;

    private readonly static Flow<object> Anastasia =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Gather(new Ministers())
        let registry = ministers.Value.Registry
        from _ in Pulse.FirstOf(
            (() => input == null,                         /**/ () => Null),
            (() => Is.Primitive(input, registry),         /**/ () => Pulse.ToFlow(Primitive, input)),
            (() => ministers.Value.AlreadyVisited(input), /**/ () => Pulse.Trace(CycleMarker)),
            (() => Is.ObjectProperty(input),              /**/ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => Is.Dictionary(input),                  /**/ () => Pulse.ToFlow(Dictionary, (IDictionary)input)),
            (() => Is.KeyValuePair(input),                /**/ () => Pulse.ToFlow(KeyValuePair, input)),
            (() => Is.Collection(input),                  /**/ () => Pulse.ToFlow(Collection, (IEnumerable)input)),
            (() => Is.Tuple(input),                       /**/ () => Pulse.ToFlow(Tuple, input)),
            (() => Is.Object(input),                      /**/ () => Pulse.ToFlow(Object, input)))
        select input;

    public static Flow<object> Tsar(Ministers ministers) =>
        from input in Pulse.Start<object>()
        from _ in Pulse.Gather(ministers)
        from __ in Pulse.ToFlow(Anastasia, input)
        select input;
}
