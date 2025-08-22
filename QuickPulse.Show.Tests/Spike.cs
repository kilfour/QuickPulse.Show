using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests;

public class Spike
{
    [Fact]
    public void Introduce_a_replaced_primitive()
    {
        var result = Please.AllowMe()
            .ToReplace<int>(a => "REPLACED")
            .To<Models.Coach>(a => a.Ignore(a => a.Skills))
            .IntroduceThis(42, false);
        Assert.Equal("REPLACED", result);
    }

    [Fact]
    public void Introduce_part_of_the_coach()
    {
        var result = Please.AllowMe()
            .To<Models.Coach>(a => a.Ignore(a => a.Skills))
            .IntroduceThis(new Models.Coach("name", "email"), false);

        Assert.Equal("{ Name: \"name\", Email: \"email\" }", result);
    }

    [Fact]
    public void Introduce_a_totally_different_coach()
    {
        var result = Please.AllowMe()
            .To<Models.Coach>(a => a.Use(a => "REPLACED"))
            .IntroduceThis(new Models.Coach("name", "email"), false);

        Assert.Equal("REPLACED", result);
    }

    [Fact]
    public void PulseToLogTest()
    {
        "a Test".PulseToLog("my.log");
    }
}

