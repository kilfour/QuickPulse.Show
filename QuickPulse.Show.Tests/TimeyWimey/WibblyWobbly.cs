using QuickPulse.Show.TimeyWimey;

namespace QuickPulse.Show.Tests.TimeyWimey;

public class WibblyWobbly
{
    [Fact]
    public void HumanDate_To_DateTime()
    {
        Assert.Equal(new DateTime(2025, 1, 20), 20.January(2025));
    }

    // [Fact]
    // public void HumanDate_With_Hours()
    // {
    //     Assert.Equal(new DateTime(2025, 1, 20, 10, 0, 0), 20.January(2025, 10.Hours()));
    // }
}