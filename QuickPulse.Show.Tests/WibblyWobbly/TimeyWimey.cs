using QuickPulse.Instruments;
using WibblyWobbly;

namespace QuickPulse.Show.Tests.WibblyWobbly;

public class TimeyWimey
{
    [Fact]
    public void HumanDate_To_DateOnly()
    {
        Assert.Equal(new DateOnly(2025, 1, 20), 20.January(2025));
    }

    [Fact]
    public void HumanDate_To_DateTime()
    {
        Assert.Equal(new DateTime(2025, 1, 20, 10, 0, 0), 20.January(2025, 10.OClock()));
    }

    [Fact]
    public void HumanTime_OClock()
    {
        Assert.Equal(new TimeOnly(5, 0, 0), 5.OClock());
        Assert.Equal(new TimeOnly(0, 0, 0), 0.OClock());

        Assert.Throws<ComputerSaysNo>(() => 25.OClock());

        Assert.Throws<ComputerSaysNo>(() => (-2).OClock());
        Assert.Throws<ComputerSaysNo>(() => 100.OClock());
    }

    [Fact]
    public void HumanTime_To()
    {
        Assert.Equal(new TimeOnly(11, 50, 0), 10.To(12));
        Assert.Equal(new TimeOnly(23, 50, 0), 10.To(0));

        Assert.Throws<ComputerSaysNo>(() => 10.To(25));
        Assert.Throws<ComputerSaysNo>(() => 60.To(2));
        Assert.Throws<ComputerSaysNo>(() => 1.To(-2));
        Assert.Throws<ComputerSaysNo>(() => 59.To(100));
        Assert.Throws<ComputerSaysNo>(() => (-2).To(2));
        Assert.Throws<ComputerSaysNo>(() => 100.To(2));
    }

    [Fact]
    public void HumanTime_To_Seconds()
    {
        Assert.Equal(new TimeOnly(11, 50, 30), 10.To(12).Seconds(30));
        Assert.Equal(new TimeOnly(23, 50, 30, 55), 10.To(0).Seconds(30, 55));
    }

    [Fact]
    public void HumanTime_Past()
    {
        Assert.Equal(new TimeOnly(11, 20, 0), 20.Past(11));

        Assert.Throws<ComputerSaysNo>(() => 10.Past(25));
        Assert.Throws<ComputerSaysNo>(() => 10.Past(-2));

        Assert.Throws<ComputerSaysNo>(() => (-2).Past(2));
        Assert.Throws<ComputerSaysNo>(() => 100.Past(2));
    }

    [Fact]
    public void PM()
    {
        Assert.Equal(new TimeOnly(17, 0), 5.OClock().PM());

        Assert.Throws<ComputerSaysNo>(() => new TimeOnly(0, 0).PM());
        Assert.Throws<ComputerSaysNo>(() => new TimeOnly(13, 0).PM());
    }

    [Fact]
    public void TwelveOClockPm_Should_BeNoon_NotMidnight()
        => Assert.Equal(new TimeOnly(12, 0), 12.OClock().PM());

    [Fact]
    public void MidDay() => Assert.Equal(12, Mid.Day);

    [Fact]
    public void MidNight() => Assert.Equal(00, Mid.Night);

    [Fact]
    public void ToMidNight() => Assert.Equal(new TimeOnly(23, 40), 20.To(Mid.Night));
}