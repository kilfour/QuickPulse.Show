using QuickPulse.Explains.Text;

namespace QuickPulse.Show.Tests.Reference.Confguring.Inlining;

public class ListInList
{
    [Fact]
    public void RunTest()
    {
        var input = new List<List<int>> { new() { 1, 2 }, new() { 3, 4 } };
        var result = Please.AllowMe()
            .ToInline<List<int>>()
            .IntroduceThis(input);
        var reader = LinesReader.FromText(result);
        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    [ 1, 2 ],", reader.NextLine());
        Assert.Equal("    [ 3, 4 ]", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}