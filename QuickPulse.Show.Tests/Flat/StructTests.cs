namespace QuickPulse.Show.Tests.Flat;

public class StructTests
{
    public readonly record struct PointRS(int X, int Y);

    public struct PointS { public int X; public int Y; }

    [Fact]
    public void Introduce_RecordStruct() =>
        Assert.Equal("{ X: 3, Y: 4 }", Introduce.This(new PointRS(3, 4), false));

    [Fact]
    public void Introduce_Struct_WithFields() =>
        Assert.Equal("{ X: 1, Y: 2 }", Introduce.This(new PointS { X = 1, Y = 2 }, false));

    [Fact]
    public void Introduce_Struct_WithRefMember_GetsDescended()
    {
        var s = new Wrapper { Name = "hi", Data = [1, 2] };
        Assert.Equal("{ Name: \"hi\", Data: [ 1, 2 ] }", Introduce.This(s, false));
    }

    public struct Wrapper { public string? Name; public int[]? Data; }
}
