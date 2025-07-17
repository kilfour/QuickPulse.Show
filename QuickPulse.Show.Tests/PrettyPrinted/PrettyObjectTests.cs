using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyObjectTests : AbstractPrettyPrintTests
{
    [Fact]
    public void Pulse_SimpleObject()
    {
        var result = Pulse(new Person("Alice", 30));
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"Alice\",", reader.NextLine());
        Assert.Equal("    Age: 30", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyObject()
    {
        var result = Pulse(new { });
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
    }


    [Fact]
    public void Pulse_AnonymousWithNulls()
    {
        var result = Pulse(new { A = (string?)null, B = (int?)null });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    A: null,", reader.NextLine());
        Assert.Equal("    B: null", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
