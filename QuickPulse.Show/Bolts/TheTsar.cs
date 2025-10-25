using System.Collections;

namespace QuickPulse.Show.Bolts;

public static class The
{
    private const string CycleMarker = "<cycle>";

    private readonly static Flow<object> Cycle =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Draw<Ministers>()
        let formatFunction = ministers.GetReferencingFormatFunction(input)
        from _ in formatFunction is null ? Indented(CycleMarker) : Indented(formatFunction(input))
        select input;

    private readonly static Flow<Flow> Spacing =
        from ministers in Pulse.Draw<Ministers>()
        from _ in ministers.PrettyPrint && !ministers.Inlined ? Pulse.Trace(Environment.NewLine) : Pulse.Trace(" ")
        select Flow.Continue;

    private readonly static Flow<Flow> Indent =
        Pulse.TraceIf<Ministers>(
            a => a.DoINeedToIndentThis(),
            a => new string(' ', a.Level * 4));

    private static Flow<Flow> Indented(string str) => Indent.Then(Pulse.Trace(str));

    private readonly static Flow<Flow> Separator = Pulse.Trace(",");
    private readonly static Flow<Flow> Colon = Pulse.Trace(": ");
    private readonly static Flow<Flow> Null = Indented("null");

    private static Flow<Flow> Enclosed(string left, string right, Flow<Flow> innerFlow) =>
        from leftBracket in Pulse.Trace(left)
        from _ in Pulse.Scoped<Ministers>(a => a.IncreaseLevel().EnableIndent(), innerFlow)
        from spacing in Spacing
        from __ in Pulse.Scoped<Ministers>(a => a.EnableIndent(), Indented(right))
        select Flow.Continue;

    private static Flow<Flow> Braced(Flow<Flow> innerFlow) => Enclosed("{", "}", innerFlow);
    private static Flow<Flow> Bracketed(Flow<Flow> innerFlow) => Enclosed("(", ")", innerFlow);
    private static Flow<Flow> SquareBracketed(Flow<Flow> innerFlow) => Enclosed("[", "]", innerFlow);

    private readonly static Flow<object> Fallback =
        from input in Pulse.Start<object?>()
        from indent in Pulse.When<Ministers>(a => a.NeedsIndent, Indent)
        from _ in Pulse.Trace(input)
        select input;

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        from ministers in Pulse.Draw<Ministers>()
        from indent in Pulse.When<Ministers>(a => a.NeedsIndent, Indent)
        let formatFunction = ministers.GetFormatFunction(input)
        from _ in Pulse.Trace(formatFunction(input))
        select input;

    private readonly static Flow<object> InterspersedPrimed =
        from input in Pulse.Start<object>()
        from seperator in Pulse.When<Ministers>(a => a.StartOfCollection.Restricted(), Separator)
        let element = Pulse.ToFlow(Anastasia!, input)
        from _ in Spacing.Then(element)
        select input;

    private readonly static Flow<IEnumerable> Interspersed =
        from input in Pulse.Start<IEnumerable>()
        let inner = Pulse.ToFlow(InterspersedPrimed, input.Cast<object>())
        from _ in Pulse.Scoped<Ministers>(a => a.PrimeStartOfCollection(), inner)
        select input;

    private readonly static Flow<IEnumerable> BracketedInterspersed =
        from input in Pulse.Start<IEnumerable>()
        from _ in Indent.Then(SquareBracketed(Pulse.ToFlow(Interspersed, input)))
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from _ in Pulse.Scoped<Ministers>(a => a.SetInlineFlag(input), BracketedInterspersed.Dissipate())
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
        from _ in Indent.Then(Braced(Pulse.ToFlow(Interspersed, input)))
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
        from ministers in Pulse.Draw<Ministers>()
        let items = Get.FieldValues(input, ministers)
        from _ in Indent.Then(Bracketed(Pulse.ToFlow(Interspersed, items)))
        select input;

    private readonly static Flow<object> DefaultObject =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Draw<Ministers>()
        let items = Get.ObjectProperties(input, ministers)
        from _1 in Pulse.TraceIf(ministers.WithClass, () => $"{input.GetType().Name} ")
        from _2 in Indent.Then(Braced(Pulse.ToFlow(Interspersed, items)))
        select input;

    private readonly static Flow<object> ObjectFinal =
        from input in Pulse.Start<object>()
        from formatter in Pulse.Draw<Ministers, Func<object, string>>(a => a.GetObjectFormatFunction(input))
        from _ in Pulse.TraceIf(formatter != null, () => formatter(input))
        from __ in Pulse.ToFlowIf(formatter == null, DefaultObject, () => input)
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        let flow = Pulse.ToFlow(ObjectFinal, input)
        from _ in needsInlining
            ? Indent.Then(Pulse.Scoped<Ministers>(a => a.SetInlineFlag(input), flow))
            : flow
        select input;

    private static Flow<Flow> Guarded(object node, Flow<Flow> inner) =>
        from _ in Pulse.Scoped<Ministers>(m => m.Enter(node), inner)
        from __ in Pulse.Scoped<Ministers>(m => m.Exit(node), Pulse.NoOp())
        select Flow.Continue;

    private readonly static Flow<object> Anastasia =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Draw<Ministers>()
        let registry = ministers.Registry
        from _ in Pulse.FirstOf(
            (() => input == null,                 /**/ () => Null),
            (() => ministers.HasFormatter(input), /**/ () => Pulse.ToFlow(Anastasia!, ministers.GetFormattedObject(input)!)),
            (() => Is.Primitive(input, registry), /**/ () => Pulse.ToFlow(Primitive, input)),
            (() => Is.ObjectProperty(input),      /**/ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => ministers.IsOnPath(input),     /**/ () => Pulse.ToFlow(Cycle, input)),
            (() => Is.Dictionary(input),          /**/ () => Guarded(input, Pulse.ToFlow(Dictionary, (IDictionary)input))),
            (() => Is.KeyValuePair(input),        /**/ () => Guarded(input, Pulse.ToFlow(KeyValuePair, input))),
            (() => Is.Collection(input),          /**/ () => Guarded(input, Pulse.ToFlow(Collection, (IEnumerable)input))),
            (() => Is.Tuple(input),               /**/ () => Guarded(input, Pulse.ToFlow(Tuple, input))),
            (() => Is.Object(input),              /**/ () => Guarded(input, Pulse.ToFlow(Object, input))),
            (() => true,                          /**/ () => Pulse.ToFlow(Fallback, input)))
        select input;

    public static Flow<object> Tsar(Ministers ministers) =>
        from input in Pulse.Start<object>()
        from _ in Pulse.Prime(() => ministers)
        from __ in Pulse.ToFlow(Anastasia, input)
        select input;
}
