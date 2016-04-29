# NppPlugin .NET package for VS2015 and beyond...

This is a fork of UFO's plugin package updated for VS2015

## Getting started
  1. Download a [release](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/releases/)
  2. Place the visual studio project template in the visual studio path (see [instructions](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/blob/master/Visual%20Studio%20Project%20Template%20C%23/HOW-TO-INSTALL-ME.txt) inside the release)
  3. Create a new Notepad++ project inside Visual studio
  4. Build and copy dll to the Notepad++/plugins folder
  5. DONE!

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
For the main work on the plugin package
  * UFO
    * v0.5 http://sourceforge.net/projects/sourcecookifier/files/other%20plugins/NppPlugin.NET.v0.5.zip/download 
    * and v0.7 https://bitbucket.org/uph0/npppluginnet 

All credits for the used DllExport technique to following guys:
  * Dark Daskin: http://www.codeproject.com/KB/dotnet/DllExporter.aspx
  * Robert Giesecke: http://sites.google.com/site/robertgiesecke/Home/uploads/csharpprojecttemplateforunmanagedexports https://www.nuget.org/packages/UnmanagedExports


## Requirements:
  * works with .NET Runtime 2.0 and above
  * UNICODE only! ANSI is doable, but not supported so far

