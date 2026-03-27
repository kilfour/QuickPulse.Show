using System.Collections;

namespace QuickPulse.Show.Bolts;

public static class The
{
    private const string CycleMarker = "<cycle>";

    private static Flow<Flow> Cycle(object input) =>
        from ministers in Pulse.Draw<Ministers>()
        let formatFunction = ministers.GetReferencingFormatFunction(input)
        from _ in formatFunction is null ? Indented(CycleMarker) : Indented(formatFunction(input))
        select Flow.Continue;

    private readonly static Flow<Flow> NewLine =
        Pulse.Trace(Environment.NewLine).Then(Pulse.Manipulate<IndentControl>(a => a.OnNewLine(true)).Dissipate());

    private readonly static Flow<Flow> Space =
        Pulse.Trace(" ").Then(Pulse.Manipulate<IndentControl>(a => a.OnNewLine(false)).Dissipate());

    private readonly static Flow<Flow> Spacing =
        from prettyPrint in Pulse.Draw<IndentControl, bool>(a => a.PrettyPrint && !a.NeedsInlining)
        from _ in prettyPrint ? NewLine : Space
        select Flow.Continue;

    private static string Indent(int level) => new(' ', level * 4);

    private readonly static Flow<Flow> EmitIndent =
        Pulse.TraceIf<IndentControl>(
            a => a.NeedsIndent(),
            a => Indent(a.Level));

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

    private static Flow<Flow> Fallback(object input) =>
        from indent in EmitIndent
        from print in Pulse.Trace(input)
        select Flow.Continue;

    private static Flow<Flow> Primitive(object input) =>
        from formatFunction in Pulse.Draw<Ministers, Func<object?, string>>(a => a.GetFormatFunction(input))
        from indent in EmitIndent
        from print in Pulse.Trace(formatFunction(input))
        select Flow.Continue;

    private static Flow<Flow> SystemType(object input) =>
        from formatFunction in Pulse.Draw<Ministers, Func<object, string>>(a => a.GetSystemTypeFormatFunction(input))
        from indent in EmitIndent
        from print in Pulse.Trace(formatFunction(input))
        select Flow.Continue;

    private static Flow<Flow> InterspersedPrimed(object input) =>
        from seperator in Pulse.When<Joiner>(a => a.NeedsSeparator(), Separator)
        from _ in Spacing.Then(Pulse.ToFlow(Anastasia!, input))
        select Flow.Continue;

    private static Flow<Flow> Interspersed(IEnumerable input) =>
        from _ in Pulse.Scoped<Joiner>(a => a.Prime(),
            Pulse.ToFlow(InterspersedPrimed, input.Cast<object>()))
        select Flow.Continue;

    private static Flow<Flow> BracketedInterspersed(IEnumerable input) =>
        EmitIndent.Then(SquareBracketed(Pulse.ToFlow(Interspersed, input)));

    private static Flow<Flow> Collection(IEnumerable input) =>
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from indent in Pulse.TraceIf<IndentControl>(a => needsInlining && a.IsNewLine(), a => Indent(a.Level))
        from prefix in Pulse.Draw<Ministers, string>(a => a.GetPrefix(input))
        from tracePrefix in Pulse.Trace(prefix)
        from _ in Pulse.Scoped<IndentControl>(
            a => a.Inline(needsInlining), BracketedInterspersed(input))
        select Flow.Continue;

    private static Flow<Flow> LabeledValue((object Label, object Value) input) =>
        from label in Pulse.ToFlow(Anastasia!, input.Label)
        from _ in Pulse.Manipulate<IndentControl>(a => a.OnNewLine(false))
        from colon in Colon
        from value in Pulse.Scoped<IndentControl>(
            a => a.DisableIndent(),
            Pulse.ToFlow(Anastasia!, input.Value))
        select Flow.Continue;

    private static (object, object) KeyValueAsTuple(object input) =>
        (input.GetType().GetProperty("Key")?.GetValue(input)!,
        input.GetType().GetProperty("Value")?.GetValue(input)!);

    private static Flow<Flow> KeyValuePair(object input) =>
        from _ in Pulse.Scoped<IndentControl>(a => a.EnableIndent(),
            Pulse.ToFlow(LabeledValue, KeyValueAsTuple(input)))
        select Flow.Continue;

    private static Flow<Flow> Dictionary(IDictionary input) =>
        from _ in EmitIndent.Then(Braced(Pulse.ToFlow(Interspersed, input)))
        select Flow.Continue;

    private static Flow<Flow> Property(ObjectProperty input) =>
        from key in EmitIndent.Then(Pulse.Trace(input.Name))
        from disableNewLine in Pulse.Manipulate<IndentControl>(a => a.OnNewLine(false))
        from colon in Colon
        from _ in Pulse.Scoped<IndentControl>(a => a.DisableIndent(),
            Pulse.ToFlow(Anastasia!, input.Value))
        select Flow.Continue;

    private static Flow<Flow> Tuple(object input) =>
        from fields in Pulse.Draw<Ministers, IEnumerable<object>>(a => a.FieldValues(input))
        from indent in EmitIndent
        from tuple in Bracketed(Pulse.ToFlow(Interspersed, (IEnumerable)fields))
        select Flow.Continue;

    private static Flow<Flow> DefaultObject(object input) =>
        from prefix in Pulse.Draw<Ministers, string>(a => a.GetPrefix(input))
        from tracePrefix in Pulse.Trace(prefix)
        from properties in Pulse.Draw<Ministers, IEnumerable<ObjectProperty>>(a => a.ObjectProperties(input))
        from classname in Pulse.TraceIf<Ministers>(a => a.WithClass, () => $"{input.GetType().Name} ")
        from obj in Braced(Pulse.ToFlow(Interspersed, properties))
        select Flow.Continue;

    private static Flow<Flow> Object(object input) =>
        from formatter in Pulse.Draw<Ministers, Func<object, string>>(a => a.GetObjectFormatFunction(input))
        from customized in Pulse.TraceIf(formatter != null, () => formatter(input))
        from defaulted in Pulse.ToFlowIf(formatter == null, DefaultObject, () => input)
        select Flow.Continue;

    private static Flow<Flow> MaybeInlinedObject(object input) =>
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from indent in EmitIndent
        from obj in Pulse.Scoped<IndentControl>(
            a => a.Inline(needsInlining),
            Pulse.ToFlow(Object, input))
        select Flow.Continue;

    private static Flow<Flow> Guarded(object node, Flow<Flow> inner) =>
        from _ in Pulse.Scoped<CycleGuard>(m => m.Enter(node), inner)
        from __ in Pulse.Scoped<CycleGuard>(m => m.Exit(node), Pulse.NoOp())
        select Flow.Continue;

    private static Flow<Flow> Anastasia(object input) =>
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
            (() => input is Type,                 /**/ () => Pulse.ToFlow(SystemType, input)),
            (() => Is.Object(input),              /**/ () => Guarded(input, Pulse.ToFlow(MaybeInlinedObject, input))),
            (() => true,                          /**/ () => Pulse.ToFlow(Fallback, input)))
        select Flow.Continue;

    public static Flow<Flow> Tsar(Ministers ministers, bool prettyPrint, object input) =>
        from _1 in Pulse.Prime(() => ministers)
        from _2 in Pulse.Prime(() => new Joiner())
        from _3 in Pulse.Prime(() => new CycleGuard())
        from _4 in Pulse.Prime(() => new IndentControl(prettyPrint))
        from __ in Pulse.ToFlow(Anastasia, input)
        select Flow.Continue;
}
