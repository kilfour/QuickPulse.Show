using QuickPulse.Show.Tests._tools;
namespace QuickPulse.Show.Tests;

public class CollectionFlowTests : AbstractFlowTests
{
    [Fact]
    public void Pulse_IntList() =>
        Assert.Equal("[ 1, 2, 3 ]", Pulse(new List<int>([1, 2, 3])));

    [Fact]
    public void Pulse_StringList() =>
        Assert.Equal("[ \"1\", \"2\", \"3\" ]", Pulse(new List<string>(["1", "2", "3"])));

    [Fact]
    public void Pulse_NestedList() =>
        Assert.Equal("[ [ 1, 2, 3 ], [ 4 ], [ 5, 6 ] ]",
            Pulse(new List<List<int>>([
                new List<int>([1, 2, 3]),
                new List<int>([4]),
                new List<int>([5, 6])])));
}
