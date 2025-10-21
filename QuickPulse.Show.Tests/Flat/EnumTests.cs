namespace QuickPulse.Show.Tests.Flat;

public class EnumTests
{
    [Fact]
    public void Introduce_It() =>
        Assert.Equal("Monday", Introduce.This(DayOfWeek.Monday), false);

    [Flags]
    public enum Colors { None = 0, Red = 1, Blue = 2 }

    [Fact]
    public void Introduce_FlagsEnum_Should_Show_Named_Combination()
    {
        var actual = Introduce.This(Colors.Red | Colors.Blue);
        Assert.Equal("Red, Blue", actual, ignoreCase: false);
    }

    [Fact]
    public void Introduce_NullableEnum_Should_Render_Name()
    {
        DayOfWeek? day = DayOfWeek.Monday;
        Assert.Equal("Monday", Introduce.This(day));
    }
}
