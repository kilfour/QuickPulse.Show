using QuickPulse.Explains.Deprecated;

namespace QuickPulse.Show.Tests.DocTests;


[Doc(Order = "1-3", Caption = "Output Style", Content =
@"The output follows a C#-inspired, developer-friendly style:

* **Objects** use `{ Prop: Value }` syntax
* **Strings** are quoted
* **Primitives** render as-is
* **Collections** render in square brackets: `[ ... ]`
* **Tuples** and anonymous types print with parentheses or braces respectively
* **Null** prints as `null`
")]
public class OutputStyle
{
    [Fact]
    [Doc(Order = "1-3-1", Caption = "", Content =
@"
```csharp
Introduce.This(123);                        // => ""123""
Introduce.This(""hi"");                       // => ""\""hi\""""
Introduce.This(new[] { 1, 2 });             // => ""[ 1, 2 ]""
Introduce.This((1, ""a""));                   // => ""(1, \""a\"")""
Introduce.This(new { X = 1, Y = ""Z"" });     // => ""{ X: 1, Y: \""Z\"" }""
Introduce.This(null);                       // => ""null""
```")]
    public void Demonstrate()
    {
        Assert.Equal("123", Introduce.This(123, false));
        Assert.Equal("\"hi\"", Introduce.This("hi", false));
        Assert.Equal("[ 1, 2 ]", Introduce.This(new[] { 1, 2 }, false));
        Assert.Equal("( 1, \"a\" )", Introduce.This((1, "a"), false));
        Assert.Equal("{ X: 1, Y: \"Z\" }", Introduce.This(new { X = 1, Y = "Z" }, false));
        Assert.Equal("null", Introduce.This(null!, false));
    }
}