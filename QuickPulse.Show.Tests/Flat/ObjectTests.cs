using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.Flat;

public class ObjectTests
{
    [Fact]
    public void Introduce_SimpleObject() =>
        Assert.Equal("{ Name: \"Alice\", Age: 30 }", Introduce.This(new Models.Person("Alice", 30), false));

    [Fact]
    public void Introduce_Cycle()
    {
        var node = new Models.Node("root");
        node.Next = node;
        var result = Introduce.This(node, false);
        Assert.Equal("{ Name: \"root\", Next: <cycle> }", result);
    }

    [Fact]
    public void Introduce_EmptyObject()
    {
        var result = Introduce.This(new { }, false);
        Assert.Equal("{ }", result);
    }

    [Fact]
    public void Introduce_Tuple()
    {
        var result = Introduce.This(("a", 1), false);
        Assert.Equal("( Item1: \"a\", Item2: 1 )", result);
    }

    [Fact]
    public void Introduce_Coach()
    {
        var result = Introduce.This(new Models.Coach("name", "email"), false);
        Assert.Equal("{ Name: \"name\", Email: \"email\", Skills: [ ] }", result);
    }
}
