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
        Assert.Equal("( \"a\", 1 )", result);
    }

    [Fact]
    public void Introduce_Tuple_of_Lists()
    {
        (IEnumerable<int>, IEnumerable<int>) input = ([1], [1]);
        var result = Introduce.This(input, false);
        Assert.Equal("( [ 1 ], [ 1 ] )", result);
    }

    [Fact]
    public void Introduce_Coach()
    {
        var result = Introduce.This(new Models.Coach("name", "email"), false);
        Assert.Equal("{ Name: \"name\", Email: \"email\", Skills: [ ] }", result);
    }

    [Fact]
    public void Object_With_enum()
    {
        var result = Introduce.This(new Models.Enumy() { Day = DayOfWeek.Wednesday }, false);
        Assert.Equal("{ Day: Wednesday }", result);
    }

    [Fact]
    public void Object_Horses()
    {
        var input = new Models.Horses.TimeSlotJasonDTO
        {
            Day = Models.Horses.WeekDays.Wednesday,
            Start = 8,
            End = 17
        };
        var result = Introduce.This(input, false);
        Assert.Equal("{ Day: Wednesday, Start: 8, End: 17 }", result);
    }

    public readonly record struct GridPoint(int X, int Y);

    [Fact]
    public void Struct()
    {
        var result = Introduce.This(new GridPoint(5, 4), false);
        Assert.Equal("{ X: 5, Y: 4 }", result); // <= uses fallback
    }
}