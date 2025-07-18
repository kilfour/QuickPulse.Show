using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyDictionaryTests : AbstractPrettyPrintTests
{
    [Fact]
    public void Pulse_DictionaryWithNulls()
    {
        var dict = new Dictionary<string, string?>
    {
        { "", "value" },
        { "key", null }
    };

        var result = Introduce(dict);
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    \"\": \"value\",", reader.NextLine());
        Assert.Equal("    \"key\": null", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
