namespace QuickPulse.Show.Tests;

public class WibblyWobblyFormattingTests
{

    private static string Introduce<T>(T val)
        => Please.AllowMe().ToUseNonLinearTime().IntroduceThis(val);

    [Fact]
    public void DateOnly()
        => Assert.Equal("20.January(2025)", Introduce(new DateOnly(2025, 1, 20)));

    [Fact]
    public void TimeOnly_O_Clock()
        => Assert.Equal("15.OClock()", Introduce(new TimeOnly(15, 0)));

    [Fact]
    public void TimeOnly_Past()
        => Assert.Equal("20.Past(15)", Introduce(new TimeOnly(15, 20)));

    [Fact]
    public void TimeOnly_To()
        => Assert.Equal("20.To(16)", Introduce(new TimeOnly(15, 40)));

    [Fact]
    public void TimeOnly_To_12()
        => Assert.Equal("20.To(Mid.Day)", Introduce(new TimeOnly(11, 40)));

    [Fact]
    public void TimeOnly_Past_12()
        => Assert.Equal("20.Past(Mid.Day)", Introduce(new TimeOnly(12, 20)));

    [Fact]
    public void TimeOnly_To_00()
        => Assert.Equal("20.To(Mid.Night)", Introduce(new TimeOnly(23, 40)));

    [Fact]
    public void TimeOnly_Past_00()
        => Assert.Equal("20.Past(Mid.Night)", Introduce(new TimeOnly(0, 20)));

    [Fact]
    public void DateTime_To_00()
        => Assert.Equal("1.January(2025).At(20.To(Mid.Night))", Introduce(new DateTime(2025, 1, 1, 23, 40, 0)));

    [Fact]
    public void DateTime_Past_00()
        => Assert.Equal("1.January(2025).At(20.Past(Mid.Night))", Introduce(new DateTime(2025, 1, 1, 0, 20, 0)));

    [Fact]
    public void TimeOnly_Seconds()
        => Assert.Equal("20.To(Mid.Night).Seconds(3)", Introduce(new TimeOnly(23, 40, 3)));

    [Fact]
    public void TimeOnly_MilliSeconds()
        => Assert.Equal("20.To(Mid.Night).Seconds(3, 4)", Introduce(new TimeOnly(23, 40, 3, 4)));

    private static string IntroduceIgnoreSeconds<T>(T val)
        => Please.AllowMe().ToUseNonLinearTime(true).IntroduceThis(val);

    [Fact]
    public void TimeOnly_Seconds_ignore()
       => Assert.Equal("20.To(Mid.Night)", IntroduceIgnoreSeconds(new TimeOnly(23, 40, 3)));

    [Fact]
    public void TimeOnly_MilliSecondsIgnore()
        => Assert.Equal("20.To(Mid.Night)", IntroduceIgnoreSeconds(new TimeOnly(23, 40, 3, 4)));
}