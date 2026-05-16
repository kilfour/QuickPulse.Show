using System.Collections;
using QuickPulse.Show.Bolts.State;

namespace QuickPulse.Show.Bolts;

public static class The
{
    private const string CycleMarker = "<cycle>";

    private static Flow<Flow> Cycle(object input) =>
        from formatter in Pulse.Draw<Ministers, Func<object, string>>(
            a => a.GetReferencingFormatFunction(input))
        from _ in formatter is null
            ? Indented(CycleMarker)
            : Indented(formatter(input))
        select Flow.Continue;

    private readonly static Flow<Flow> NewLine =
        Pulse
            .Trace(Environment.NewLine)
            .Manipulate<IndentControl>(a => a.OnNewLine(true))
            .Dissipate();

    private readonly static Flow<Flow> Space =
        Pulse
            .Trace(" ")
            .Manipulate<IndentControl>(a => a.OnNewLine(false))
            .Dissipate();

    private readonly static Flow<Flow> Spacing =
        from prettyPrint in Pulse.Draw<IndentControl, bool>(a => a.PrettyPrint && !a.NeedsInlining)
        from _ in prettyPrint ? NewLine : Space
        select Flow.Continue;

    private static string Indent(int level) => new(' ', level * 4);

    private readonly static Flow<Flow> EmitIndent =
        Pulse.Draw<IndentControl>()
            .TraceIf(a => a.NeedsIndent(), a => Indent(a.Level));

    private static Flow<Flow> Indented(string str) => EmitIndent.Trace(str);

    private readonly static Flow<Flow> Separator = Pulse.Trace(",");
    private readonly static Flow<Flow> Colon = Pulse.Trace(": ");
    private readonly static Flow<Flow> Null = Indented("null");

    private static Flow<Flow> Enclosed(string left, string right, Flow<Flow> innerFlow) =>
        Pulse
            .Trace(left)
            .Scoped<IndentControl>(a => a.IncreaseLevel().EnableIndent(), innerFlow)
            .Then(Spacing)
            .Scoped<IndentControl>(a => a.EnableIndent(), Indented(right))
            .Dissipate();

    private static Flow<Flow> Braced(Flow<Flow> innerFlow) => Enclosed("{", "}", innerFlow);
    private static Flow<Flow> Bracketed(Flow<Flow> innerFlow) => Enclosed("(", ")", innerFlow);
    private static Flow<Flow> SquareBracketed(Flow<Flow> innerFlow) => Enclosed("[", "]", innerFlow);

    private static Flow<Flow> Fallback(object input) => EmitIndent.Trace(input);

    private static Flow<Flow> Formatted(object input, Func<Ministers, Func<object?, string>> draw) =>
        from formatFunction in Pulse.Draw(draw)
        from _ in EmitIndent.Trace(formatFunction(input))
        select Flow.Continue;

    private static Flow<Flow> Primitive(object input) =>
        Formatted(input, a => a.GetFormatFunction(input));

    private static Flow<Flow> SystemType(object input) =>
        Formatted(input, a => a.GetSystemTypeFormatFunction(input)!);

    private static Flow<Flow> InterspersedPrimed(object input) =>
        Pulse
            .When<Joiner>(a => a.NeedsSeparator(), Separator)
            .Then(Spacing.ToFlow(Anastasia!, input));

    private static Flow<Flow> Interspersed(IEnumerable input) =>
        Pulse.Scoped<Joiner>(a => a.Prime(),
            Pulse.ToFlow(InterspersedPrimed, input.Cast<object>()));

    private static Flow<Flow> BracketedInterspersed(IEnumerable input) =>
        EmitIndent.Then(SquareBracketed(Pulse.ToFlow(Interspersed, input)));

