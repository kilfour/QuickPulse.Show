using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class EmptiesTests : AbstractPrettyPrintTests
{
    [Fact]
    public void Pulse_EmptyObject()
    {
        var result = Introduce(new { });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyList()
    {
        var result = Introduce(new List<int>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyArray()
    {
        var result = Introduce(new string[0]);
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyDictionary()
    {
        var result = Introduce(new Dictionary<string, int>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyTuple()
    {
        var result = Introduce(ValueTuple.Create());
        var reader = LinesReader.FromText(result);

        Assert.Equal("(", reader.NextLine());
        Assert.Equal(")", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyHashSet()
    {
        var result = Introduce(new HashSet<double>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyNestedList()
    {
        var result = Introduce(new List<List<string>>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Pulse_EmptyNullable()
    {
        int? value = null;
        var result = Introduce(value!);
        var reader = LinesReader.FromText(result);

        Assert.Equal("null", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
