using QuickPulse.Explains;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.Deposition;


[Doc(Order = "1", Caption = "QuickPulse.Show", Content =
@"> Please allow `this` to introduce oneself, hope you guess my type.
")]
public class QuickPulseShow
{
    [Fact]
    [Doc(Order = "1-1", Caption = "", Content =
@"```csharp
Introduce.This(new List<Models.Person> { new(""Alice"", 26), new(""Bob"", 21) }, false);
    // => ""[ { Name: \""Alice\"", Age: 26 }, { Name: \""Bob\"", Age: 21 } ]""
```
")]
    public void InitialExample()
    {
        var str = Introduce.This(new List<Models.Person> { new("Alice", 26), new("Bob", 21) }, false);
        Assert.Equal("[ { Name: \"Alice\", Age: 26 }, { Name: \"Bob\", Age: 21 } ]", str);
    }
}