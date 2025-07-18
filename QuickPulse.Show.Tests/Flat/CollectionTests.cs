using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.Flat;

public class CollectionTests
{
    [Fact]
    public void Introduce_IntList() =>
        Assert.Equal("[ 1, 2, 3 ]", Introduce.This(new List<int>([1, 2, 3]), false));

    [Fact]
    public void Introduce_StringList() =>
        Assert.Equal("[ \"1\", \"2\", \"3\" ]", Introduce.This(new List<string>(["1", "2", "3"]), false));

    [Fact]
    public void Introduce_NestedList() =>
        Assert.Equal("[ [ 1, 2, 3 ], [ 4 ], [ 5, 6 ] ]",
           Introduce.This(new List<List<int>>([
                new List<int>([1, 2, 3]),
                new List<int>([4]),
                new List<int>([5, 6])]), false));
    [Fact]
    public void Introduce_ObjectList() =>
        Assert.Equal("[ { Name: \"a\", Age: 1 }, { Name: \"b\", Age: 2 } ]",
           Introduce.This(new List<Models.Person>([new Models.Person("a", 1), new Models.Person("b", 2)]), false));
}
