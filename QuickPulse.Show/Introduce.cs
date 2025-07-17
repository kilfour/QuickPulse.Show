namespace QuickPulse.Show;

public static class Introduce
{
    public static string This(object obj, bool prettyPrint = true)
    {
        var artery = new TheStringCatcher();
        var flowContext = new FlowContext() { PrettyPrint = prettyPrint };
        Signal.From(The.Start(flowContext))
            .SetArtery(artery)
            .Pulse(obj);
        return artery.GetText();
    }
}
