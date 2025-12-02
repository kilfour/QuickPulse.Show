namespace QuickPulse.Show.Tests;

public class BugHunt
{
    [Fact]
    public void GenericIssue()
    {
        var result =
            Please.AllowMe()
                .To<Type>(a => a.Use(a => a.Name))
                .IntroduceThis(typeof(int), false);
        Assert.Equal("Int32", result);
    }
}