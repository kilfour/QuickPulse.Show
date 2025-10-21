namespace QuickPulse.Show.Tests.Flat;

public class NullableTests
{
    [Fact]
    public void Introduce_It() =>
        Assert.Equal("5", Introduce.This((int?)5), false);

    [Fact]
    public void Introduce_Null() =>
        Assert.Equal("null", Introduce.This((int?)null!), false);
}
