using QuickPulse.Explains.Deprecated;
using QuickPulse.Explains.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.DocTests.Chapters;


[Doc(Order = "1", Caption = "QuickPulse.Show", Content =
@"> Please allow `this` to introduce oneself, hope you guess my type.
")]
public class A_Introduction
{
    [Fact]
    [Doc(Order = "1-1", Caption = "", Content =
@"```csharp
Introduce.This(new List<Models.Person> { new(""Alice"", 26), new(""Bob"", 21) }, false);
    // => ""[ { Name: \""Alice\"", Age: 26 }, { Name: \""Bob\"", Age: 21 } ]""
```
Erm, ... well, ... I guess we're done here, ...  

That's it really, ... One method.  

Oh and the optional `false` parameter renders the output on one line.  
So yeah there's that.

Or ... *would you like to know more ?*
")]
    public void InitialExample()
    {
        var str = Introduce.This(new List<Models.Person> { new("Alice", 26), new("Bob", 21) }, false);
        Assert.Equal("[ { Name: \"Alice\", Age: 26 }, { Name: \"Bob\", Age: 21 } ]", str);
    }

    [Fact]
    [Doc(Order = "1-2", Caption = "Purpose", Content =
@"**QuickPulse.Show** provides lightweight, opinionated, ~~pretty-printing~~ honest-printing for diagnostics,
debugging, and testing. It's not a general-purpose serializer, it's meant to give you a clean,
readable snapshot of values as they flow through your code.
")]
    public void PrettyPrinted()
    {
        var str = Introduce.This(new List<Models.Person> { new("Alice", 26), new("Bob", 21) });
        var reader = LinesReader.FromText(str);
        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        Name: \"Alice\",", reader.NextLine());
        Assert.Equal("        Age: 26", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        Name: \"Bob\",", reader.NextLine());
        Assert.Equal("        Age: 21", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }
}