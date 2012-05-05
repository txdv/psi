Psi
===

Description
-----------

This Project aims to provide a parser which parses the log output of the goldsrc hlds engine (Counter-Strike, Half-Life, etc.).
It is written in C# and completely by hand instead of using a parser generator for these reasons:

* The actual *language* is easy to parse.
* Keeping the binary size as small as possible is one of the goals.
* Speed matters and I'm talking about the kind of speed where 1 extra second for 140MB of logs matters.
* Avoiding dependencies, no need for any kind of Antlr3.Runtime.dll and stuff like that for this rather simple functionality.

The code might seem little bit hacky, but it is efficient and well tested.

License
-------

The library is available under the [MIT license](http://en.wikipedia.org/wiki/MIT_License).
