using QuickPulse.Explains;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.Notes.G_SystemTypes;


[DocFile]
[DocContent(
@"Some known non-primitive types have their own special formatting in QuickPulse.Show.

Currently only `System.Type` is supported.
")]
public class SystemTypes
{
    [Fact]
    public void PrimitiveType()
    {
        var result =
            Please.AllowMe()
                .IntroduceThis(typeof(int));
        Assert.Equal("Int32", result);
    }

    [Fact]
    public void ReferenceType()
    {
        var result =
            Please.AllowMe()
                .IntroduceThis(typeof(Models.Coach));
        Assert.Equal("Coach", result);
    }

    [Fact]
    public void OverridingTheDefault()
    {
        var result =
            Please.AllowMe()
            .ToRegisterSystemType<Type>(a => "OVERRIDDEN")
                .IntroduceThis(typeof(int));
        Assert.Equal("OVERRIDDEN", result);
    }
}