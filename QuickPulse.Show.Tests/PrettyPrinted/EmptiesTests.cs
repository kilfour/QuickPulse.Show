using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class EmptiesTests : AbstractPrettyPrintTests
{
    [Fact]
    public void Pulse_EmptyObject()
    {
        var result = Pulse(new { });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyList()
    {
        var result = Pulse(new List<int>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyArray()
    {
        var result = Pulse(new string[0]);
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyDictionary()
    {
        var result = Pulse(new Dictionary<string, int>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyTuple()
    {
        var result = Pulse(ValueTuple.Create());
        var reader = LinesReader.FromText(result);

        Assert.Equal("(", reader.NextLine());
        Assert.Equal(")", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyHashSet()
    {
        var result = Pulse(new HashSet<double>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyNestedList()
    {
        var result = Pulse(new List<List<string>>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyNullable()
    {
        int? value = null;
        var result = Pulse(value!);
        var reader = LinesReader.FromText(result);

        Assert.Equal("null", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
