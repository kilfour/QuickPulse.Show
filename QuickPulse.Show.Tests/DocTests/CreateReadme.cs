using QuickPulse.Explains.Deprecated;

namespace QuickPulse.Show.Tests.DocTests;

public class CreateReadme
{
    [Fact]
    public void FromDocAttributes()
    {
        new Document().ToFile("README.md", typeof(CreateReadme).Assembly);
    }
}