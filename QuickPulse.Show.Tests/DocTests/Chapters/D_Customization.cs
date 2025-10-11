using QuickPulse.Explains;

namespace QuickPulse.Show.Tests.DocTests.Chapters;

[DocFile]
[DocContent(
@"Currently, `Introduce.This` is not configurable. Future versions will support:

* Maximum depth / length controls.
* Custom format hooks per type.
* Fine grained control over object properties (Ignore, Customize, ...).

For now, it's designed to *Just Work* for 90% of debugging needs.

## Installation

QuickPulse.Show is available on NuGet:

```bash
Install-Package QuickPulse.Show
```

Or via the .NET CLI:

```bash
dotnet add package QuickPulse.Show
```")]
public class D_Customization
{
    [Fact]
    public void Placeholder() { /* Placeholder */ }
}