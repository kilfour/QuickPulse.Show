using QuickPulse.Explains.Text;
using QuickPulse.Show.Tests._tools;

namespace QuickPulse.Show.Tests.PrettyPrinted;

public class PrettyCollectionTests
{
    [Fact]
    public void Introduce_IntList()
    {
        var result = Introduce.This(new List<int>([1, 2, 3]));
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    1,", reader.NextLine());
        Assert.Equal("    2,", reader.NextLine());
        Assert.Equal("    3", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_EmptyList()
    {
        var result = Introduce.This(new List<int>([]));
        var reader = LinesReader.FromText(result);
        Assert.Equal("[ ]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_NestedList_IndentedCorrectly()
    {
        var result = Introduce.This(new List<List<int>> { new() { 1, 2 }, new() { 3 } });
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    [", reader.NextLine());
        Assert.Equal("        1,", reader.NextLine());
        Assert.Equal("        2", reader.NextLine());
        Assert.Equal("    ],", reader.NextLine());
        Assert.Equal("    [", reader.NextLine());
        Assert.Equal("        3", reader.NextLine());
        Assert.Equal("    ]", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ObjectList()
    {
        var result = Introduce.This(new List<Models.Person>([new Models.Person("a", 1), new Models.Person("b", 2)]));
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        Name: \"a\",", reader.NextLine());
        Assert.Equal("        Age: 1", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        Name: \"b\",", reader.NextLine());
        Assert.Equal("        Age: 2", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListContainingEmptyObject()
    {
        var result = Introduce.This(new List<object> { new { }, new { X = 1 } });
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        X: 1", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListOfNulls()
    {
        var result = Introduce.This(new List<object?> { null, null });
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    null,", reader.NextLine());
        Assert.Equal("    null", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListOfTuplesWithNulls()
    {
        var result = Introduce.This(new List<(string?, int?)> { (null, 1), ("x", null) });

        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    (", reader.NextLine());
        Assert.Equal("        null,", reader.NextLine());
        Assert.Equal("        1", reader.NextLine());
        Assert.Equal("    ),", reader.NextLine());
        Assert.Equal("    (", reader.NextLine());
        Assert.Equal("        \"x\",", reader.NextLine());
        Assert.Equal("        null", reader.NextLine());
        Assert.Equal("    )", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListWithEmptyDictionary()
    {
        var result = Introduce.This(new List<Dictionary<string, string>> { new(), new() });
        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListOfPolymorphicObjects()
    {
        var result = Introduce.This(new List<object?>
    {
        42,
        "hello",
        null,
        new { A = 1, B = 2 },
        new List<string> { "x", "y" },
        new Dictionary<string, int> { ["foo"] = 123 },
    });

        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    42,", reader.NextLine());
        Assert.Equal("    \"hello\",", reader.NextLine());
        Assert.Equal("    null,", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        A: 1,", reader.NextLine());
        Assert.Equal("        B: 2", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());
        Assert.Equal("    [", reader.NextLine());
        Assert.Equal("        \"x\",", reader.NextLine());
        Assert.Equal("        \"y\"", reader.NextLine());
        Assert.Equal("    ],", reader.NextLine());
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        \"foo\": 123", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    [Fact]
    public void Introduce_ListOfNestedPolymorphicObjects()
    {
        var result = Introduce.This(new List<object?>
    {
        new List<object>
        {
            "outer",
            new { A = 1 },
            new List<object>
            {
                "inner",
                new Dictionary<string, object> { ["deep"] = new { B = 2 } }
            }
        }
    });

        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());
        Assert.Equal("    [", reader.NextLine());
        Assert.Equal("        \"outer\",", reader.NextLine());
        Assert.Equal("        {", reader.NextLine());
        Assert.Equal("            A: 1", reader.NextLine());
        Assert.Equal("        },", reader.NextLine());
        Assert.Equal("        [", reader.NextLine());
        Assert.Equal("            \"inner\",", reader.NextLine());
        Assert.Equal("            {", reader.NextLine());
        Assert.Equal("                \"deep\": {", reader.NextLine());
        Assert.Equal("                    B: 2", reader.NextLine());
        Assert.Equal("                }", reader.NextLine());
        Assert.Equal("            }", reader.NextLine());
        Assert.Equal("        ]", reader.NextLine());
        Assert.Equal("    ]", reader.NextLine());
        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    class NamedObject
    {
        public int X { get; set; }
        public string Y { get; set; } = "";
    }

    [Fact]
    public void Introduce_ShapeCollisionInPolymorphicList()
    {
        var result = Introduce.This(new List<object>
            {
                new NamedObject { X = 1, Y = "one" },       // class with props
                new { X = 1, Y = "one" },                   // anonymous object
                (X: 1, Y: "one"),                           // tuple
                new Dictionary<string, object>              // dictionary with same keys
                {
                    ["X"] = 1,
                    ["Y"] = "one"
                }
            });

        var reader = LinesReader.FromText(result);

        Assert.Equal("[", reader.NextLine());

        // Named object
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        X: 1,", reader.NextLine());
        Assert.Equal("        Y: \"one\"", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());

        // Anonymous object
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        X: 1,", reader.NextLine());
        Assert.Equal("        Y: \"one\"", reader.NextLine());
        Assert.Equal("    },", reader.NextLine());

        // Tuple
        Assert.Equal("    (", reader.NextLine());
        Assert.Equal("        1,", reader.NextLine()); // or X: 1, if named
        Assert.Equal("        \"one\"", reader.NextLine());
        Assert.Equal("    ),", reader.NextLine());

        // Dictionary
        Assert.Equal("    {", reader.NextLine());
        Assert.Equal("        \"X\": 1,", reader.NextLine());
        Assert.Equal("        \"Y\": \"one\"", reader.NextLine());
        Assert.Equal("    }", reader.NextLine());

        Assert.Equal("]", reader.NextLine());
        Assert.True(reader.EndOfContent());
    }

    public class SelfReferenceList
    {
        public List<SelfReferenceList> List = [];
    }

    [Fact]
    public void Introduce_Cycle()
    {
        var thing = new SelfReferenceList();
        thing.List.Add(thing);
        var result = Introduce.This(thing, true);
        var reader = LinesReader.FromText(result);
        Assert.Equal("{", reader.NextLine());
        Assert.Equal("    List: [", reader.NextLine());
        Assert.Equal("        <cycle>", reader.NextLine());
        Assert.Equal("    ]", reader.NextLine());
        Assert.Equal("}", reader.NextLine());
    }
}
