using QuickPulse.Explains.Text;


namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyDictionaryTests
{
    [Fact]
    public void Introduce_DictionaryWithNulls()
    {
        var dict = new Dictionary<string, string?>
    {
        { "", "value" },
        { "key", null }
    };

        var result = Introduce.This(dict);
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    \"\": \"value\",", reader.NextLine());
        Assert.Equal("    \"key\": null", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
