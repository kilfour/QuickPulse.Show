using QuickPulse.Instruments;
using QuickPulse.Show.TimeyWimey;

namespace QuickPulse.Show.Tests.TimeyWimey;

public class WibblyWobbly
{
    [Fact]
    public void HumanDate_To_DateOnly()
    {
        Assert.Equal(new DateOnly(2025, 1, 20), 20.January(2025));
    }

    [Fact]
    public void HumanDate_To_DateTime()
    {
        Assert.Equal(new DateTime(2025, 1, 20, 10, 0, 0), 20.January(2025).At(10.OClock()));
    }

    [Fact]
    public void HumanTime_OClock()
    {
        Assert.Equal(new TimeOnly(5, 0, 0), 5.OClock());

        Assert.Throws<ComputerSaysNo>(() => 0.OClock());
        Assert.Throws<ComputerSaysNo>(() => 13.OClock());

        Assert.Throws<ComputerSaysNo>(() => (-2).OClock());
        Assert.Throws<ComputerSaysNo>(() => 100.OClock());
    }

    [Fact]
    public void HumanTime_To()
    {
        Assert.Equal(new TimeOnly(11, 50, 0), 10.To(12));

        Assert.Throws<ComputerSaysNo>(() => 10.To(17));
        Assert.Throws<ComputerSaysNo>(() => 10.To(0));

        Assert.Throws<ComputerSaysNo>(() => 0.To(2));
        Assert.Throws<ComputerSaysNo>(() => 60.To(2));

        Assert.Throws<ComputerSaysNo>(() => 1.To(-2));
        Assert.Throws<ComputerSaysNo>(() => 59.To(100));

        Assert.Throws<ComputerSaysNo>(() => (-2).To(2));
        Assert.Throws<ComputerSaysNo>(() => 100.To(2));
    }

    [Fact]
    public void HumanTime_Past()
    {
        Assert.Equal(new TimeOnly(11, 20, 0), 20.Past(11));

        Assert.Throws<ComputerSaysNo>(() => 10.Past(13));
        Assert.Throws<ComputerSaysNo>(() => 10.Past(-2));

        Assert.Throws<ComputerSaysNo>(() => (-2).Past(2));
        Assert.Throws<ComputerSaysNo>(() => 100.Past(2));
    }

    [Fact]
    public void PM()
    {
        Assert.Equal(new TimeOnly(17, 0), 5.OClock().PM());
    }
}