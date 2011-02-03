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

Installation
------------

I'm using Mono 2.8 for development, but really every environment which supports .NET 2.0 and higher should be enough in order to build this small project. Just hit (x)build against psi.sln

Help
----

If you need to talk to me, I'm available at andrius.bentkus@gmail.com or txdv @ irc.oftc.net, irc.freenode.org, irc.quakenet.org, irc.gimp.org.

Coding guidelines
----------------

Use the [mono coding guidlines](http://www.mono-project.com/Coding_Guidelines), just use new lines for parenthesis after name spaces, and class definitions and skip the space between the method name and the opening parenthesis.

Contributing
------------

If you need to use this library and you need to make some modifications feel free to contribute back.
Just fork on github, make a merge request for the modifications.

License
-------

This library is available under the [GPL3](http://www.gnu.org/licenses/gpl.html) license.


Authors
-------

* [Andrius Bentkus](mailto:andrius.bentkus@gmail.com)
