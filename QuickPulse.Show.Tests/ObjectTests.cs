using QuickExplainIt.Text;
using QuickPulse.Show.Tests._tools;
namespace QuickPulse.Show.Tests;

public class ObjectTests : AbstractFlowTests
{
    [Fact]
    public void Pulse_SimpleObject() =>
        Assert.Equal("{ Name: \"Alice\", Age: 30 }", Pulse(new Person("Alice", 30)));

    [Fact]
    public void Pulse_Cycle()
    {
        var node = new Node("root");
        node.Next = node;
        var result = Pulse(node);
        Assert.Equal("{ Name: \"root\", Next: <cycle> }", result);
    }

    [Fact]
    public void Pulse_EmptyObject()
    {
        var result = Pulse(new { });
        Assert.Equal("{ }", result);
    }

    [Fact]
    public void Pulse_Tuple()
    {
        var result = Pulse(("a", 1));
        Assert.Equal("( Item1: \"a\", Item2: 1 )", result);
    }
}
