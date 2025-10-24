using QuickPulse.Explains.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests;

public class Spike
{
    [Fact]
    public void Introduce_a_replaced_primitive()
    {
        var result = Please.AllowMe()
            .ToReplace<int>(a => "REPLACED")
            .IntroduceThis(42, false);
        Assert.Equal("REPLACED", result);
    }

    [Fact]
    public void Introduce_replace_all_ints()
    {
        var result = Please.AllowMe()
            .ToReplaceAll(t => t == typeof(int), a => "REPLACED")
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

    [Fact]
    public void Introduce_a_cyclic_format()
    {
        var node = new Models.Node("root");
        node.Next = node;
        var result = Please.AllowMe()
            .ToSelfReference<Models.Node>(a => $"<cycle: {typeof(Models.Node).Name} {{ Name = \"{a.Name}\" }}>")
            .IntroduceThis(node, false);
        Assert.Equal("{ Name: \"root\", Next: <cycle: Node { Name = \"root\" }> }", result);
    }

    [Fact]
    public void Introduce_inlining()
    {
        var result = Please.AllowMe()
            .ToInline<HashSet<string>>()
            .IntroduceThis(new Models.Coach("name", "email"));
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    Name: \"name\",", reader.NextLine());
        Assert.Equal("    Email: \"email\",", reader.NextLine());
        Assert.Equal("    Skills: [ ]", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_inlining_object()
    {
        var result = Please.AllowMe()
            .ToInline<Models.Coach>()
            .IntroduceThis(new Models.Coach("name", "email"));
        Assert.Equal("{ Name: \"name\", Email: \"email\", Skills: [ ] }", result);
    }

    [Fact]
    public void Introduce_inlining_object_in_list()
    {
        var result = Please.AllowMe()
            .ToInline<Models.Person>()
            .IntroduceThis(new List<Models.Person> { new("1", 1) });
        var reader = LinesReader.FromText(result);
        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    { Name: \"1\", Age: 1 }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}