    private static Flow<Flow> Collection(IEnumerable input) =>
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from _ in
            Pulse
                .Draw<IndentControl>()
                .TraceIf(a => needsInlining && a.IsNewLine(), a => Indent(a.Level))
                .Draw<Ministers, string>(a => a.GetPrefix(input)).Trace()
                .Scoped<IndentControl>(
                    a => a.Inline(needsInlining),
                    BracketedInterspersed(input))
        select Flow.Continue;

    private static Flow<Flow> LabeledValue((object Label, object Value) input) =>
        Pulse
            .ToFlow(Anastasia!, input.Label)
            .Manipulate<IndentControl>(a => a.OnNewLine(false))
            .Then(Colon)
            .Scoped<IndentControl>(
                a => a.DisableIndent(),
                Pulse.ToFlow(Anastasia!, input.Value));

    private static (object, object) KeyValueAsTuple(object input) =>
        (
            input.GetType().GetProperty("Key")?.GetValue(input)!,
            input.GetType().GetProperty("Value")?.GetValue(input)!
        );

    private static Flow<Flow> KeyValuePair(object input) =>
        Pulse.Scoped<IndentControl>(a => a.EnableIndent(),
            Pulse.ToFlow(LabeledValue, KeyValueAsTuple(input)));

    private static Flow<Flow> Dictionary(IDictionary input) =>
        EmitIndent.Then(Braced(Pulse.ToFlow(Interspersed, input)));

    private static Flow<Flow> Property(ObjectProperty input) =>
        EmitIndent
            .Trace(input.Name)
            .Manipulate<IndentControl>(a => a.OnNewLine(false))
            .Then(Colon)
            .Scoped<IndentControl>(a => a.DisableIndent(), Pulse.ToFlow(Anastasia!, input.Value));

    private static Flow<Flow> Tuple(object input) =>
        from fields in Pulse.Draw<Ministers, IEnumerable<object>>(a => a.FieldValues(input))
        from _ in
            EmitIndent
                .Then(Bracketed(Pulse.ToFlow(Interspersed, (IEnumerable)fields)))
        select Flow.Continue;

    private static Flow<Flow> ObjectHeader(object input) =>
        from prefix in Pulse.Draw<Ministers, string>(a => a.GetPrefix(input))
        from withClass in Pulse.Draw<Ministers, bool>(a => a.WithClass)
        from _ in
            Pulse
                .Trace(prefix)
                .TraceIf(withClass, _ => $"{input.GetType().Name} ")
        select Flow.Continue;

    private static Flow<Flow> DefaultObject(object input) =>
        from properties in Pulse.Draw<Ministers, IEnumerable<ObjectProperty>>(a => a.ObjectProperties(input))
        from _ in
            ObjectHeader(input)
                .Then(Braced(Pulse.ToFlow(Interspersed, properties)))
        select Flow.Continue;

    private static Flow<Flow> Object(object input) =>
        from formatter in Pulse.Draw<Ministers, Func<object, string>>(a => a.GetObjectFormatFunction(input))
        from _ in Pulse
            .TraceIf(formatter != null, () => formatter(input))
            .ToFlowIf(formatter == null, DefaultObject, () => input)
        select Flow.Continue;

    private static Flow<Flow> MaybeInlinedObject(object input) =>
        from needsInlining in Pulse.Draw<Ministers, bool>(a => a.NeedsInlining(input))
        from _ in EmitIndent
            .Scoped<IndentControl>(
                a => a.Inline(needsInlining),
                Pulse.ToFlow(Object, input))
        select Flow.Continue;

    private static Flow<Flow> Guarded(object node, Flow<Flow> inner) =>
        Pulse.Scoped<CycleGuard>(m => m.Enter(node), inner);

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
        Pulse
            .Prime(() => ministers).Dissipate()
            .Prime(() => new Joiner()).Dissipate()
            .Prime(() => new CycleGuard()).Dissipate()
            .Prime(() => new IndentControl(prettyPrint)).Dissipate()
            .ToFlow(Anastasia, input);
}
