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
        from prettyPrint in Pulse.Draw<IndentControl, bool>(a => a.PrettyPrint)
        from _ in prettyPrint ? Pulse.Trace(Environment.NewLine) : Pulse.Trace(" ")
        select Flow.Continue;

    private readonly static Flow<Flow> EmitIndent =
        Pulse.TraceIf<IndentControl>(
            a => a.NeedsIndent(),
            a => new string(' ', a.Level * 4));

    private static Flow<Flow> Indented(string str) => EmitIndent.Then(Pulse.Trace(str));

    private readonly static Flow<Flow> Separator = Pulse.Trace(",");
    private readonly static Flow<Flow> Colon = Pulse.Trace(": ");
    private readonly static Flow<Flow> Null = Indented("null");

    private static Flow<Flow> Enclosed(string left, string right, Flow<Flow> innerFlow) =>
        from leftBracket in Pulse.Trace(left)
        from _ in Pulse.Scoped<IndentControl>(a => a.IncreaseLevel().EnableIndent(), innerFlow)
        from spacing in Spacing
        from __ in Pulse.Scoped<IndentControl>(a => a.EnableIndent(), Indented(right))
        select Flow.Continue;

    private static Flow<Flow> Braced(Flow<Flow> innerFlow) => Enclosed("{", "}", innerFlow);
    private static Flow<Flow> Bracketed(Flow<Flow> innerFlow) => Enclosed("(", ")", innerFlow);
    private static Flow<Flow> SquareBracketed(Flow<Flow> innerFlow) => Enclosed("[", "]", innerFlow);

    private readonly static Flow<object> Fallback =
        from input in Pulse.Start<object?>()
        from indent in EmitIndent
        from print in Pulse.Trace(input)
        select input;

    private readonly static Flow<object> Primitive =
        from input in Pulse.Start<object?>()
        from formatFunction in Pulse.Draw<Ministers, Func<object?, string>>(a => a.GetFormatFunction(input))
        from indent in EmitIndent
        from print in Pulse.Trace(formatFunction(input))
        select input;

    private readonly static Flow<object> InterspersedPrimed =
        from input in Pulse.Start<object>()
        from seperator in Pulse.When<Joiner>(a => a.NeedsSeparator(), Separator)
        from _ in Spacing.Then(Pulse.ToFlow(Anastasia!, input))
        select input;

    private readonly static Flow<IEnumerable> Interspersed =
        from input in Pulse.Start<IEnumerable>()
        from _ in Pulse.Scoped<Joiner>(a => a.Prime(),
            Pulse.ToFlow(InterspersedPrimed, input.Cast<object>()))
        select input;

    private readonly static Flow<IEnumerable> BracketedInterspersed =
        from input in Pulse.Start<IEnumerable>()
        from _ in EmitIndent.Then(SquareBracketed(Pulse.ToFlow(Interspersed, input)))
        select input;

    private readonly static Flow<IEnumerable> Collection =
        from input in Pulse.Start<IEnumerable>()
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from _ in Pulse.Scoped<IndentControl>(
            a => a.Inline(needsInlining),
            BracketedInterspersed.Dissipate())
        select input;

    private readonly static Flow<(object, object)> LabeledValue =
        from input in Pulse.Start<(object Label, object Value)>()
        from label in Pulse.ToFlow(Anastasia!, input.Label)
        from colon in Colon
        from value in Pulse.Scoped<IndentControl>(
            a => a.DisableIndent(),
            Pulse.ToFlow(Anastasia!, input.Value))
        select input;

    private static (object, object) KeyValueAsTuple(object input) =>
        (input.GetType().GetProperty("Key")?.GetValue(input)!,
        input.GetType().GetProperty("Value")?.GetValue(input)!);

    private readonly static Flow<object> KeyValuePair =
        from input in Pulse.Start<object>()
        from _ in Pulse.Scoped<IndentControl>(a => a.EnableIndent(),
            Pulse.ToFlow(LabeledValue, KeyValueAsTuple(input)))
        select input;

    private readonly static Flow<IDictionary> Dictionary =
        from input in Pulse.Start<IDictionary>()
        from _ in EmitIndent.Then(Braced(Pulse.ToFlow(Interspersed, input)))
        select input;

    private readonly static Flow<ObjectProperty> Property =
        from input in Pulse.Start<ObjectProperty>()
        from key in EmitIndent.Then(Pulse.Trace(input.Name))
        from colon in Colon
        from _ in Pulse.Scoped<IndentControl>(a => a.DisableIndent(),
            Pulse.ToFlow(Anastasia!, input.Value))
        select input;

    private readonly static Flow<object> Tuple =
        from input in Pulse.Start<object>()
        from fields in Pulse.Draw<Ministers, IEnumerable<object>>(a => a.FieldValues(input))
        from indent in EmitIndent
        from tuple in Bracketed(Pulse.ToFlow(Interspersed, fields))
        select input;

    private readonly static Flow<object> DefaultObject =
        from input in Pulse.Start<object>()
        from properties in Pulse.Draw<Ministers, IEnumerable<ObjectProperty>>(a => a.ObjectProperties(input))
        from classname in Pulse.TraceIf<Ministers>(a => a.WithClass, () => $"{input.GetType().Name} ")
        from obj in Braced(Pulse.ToFlow(Interspersed, properties))
        select input;

    private readonly static Flow<object> Object =
        from input in Pulse.Start<object>()
        from formatter in Pulse.Draw<Ministers, Func<object, string>>(a => a.GetObjectFormatFunction(input))
        from customized in Pulse.TraceIf(formatter != null, () => formatter(input))
        from defaulted in Pulse.ToFlowIf(formatter == null, DefaultObject, () => input)
        select input;

    private readonly static Flow<object> MaybeInlinedObject =
        from input in Pulse.Start<object>()
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from indent in EmitIndent
        from obj in Pulse.Scoped<IndentControl>(
            a => a.Inline(needsInlining),
            Pulse.ToFlow(Object, input))
        select input;

    private static Flow<Flow> Guarded(object node, Flow<Flow> inner) =>
        from _ in Pulse.Scoped<CycleGuard>(m => m.Enter(node), inner)
        from __ in Pulse.Scoped<CycleGuard>(m => m.Exit(node), Pulse.NoOp())
        select Flow.Continue;

    private readonly static Flow<object> Anastasia =
        from input in Pulse.Start<object>()
        from ministers in Pulse.Draw<Ministers>()
        from cycleGuard in Pulse.Draw<CycleGuard>()
        let registry = ministers.Registry
        from _ in Pulse.FirstOf(
            (() => input == null,                 /**/ () => Null),
            (() => ministers.HasFormatter(input), /**/ () => Pulse.ToFlow(Anastasia!, ministers.GetFormattedObject(input)!)),
            (() => Is.Primitive(input, registry), /**/ () => Pulse.ToFlow(Primitive, input)),
            (() => Is.ObjectProperty(input),      /**/ () => Pulse.ToFlow(Property, (ObjectProperty)input)),
            (() => cycleGuard.IsOnPath(input),    /**/ () => Pulse.ToFlow(Cycle, input)),
            (() => Is.Dictionary(input),          /**/ () => Guarded(input, Pulse.ToFlow(Dictionary, (IDictionary)input))),
            (() => Is.KeyValuePair(input),        /**/ () => Guarded(input, Pulse.ToFlow(KeyValuePair, input))),
            (() => Is.Collection(input),          /**/ () => Guarded(input, Pulse.ToFlow(Collection, (IEnumerable)input))),
            (() => Is.Tuple(input),               /**/ () => Guarded(input, Pulse.ToFlow(Tuple, input))),
            (() => Is.Object(input),              /**/ () => Guarded(input, Pulse.ToFlow(MaybeInlinedObject, input))),
            (() => true,                          /**/ () => Pulse.ToFlow(Fallback, input)))
        select input;

    public static Flow<object> Tsar(Ministers ministers, bool prettyPrint) =>
        from input in Pulse.Start<object>()
        from _1 in Pulse.Prime(() => ministers)
        from _2 in Pulse.Prime(() => new Joiner())
        from _3 in Pulse.Prime(() => new CycleGuard())
        from _4 in Pulse.Prime(() => new IndentControl(prettyPrint))
        from __ in Pulse.ToFlow(Anastasia, input)
        select input;
}
