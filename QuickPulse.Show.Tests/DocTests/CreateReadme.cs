using QuickPulse.Explains;

namespace QuickPulse.Show.Tests.DocTests;

public class CreateReadme
{
    [Fact]
    public void FromDocAttributes()
    {
        Explain.This<CreateReadme>("README.md");
    }
}