using QuickPulse.Explains;

namespace QuickPulse.Show.Tests.Notes.G_SystemTypes;


[DocFile]
[DocContent(
@"Some known non-primitive types have their own special formatting in QuickPulse.Show.

Currently only `System.Type` is supported.
")]
public class SystemTypes
{
    [Fact]
    public void RunTest()
    {
        var result =
            Please.AllowMe()
                .To<Type>(a => a.Use(a => a.Name))
                .IntroduceThis(typeof(int));
        Assert.Equal("Int32", result);
    }
}