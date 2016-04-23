# NppPlugin .NET package for VS2015

## Fork information
This is a fork of UFO's plugin package http://sourceforge.net/projects/sourcecookifier/files/other%20plugins/NppPlugin.NET.v0.5.zip/download  which seems abandoned (and only supports up to visual studio 2010).



## Content information
This package contains two folders:

1) Visual Studio Project Template C#:
    A VS template for very fast and even easier building of .NET plugins for Notepad++.
    Setting up a plugin for N++ has never ever been as easy as with this package.
    Please See its containing text file for further information.

2) Demo Plugin:
    An example .NET plugin for Notepad++, build upon the template above.
    It demonstrates the same functionality as the original demo plugin by Don HO:
    http://notepad-plus.sourceforge.net/commun/pluginDemoTemplate/NppPluginTemplate.zip
    I don't know if I've added new bugs, but I've corrected some small mistakes which
    are in the original demo. I've also added example code for registering icons for
    the tab of a dockable dialog and for Notepad++'s tool bar (and how to toggle its
    state).


## Requirements:

  * works with .NET Runtime 2.0 and above
  * UNICODE only! ANSI is doable, but not supported so far

please feel free to contribute!