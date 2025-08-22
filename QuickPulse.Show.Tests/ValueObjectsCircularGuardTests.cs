namespace QuickPulse.Show.Tests;

public class ValueObjectsCircularGuardTests
{
    public record Val(int V);
    public class Vals { public Val One { get; set; } public Val Two { get; set; } public Val Three { get; set; } }

    [Fact]
    public void ShouldNotDetect()
    {
        var result = Introduce.This(new Vals { One = new Val(1), Two = new Val(1), Three = new Val(3) }, false);
        Assert.Equal("{ One: { V: 1 }, Two: { V: 1 }, Three: { V: 3 } }", result);
    }

    public class Box { public Val Content { get; set; } }

    [Fact]
    public void ShouldNotDetectButDoes()
    {
        var a = new Box { Content = new Val(1) };
        var b = new Box { Content = new Val(1) };
        var result = Introduce.This(new { A = a, B = b }, false);
        Assert.Equal("{ A: { Content: { V: 1 } }, B: { Content: { V: 1 } } }", result);
    }

    public class Person { public string Name { get; set; } public Address Address { get; set; } }
    public class Address { public string City { get; set; } }

    [Fact]
    public void SharedNodeAcrossBranches_IsNotCircular_ButCurrentGuardFlagsIt()
    {
        var shared = new Address { City = "Ghent" };
        var root = new
        {
            A = new Person { Name = "A", Address = shared },
            B = new Person { Name = "B", Address = shared } // same instance, different branch
        };

        var result = Introduce.This(root, false);

        Assert.Equal(
            "{ A: { Name: \"A\", Address: { City: \"Ghent\" } }, B: { Name: \"B\", Address: { City: \"Ghent\" } } }",
            result);
    }
}