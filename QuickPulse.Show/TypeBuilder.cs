using QuickPulse.Arteries;

namespace QuickPulse.Show;

public class TypeBuilder
{
    private Options options = new Options();

    public TypeBuilder ToReplace<T>(Func<T, string> formatter)
    {
        options.Registry.Register(formatter);
        return this;
    }

    public TypeBuilder To<T>(Action<OptionsBuilder<T>> customize)
    {
        customize(new OptionsBuilder<T>(this, options));
        return this;
    }

    public string IntroduceThis<T>(T obj, bool prettyPrint = true)
    {
        return Signal.From(The.Tsar(
            new Ministers()
            {
                PrettyPrint = prettyPrint,
                FieldsToIgnore = options.FieldsToIgnore,
                PropertiesToIgnore = options.PropertiesToIgnore,
                Registry = options.Registry,
                TypeRegistry = options.TypeRegistry
            }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj!)
            .GetArtery<Holden>()
            .Whispers();
    }
}
