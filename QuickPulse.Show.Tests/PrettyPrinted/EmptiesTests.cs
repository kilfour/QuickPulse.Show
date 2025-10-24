using QuickPulse.Explains.Text;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class EmptiesTests
{
    [Fact]
    public void Introduce_EmptyObject()
    {
        var result = Introduce.This(new { });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_EmptyList()
    {
        var result = Introduce.This(new List<int>());
        Assert.Equal("[ ]", result);
    }

    [Fact]
    public void Introduce_EmptyArray()
    {
        var result = Introduce.This(Array.Empty<string>());
        Assert.Equal("[ ]", result);
    }

    [Fact]
    public void Introduce_EmptyDictionary()
    {
        var result = Introduce.This(new Dictionary<string, int>());
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_EmptyTuple()
    {
        var result = Introduce.This(ValueTuple.Create());
        var reader = LinesReader.FromText(result);

        Assert.Equal("(", reader.NextLine());
        Assert.Equal(")", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_EmptyHashSet()
    {
        var result = Introduce.This(new HashSet<double>());
        Assert.Equal("[ ]", result);
    }

    [Fact]
    public void Introduce_EmptyNestedList()
    {
        var result = Introduce.This(new List<List<string>>());
        Assert.Equal("[ ]", result);
    }

    [Fact]
    public void Introduce_EmptyNullable()
    {
        int? value = null;
        var result = Introduce.This(value!);
        var reader = LinesReader.FromText(result);

        Assert.Equal("null", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}
