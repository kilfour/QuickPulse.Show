# QuickPulse.Show
> Please allow `this` to introduce oneself, hope you guess my type.

```csharp
Introduce.This(new List<Models.Person> { new("Alice", 26), new("Bob", 21) }, false);
    // => "[ { Name: \"Alice\", Age: 26 }, { Name: \"Bob\", Age: 21 } ]"
```
Erm, ... well, ... I guess we're done here, ...  

That's it really, ... One method.  

Oh and the optional `false` parameter renders the output on one line.  
So yeah there's that.

Or ... *would you like to know more ?*

## Purpose
**QuickPulse.Show** provides lightweight, opinionated, ~~pretty-printing~~ honest-printing for diagnostics,
debugging, and testing. It's not a general-purpose serializer, it's meant to give you a clean,
readable snapshot of values as they flow through your code.

## Output Style
The output follows a C#-inspired, developer-friendly style:

* **Objects** use `{ Prop: Value }` syntax
* **Strings** are quoted
* **Primitives** render as-is
* **Collections** render in square brackets: `[ ... ]`
* **Tuples** and anonymous types print with parentheses or braces respectively
* **Null** prints as `null`


```csharp
Introduce.This(123);                        // => "123"
Introduce.This("hi");                       // => "\"hi\""
Introduce.This(new[] { 1, 2 });             // => "[ 1, 2 ]"
Introduce.This((1, "a"));                   // => "(1, \"a\")"
Introduce.This(new { X = 1, Y = "Z" });     // => "{ X: 1, Y: \"Z\" }"
Introduce.This(null);                       // => "null"
```
## Supported Types
*Formatting is recursive, but avoids circular reference handling.*

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

## Customization
Currently, `Introduce.This` is not configurable. Future versions will support:

* Maximum depth / length controls.
* Custom format hooks per type.
* Fine grained control over object properties (Ignore, Customize, ...).

For now, it's designed to *Just Work* for 90% of debugging needs.

## Installation

QuickPulse.Show is available on NuGet:

```bash
Install-Package QuickPulse.Show
```

Or via the .NET CLI:

```bash
dotnet add package QuickPulse.Show
```
