using QuickExplainIt;

namespace QuickPulse.Show.Tests._tools;

public class CreateReadme
{
    [Fact(Skip = "Not Yet")]
    public void FromDocAttributes()
    {
        new Document().ToFile("README.md", typeof(CreateReadme).Assembly);
    }
}