using QuickPulse.Arteries;
using QuickPulse.Instruments;
using QuickPulse.Show.Reflects;

namespace QuickPulse.Show.Bolts;

public class ManOfWealthAndTaste
{
    private readonly Puzzles puzzles = new();

    public ManOfWealthAndTaste ToPrettyPrint()
        => Chain.It(() => puzzles.PrettyPrint = true, this);

    public ManOfWealthAndTaste ToAddSomeClass()
        => Chain.It(() => puzzles.WithClass = true, this);

    public ManOfWealthAndTaste ToSelfReference<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.SelfReferencingFormatter(formatter), this);

    public ManOfWealthAndTaste ToInline<T>()
        => Chain.It(() => puzzles.InlineType(typeof(T)), this);

    public ManOfWealthAndTaste ToUseNonLinearTime(bool noSeconds = false)
        => Chain.It(() => puzzles.Registry.UsingWibblyWobbly(noSeconds), this);

    public ManOfWealthAndTaste ToReplace<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.Registry.Register(formatter), this);

    public ManOfWealthAndTaste ToReplaceAll(Func<Type, bool> predicate, Func<object, string> formatter)
        => Chain.It(() => puzzles.RegisterFormatter(predicate, formatter), this);
    public ManOfWealthAndTaste ToSubstituteWithPropertyNamed<T>(string propertyName)
        => Chain.It(() => puzzles.RegisterFormatter(a => a.HasPropertyNamed<T>(propertyName), a => a.GetValueFor(propertyName)), this);

    public ManOfWealthAndTaste ToRegisterSystemType<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.RegisterSystemTypeFormatter(formatter), this);

    public ManOfWealthAndTaste To<T>(Action<Troubadour<T>> customize)
        => Chain.It(() => customize(new Troubadour<T>(this, puzzles)), this);

    public string IntroduceThis<T>(T obj)
        => Signal.From(The.Tsar(
            new Ministers()
            {
                FieldsToIgnore = puzzles.FieldsToIgnore,
                PropertiesToIgnore = puzzles.PropertiesToIgnore,
                Registry = puzzles.Registry,
                TypeRegistry = puzzles.TypeRegistry,
                SystemTypeRegistry = puzzles.SystemTypeRegistry,
                WithClass = puzzles.WithClass,
                SelfReferencingRegistry = puzzles.SelfReferencingRegistry,
                InlinedTypes = puzzles.InlinedTypes,
                Formatters = puzzles.Formatters

            }, puzzles.PrettyPrint))
            .SetArtery(Text.Capture())
            .Pulse(obj!)
            .GetArtery<StringSink>()
            .Content();

}
