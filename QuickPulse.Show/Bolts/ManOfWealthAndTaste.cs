using QuickPulse.Arteries;
using QuickPulse.Instruments;

namespace QuickPulse.Show.Bolts;

public class ManOfWealthAndTaste
{
    private readonly Puzzles puzzles = new();

    public ManOfWealthAndTaste ToAddSomeClass()
        => Chain.It(() => puzzles.WithClass = true, this);

    public ManOfWealthAndTaste ToUseNonLinearTime(bool noSeconds = false)
        => Chain.It(() => puzzles.Registry.UsingWibblyWobbly(noSeconds), this);

    public ManOfWealthAndTaste ToReplace<T>(Func<T, string> formatter)
        => Chain.It(() => puzzles.Registry.Register(formatter), this);

    public ManOfWealthAndTaste To<T>(Action<Troubadour<T>> customize)
        => Chain.It(() => customize(new Troubadour<T>(this, puzzles)), this);

    public string IntroduceThis<T>(T obj, bool prettyPrint = true)
        => Signal.From(The.Tsar(
            new Ministers()
            {
                PrettyPrint = prettyPrint,
                FieldsToIgnore = puzzles.FieldsToIgnore,
                PropertiesToIgnore = puzzles.PropertiesToIgnore,
                Registry = puzzles.Registry,
                TypeRegistry = puzzles.TypeRegistry,
                WithClass = puzzles.WithClass,
            }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj!)
            .GetArtery<Holden>()
            .Whispers();
}
