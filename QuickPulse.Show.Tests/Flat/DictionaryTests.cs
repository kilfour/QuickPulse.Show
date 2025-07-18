using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.Flat;

public class DictionaryTests
{
    [Fact]
    public void Introduce_IntStringDictionary() =>
        Assert.Equal("{ 1: \"2\", 3: \"4\" }", Introduce.This(new Dictionary<int, string> { { 1, "2" }, { 3, "4" } }, false));

    [Fact]
    public void Introduce_NestedDictionaries() =>
        Assert.Equal("{ 1: { 2: \"Quick\" }, 3: { 4: \"Pulse\" } }",
           Introduce.This(new Dictionary<int, Dictionary<int, string>>
            {
                { 1, new Dictionary<int, string> { { 2, "Quick" } } },
                { 3, new Dictionary<int, string> { { 4, "Pulse" } } }
            }, false));
}
