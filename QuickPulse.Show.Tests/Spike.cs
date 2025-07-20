using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests;

public class Spike
{
    [Fact]
    public void Introduce_a_primitive()
    {
        var result = Please.AllowMe()
            .To<Models.Coach>(a => a.Ignore(a => a.Skills))
            .IntroduceThis(new Models.Coach("name", "email"), false);

        Assert.Equal("{ Name: \"name\", Email: \"email\" }", result);
    }
    [Fact]
    public void Introduce_part_of_the_coach()
    {
        var result = Please.AllowMe()
            .To<Models.Coach>(a => a.Ignore(a => a.Skills))
            .IntroduceThis(new Models.Coach("name", "email"), false);

        Assert.Equal("{ Name: \"name\", Email: \"email\" }", result);
    } // .To<Account>(a => a.Use(a => a.Balance.ToString()))
}

