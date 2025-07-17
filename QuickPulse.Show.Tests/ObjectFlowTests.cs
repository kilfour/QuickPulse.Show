using QuickPulse.Show.Tests._tools;
namespace QuickPulse.Show.Tests;

public class ObjectFlowTests : AbstractFlowTests
{
    [Fact]
    public void Pulse_SimpleObject() =>
        Assert.Equal("{ Name: \"Alice\", Age: 30 }", Pulse(new Person("Alice", 30)));

    [Fact]
    public void Pulse_Cycle()
    {
        var node = new Node("root");
        node.Next = node; // cyclic

        var result = Pulse(node);
        Assert.Equal("{ Name: \"root\", Next: <cycle> }", result);
    }

    public class Node
    {
        public string Name { get; }
        public Node? Next { get; set; }
        public Node(string name) => Name = name;
    }
}
