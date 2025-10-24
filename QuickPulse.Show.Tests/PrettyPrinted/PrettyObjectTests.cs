using System.Text.Json;
using QuickPulse.Explains.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyObjectTests
{
    [Fact]
    public void Introduce_SimpleObject()
    {
        var result = Introduce.This(new Models.Person("Alice", 30));
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"Alice\",", reader.NextLine());
        Assert.Equal("    Age: 30", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void JsonSerializer_SimpleObject()
    {
        var result =
            JsonSerializer.Serialize(new Models.Person("Alice", 30),
                new JsonSerializerOptions { WriteIndented = true });

        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("  \"Name\": \"Alice\",", reader.NextLine());
        Assert.Equal("  \"Age\": 30", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_EmptyObject()
    {
        var result = Introduce.This(new { });
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
    }


    [Fact]
    public void Introduce_AnonymousWithNulls()
    {
        var result = Introduce.This(new { A = (string?)null, B = (int?)null });
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    A: null,", reader.NextLine());
        Assert.Equal("    B: null", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_Coach()
    {
        var result = Introduce.This(new Models.Coach("name", "email"));
        var reader = LinesReader.FromText(result);

        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"name\",", reader.NextLine());
        Assert.Equal("    Email: \"email\",", reader.NextLine());
        Assert.Equal("    Skills: [", reader.NextLine());
        Assert.Equal("    ]", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_Cycle()
    {
        var node = new Models.Node("root");
        node.Next = node;
        var result = Introduce.This(node, true);
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"root\",", reader.NextLine());
        Assert.Equal("    Next: <cycle>", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
    }
}
