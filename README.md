# Treemap

These libraries originally came from [Microsoft Research Downloads](http://research.microsoft.com/research/downloads/), and they were part of the "Data Visualization Components", which were free for non-commercial use and available under the Microsoft Research Shared Source License. Unfortunately, the libraries and code were last updated by Microsoft in 2006, and they targeted .NET 1.1, which was 32-bit only.

This repo updates the code to:
* Use modern SDK-style projects
* Use the "Any CPU" platform target
* Target .NET Framework 4.5 and .NET Core 3.1
* Produce NuGet packages

## Microsoft Notes

The Microsoft Research Community Technologies team has developed two .NET components that render treemaps.  The TreemapGenerator is a drawing engine without its own user interface. It takes a set of hierarchical data, generates a treemap from the data, and draws it onto a bitmap or Graphics object provided by the caller. It can be used in a variety of environments, including Web applications that generate images on the server for downloading to client browsers.

The TreemapControl wraps the TreemapGenerator into a Windows Forms control.  It can be added to the Visual Studio toolbox and dropped into any Windows Forms application.

## What Is A Treemap?
A treemap is a rendering of hierarchical data as a set of nested boxes, where each box corresponds to one data element.  The nesting indicates the hierarchy, the box sizes are proportional to some attribute on each element, and the box colors are based on another attribute.

## More Info
* See Wikipedia's [Treemapping](https://en.wikipedia.org/wiki/Treemapping) article for pictures and more information.
* See Microsoft's [Treemap.chm](src/Treemap.chm) Windows Help file for API-level details on using TreemapGenerator and TreemapControl.