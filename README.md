# NppPlugin .NET package for VS2015 and beyond...

This is a fork of UFO's plugin package updated for VS2015

[![Build status](https://ci.appveyor.com/api/projects/status/5f0ui9y2ujugh6wt/branch/master?svg=true)](https://ci.appveyor.com/project/kbilsted/notepadpluspluspluginpack-net/branch/master)
[![License](http://img.shields.io/badge/License-Apache_2-red.svg?style=flat)](http://www.apache.org/licenses/LICENSE-2.0)
[![Stats](https://img.shields.io/badge/Code_lines-5,0_K-ff69b4.svg)]()
[![Stats](https://img.shields.io/badge/Doc_lines-3,1_K-ff69b4.svg)]()


## Getting started
  1. Download a [release](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/releases/) and unzip it
  2. Place the visual studio project template (the `Visual Studio Project Template C#\NppPlugin.zip`) in the visual studio path (typically `"My Documents\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\"` but see [instructions](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/blob/master/Visual%20Studio%20Project%20Template%20C%23/HOW-TO-INSTALL-ME.txt) inside the release)
  3. Ensure you have installed **Visual C++** from the visual studio installer otherwise your project wont build
  4. Create a new project inside Visual studio using `file -> new -> project -> visual C# -> Notepad++ project`
  5. Build (building will copy the dll to the `Notepad++/plugins` folder)
  6. Start Notepad++ and activate your plugin from the plugins menu


## Overall plugin architecture

Plugins can interact with Notepad++ or the underlying Scintilla engine. The plugin pack provides two classes to make this interaction easier. This is `NotepadPlusPlusGateway` and `ScintillaGateWay` which are thin layers making interaction more pleasant (and testable!). 

If you are interested in low-level access you can use the `Win32` api also included in the plugin pack. 

The architecture of the plugin is.


                   +-----------+ +-----------+
                   | scintilla | | Notepad++ |
                   +-----------+ +-----------+
                        ^             ^
                        |             |
               +--------+--------+----+------------+                   
               |                 |                 |
     +------------------+ +----------------+ +-----------+ 
     | scintillaGateway | | NotepadGateway | | Win32     |
     +------------------+ +----------------+ +-----------+ 
          ^                     ^                ^        
          |                     |                |        
          +-----------------+---+----------------+                   
                            |              
                       +-----------+ 
                       | plugin    |
                       +-----------+ 

## Content information
This package contains two folders:

  1. Templates:
    Simple templates for very fast and even easier building of .NET plugins for Notepad++.
    Setting up a plugin for N++ has never ever been as easy as with this package.
    Please see the containing txt files for further installation information.

  2. Demo:
    An example .NET plugin for Notepad++, build upon the template above.
    It demonstrates the same functionality as the original demo plugin by Don Ho:
    http://download.tuxfamily.org/nppplugins/NppPluginDemo/NppPluginDemo.zip

    
    
## Credits

  * For the main work on the plugin package
    * A Guy called "Ufo" - merging in v0.5 http://sourceforge.net/projects/sourcecookifier/files/other%20plugins/NppPlugin.NET.v0.5.zip/download and v0.7 https://bitbucket.org/uph0/npppluginnet 
  * The DllExport technique:
    * Dark Daskin: http://www.codeproject.com/KB/dotnet/DllExporter.aspx
    * Robert Giesecke: http://sites.google.com/site/robertgiesecke/Home/uploads/csharpprojecttemplateforunmanagedexports https://www.nuget.org/packages/UnmanagedExports


## Requirements:
  * works with .NET Runtime 3.5 and above (can easily be reduced to .Net runtime 2.0 if needed)
  * UNICODE plugins only.

