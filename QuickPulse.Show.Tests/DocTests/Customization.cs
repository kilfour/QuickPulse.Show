using QuickPulse.Explains;

namespace QuickPulse.Show.Tests.DocTests;


[Doc(Order = "1-5", Caption = "Customization", Content =
@"Currently, `Introduce.This` is not configurable. Future versions will support:

* Maximum depth / length controls.
* Custom format hooks per type.
* Fine grained control over object properties (Ignore, Customize, ...).

For now, it's designed to *Just Work* for 90% of debugging needs.")]
public class Customization
{
    [Fact]
    [Doc(Order = "1-5-1", Caption = "", Content =
@"")]
    public void Placeholder()
    {
    }
}