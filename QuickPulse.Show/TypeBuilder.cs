using QuickPulse.Arteries;

namespace QuickPulse.Show;

public class TypeBuilder
{
    private Options options = new Options();

    public TypeBuilder To<T>(Action<OptionsBuilder<T>> customize)
    {
        customize(new OptionsBuilder<T>(this, options));
        return this;
    }

    public string IntroduceThis<T>(T obj, bool prettyPrint = true)
    {
        return Signal.From(The.Start(
            new FlowContext()
            {
                PrettyPrint = prettyPrint,
                FieldsToIgnore = options.FieldsToIgnore,
                PropertiesToIgnore = options.PropertiesToIgnore
            }))
            .SetArtery(TheString.Catcher())
            .Pulse(obj!)
            .GetArtery<Holden>()
            .Whispers();
    }
}
