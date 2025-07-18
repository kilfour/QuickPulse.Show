using QuickPulse.Show.Tests._tools;


namespace QuickPulse.Show.Tests.Flat;

public class DictionaryTests : AbstractFlowTests
{
    [Fact]
    public void Pulse_IntStringDictionary() =>
        Assert.Equal("{ 1: \"2\", 3: \"4\" }", Pulse(new Dictionary<int, string> { { 1, "2" }, { 3, "4" } }));

    [Fact]
    public void Pulse_NestedDictionaries() =>
        Assert.Equal("{ 1: { 2: \"Quick\" }, 3: { 4: \"Pulse\" } }",
            Pulse(new Dictionary<int, Dictionary<int, string>>
            {
                { 1, new Dictionary<int, string> { { 2, "Quick" } } },
                { 3, new Dictionary<int, string> { { 4, "Pulse" } } }
            }));
}
