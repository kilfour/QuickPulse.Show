using QuickPulse.Explains;

namespace QuickPulse.Show.Tests._tools;

public class CreateReadme
{
    [Fact(Skip = " need package update")]
    public void FromDocAttributes()
    {
        new Document().ToFile("README.md", typeof(CreateReadme).Assembly);
    }
}