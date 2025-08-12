using QuickPulse.Explains.Deprecated;

namespace QuickPulse.Show.Tests.DocTests.Chapters;


[Doc(Order = "1-4", Caption = "Supported Types", Content =
@"*Formatting is recursive, but avoids circular reference handling.*

The formatter supports:

* Primitive types (`int`, `bool`, etc.)
* Strings
* `null`
* Arrays and any `IEnumerable`
* Tuples (up to 7 elements)
* Records and anonymous types
* User-defined classes and structs, best effort. Which means I tried to break it:  
  *From PrettyCollectionTests.cs:*
    * `Introduce_IntList`
    * `Introduce_NestedList_IndentedCorrectly`
    * `Introduce_ObjectList`
    * `Introduce_ListContainingEmptyObject`
    * `Introduce_ListOfNulls`
    * `Introduce_ListOfTuplesWithNulls`
    * `Introduce_ListWithEmptyDictionary`
    * `Introduce_ListOfPolymorphicObjects`
    * `Introduce_ListOfNestedPolymorphicObjects`
    * `Introduce_ShapeCollisionInPolymorphicList`

I'm sure I missed something so I'm gonna keep trying.
> And if you can break it, ... create an issue, ... make my day.
")]
public class C_SupportedTypes
{
    [Fact]
    [Doc(Order = "1-4-1", Caption = "", Content =
@"")]
    public void Placeholder()
    {
    }
}