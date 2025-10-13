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
    public void Introduce_a_coding_coach()
    {
        var result = Please.AllowMe()
            .To<Models.Coach>(a => a.Use(a => $"new Models.Coach(\"{a.Name}\", \"{a.Email}\")"))
            .IntroduceThis(new Models.Coach("name", "email"), false);

        Assert.Equal("new Models.Coach(\"name\", \"email\")", result);
    }

    [Fact]
    public void Introduce_a_named_person()
    {
        var result = Please.AllowMe()
            .ToAddSomeClass()
            .IntroduceThis(new Models.Person("Alice", 30), false);

        Assert.Equal("Person { Name: \"Alice\", Age: 30 }", result);
    }

    [Fact]
    public void Introduce_a_list_of_named_persons()
    {
        var result = Please.AllowMe()
            .ToAddSomeClass()
            .IntroduceThis(new List<Models.Person>([new Models.Person("a", 1), new Models.Person("b", 2)]), false);
        Assert.Equal("[ Person { Name: \"a\", Age: 1 }, Person { Name: \"b\", Age: 2 } ]", result);
    }
}

