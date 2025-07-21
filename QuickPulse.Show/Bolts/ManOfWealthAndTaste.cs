using QuickPulse.Arteries;

namespace QuickPulse.Show.Bolts;

public class ManOfWealthAndTaste
{
    private Puzzles puzzles = new Puzzles();

    public ManOfWealthAndTaste ToReplace<T>(Func<T, string> formatter)
    {
        puzzles.Registry.Register(formatter);
        return this;
    }

    public ManOfWealthAndTaste To<T>(Action<Troubadour<T>> customize)
    {
        customize(new Troubadour<T>(this, puzzles));
        return this;
    }

    public string IntroduceThis<T>(T obj, bool prettyPrint = true)
    {
        return Signal.From(The.Tsar(
            new Ministers()
            {
                PrettyPrint = prettyPrint,
                FieldsToIgnore = puzzles.FieldsToIgnore,
                PropertiesToIgnore = puzzles.PropertiesToIgnore,
                Registry = puzzles.Registry,
                TypeRegistry = puzzles.TypeRegistry
            }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj!)
            .GetArtery<Holden>()
            .Whispers();
    }
}
