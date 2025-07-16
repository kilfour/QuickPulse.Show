using System.Text;


namespace QuickPulse.Show.Tests._tools;

public class TheStringCatcher : IArtery
{
    private readonly StringBuilder builder = new StringBuilder();
    public void Flow(params object[] data)
    {
        foreach (var item in data)
        {
            builder.Append(item);
        }
    }
    public string GetText() { return builder.ToString(); }
}
