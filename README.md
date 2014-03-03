PortableIoC
===========

This is a GitHub hosted fork of PortableIoC located at
[Codeplex](http://portableioc.codeplex.com) created by Jeremy Likness.

PortableIoC is a C#/.NET portable class library that provides a basic,
thread-safe IoC container.

About this fork
--------------

This fork is intended to preserve all original functions, plus

* comes as a single .cs file - only include `PortableIoC.cs` in your project, no
  need for adding a dll or nuget dependency
* Unit tests were ported to NUnit (instead of Microsoft.VisualStudio.Testing) to
  allow to open and execute with Xamarin Studio / MonoDevelop

Features
--------

* Dependency resolution
* Lifetime management (shared and non-shared copies)
* Supports both constructor and property injection
* Full control over registration - delete registrations and destroy shared copies
  as needed
* Multiple resolution support through a simple label
* Thread-safe

Documentation
-------------

To create an instance of the master container simply new it: 

    IPortableIoC ioc = new PortableIoc(); 

The container will automatically register itself, so this is possible: 

    IPortableIoC anotherIoCReference = ioc.Resolve<IPortableIoC>(); 

To register an instance of IFoo that is implemented as Foo:

    ioc.Register<IFoo>(ioc => new Foo()); 

To register a specific instance of IFoo in a container called "container2" that
is implemented as FooExtended:

    ioc.Register<IFoo>(ioc => new FooExtended(), "Container2"); 

To register an instance of IBar that depends on IFoo: 

    ioc.Register<IBar>(ioc => new Bar(ioc.Resolve<IFoo>()); 

If you are using property injection: 

    ioc.Register<IBar>(ioc => new Bar { Foo = ioc.Resolve<IFoo>() }); 

To resolve bar: 

    IBar bar = ioc.Resolve<IBar>(); 

To resolve bar from a named container: 

    IBar bar = ioc.Resolve<IBar>("Container2"); 

To resolve a non-shared copy of bar: 

    IBar bar = ioc.Resolve<IBar>(true); 


License
-------

MIT. See LICENSE file for detailed license text.
