using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyObjectTests : AbstractPrettyPrintTests
{
    [Fact]
    public void Pulse_SimpleObject()
    {
        var result = Introduce(new Models.Person("Alice", 30));
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
        var result = Introduce(new { });
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
    }


    [Fact]
    public void Pulse_AnonymousWithNulls()
    {
        var result = Introduce(new { A = (string?)null, B = (int?)null });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    A: null,", reader.NextLine());
        Assert.Equal("    B: null", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_Coach()
    {
        var result = Introduce(new Models.Coach("name", "email"));
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"name\",", reader.NextLine());
        Assert.Equal("    Email: \"email\",", reader.NextLine());
        Assert.Equal("    Skills: [", reader.NextLine());
        Assert.Equal("    ]", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
