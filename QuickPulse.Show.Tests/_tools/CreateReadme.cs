using QuickPulse.Explains.Deprecated;

namespace QuickPulse.Show.Tests._tools;

public class CreateReadme
{
    [Fact]
    public void FromDocAttributes()
    {
        new Document().ToFile("README.md", typeof(CreateReadme).Assembly);
    }
}