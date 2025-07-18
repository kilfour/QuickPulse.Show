namespace QuickPulse.Show.Tests.Flat;

public class CycleStructTests
{
    private class Node { public object? Value { get; set; } }
    private struct Wrapper { public Node Node; }

    [Fact]
    public void Does_not_stack_overflow_on_struct_in_reference_cycle()
    {
        var node = new Node();
        var wrapper = new Wrapper { Node = node };
        node.Value = wrapper;
        Introduce.This(node);
    }
}