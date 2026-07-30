## 0....

### Doing

### Todo

#### Suggested for 0.1.18

* Document the existing customization API:
  * Ignoring properties and fields
  * Custom type formatters
  * Prefixes and class names
  * Inlining
  * Custom cycle formatting
* Escape `char` values consistently with strings, including quotes, backslashes, and control characters.
* Add built-in scalar formatting for `TimeSpan` and `DateTimeOffset`.
* Make reflection safer:
  * Skip indexer properties
  * Render `<unavailable>` when a property getter throws

#### Later

* Support generic-only and read-only dictionaries, including `IReadOnlyDictionary<,>` and concurrent dictionaries.
* Finish and test `ToPostProcess<T>`, or remove it from the public API.
* Add configurable maximum depth, collection length, and string length.

### To Test

### Done/Ready for Changelog after review
