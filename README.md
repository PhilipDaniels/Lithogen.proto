Next Steps
==========
[ ] Get the RazorEngine working.
    BUT it may be that ServiceStack can do all this and more.
	Xipton
	  [ ] Project file contains ToolsVersion="12.0". This toolset may be unknown or missing, in which case you may be able to resolve this by installing the appropriate version of MSBuild, or the build may have been forced to a particular ToolsVersion for policy reasons. Treating the project as if it had ToolsVersion="4.0". For more information, please see http://go.microsoft.com/fwlink/?LinkId=291333.

[ ] Investigate ServiceStack for RazorMarkdown and asset processing.
[ ] If ServiceStack looks good, use it to run HighlighjJS, otherwise I will
    probably need to port it.

Information
===========
Principles
  Never change anything in the source tree (because it will be under source control)
Razor Engine
  NuGet Rankings
    RazorEngine - 227,240 - lots of things are based on this, looks a bit simplistic. https://github.com/Antaris/RazorEngine
	RazorMachine - 9615 - looks good, see the CodeProject article. LOOKS BEST of the standalone ones. https://github.com/jlamfers/RazorMachine
    RazorGenerator - seems to be a VS extension, avoid
    RazorTempaltes - 3899 - small, maybe interesting. https://github.com/volkovku/RazorTemplates
    WestWind.RazorHosting - 2116 - can inject a model! Does not support layouts or partials?
    SharpRazor - 110 - looks interesting, can inject a model into its Parse() method
  How to inject models?
Assets Engine
Markdown Support
  Graze - appears to do .md -> .html in one step, no use of Razor.
  Ocam - appears to do .md -> .html in one step, no use of Razor. Better source code than Graze?
Image Compression
  This should be a one-off task. We can bundle some tools to help out.
    PNG quantization to 256 colors.
    MetaData stripping.
    JPEQ compression.    
ConfigZilla integration
  A basic configuration that is used to create a class that is injected into the ViewBag and can
  be used to control the views. @if ViewBag.Config.EnableGoogleAnalytics
Template site
  HTML5 boilerplate, also gives you jQuery, Modernizr and Normalize.css and Google Analytics.
  Highlight.js for syntax highlighting of code.
  jQuery Colorbox for image popups, carousels and dialogs.
  Font Awesome for icon goodness!
  Live Reload (in Debug)
  Layout should follow ASP.Net MVC standards:
    \favicon.ico
    Views\Shared
	Views\_ViewStart.cshtml
	Views\Web.config (needed at compile time)
	\css
	\scripts
	\images
	\Models (for automatic model injection)

Questions
=========
[ ] How to serve the LiveReload and static sites during development.
[ ] How to do friendly URL generation?
[ ] How to build a really flexible View Engine - like Docpad with *.cshtml.md.foo extension processing?
    Should it be a design principle that ViewEngines read a file once to get a string, then pass it on
	to other stages before it is written (by the last "ViewWriter" stage)? How can the individual stages
	be configured? A map of extension to ViewTransformFunc?

Design Notes
============
LithogenVirtualFileSystem
  LowerCasingFileSystem
Tasks
  CleanUpOutput - remove DLLs, pdb and web.config


==========================================================================================
Binding Failure>>

The Appbase shown is the location of MSBuild.

1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: === Pre-bind state information ===
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: DisplayName = Lithogen.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018:  (Fully-specified)
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Appbase = file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319/
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Initial PrivatePath = NULL
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: Calling assembly : (Unknown).
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: ===
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: This bind starts in default load context.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Using application configuration file: C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe.Config
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Using host configuration file: 
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Using machine configuration file from C:\Windows\Microsoft.NET\Framework\v4.0.30319\config\machine.config.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Policy not being applied to reference at this time (private, custom, partial, or location-based assembly bind).
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Attempting download of new URL file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319/Lithogen.Core.DLL.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Attempting download of new URL file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319/Lithogen.Core/Lithogen.Core.DLL.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Attempting download of new URL file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319/Lithogen.Core.EXE.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: LOG: Attempting download of new URL file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319/Lithogen.Core/Lithogen.Core.EXE.
1>C:\Users\Phil\repos\Lithogen\Lithogen.TaskShim\Lithogen.TaskShim.targets(7,5): error MSB4018: 

==========================================================================================
Manual>>

Lithogen
========
Lithogen is a static site generator for .Net. A "static site" is a website that
contains only HTML, CSS and JavaScript. There is no back-end processor such
as ASP or CGI, and as a consequence the resulting website can be served by
any webserver - including Linux based ones such as Nginx - and is typically
small and fast compared to a website that needs a backend.

Lithogen builds on standard ASP.Net technologies and runs inside Visual Studio
- there is no command line tool to run, you just build the solution as you
would normally and the output folder will contain your static website.

Quick Start
===========
Clone this repo, start adding `.cshtml` or `.md` pages in the project and build!

Feature List
============
* Websites are based on the HTML5 boilerplate project, which gives you a great
starting point for building modern, mobile-friendly websites. HTML5 boilerplate
also gives you jQuery, Modernizr and Normalize.css and Google Analytics.

* Razor or Markdown syntax for web page authoring.

* Highlight.js for syntax highlighting of code.

* jQuery Colorbox for image popups, carousels and dialogs.

* Font Awesome for icon goodness!

* WebGrease for minification of JavaScript, CSS and images. Not to mention SASS
support!

* In the standard Debug build, Live Reload is used to give instant feedback on
your changes.

* Use MSBuild configurations (Debug, Release, etc.) in conjunction with
ConfigZilla to create different builds of your website. You can trivially
include or exclude features based on the current configuration.

* Extension points to allow you to hook into the build pipeline and extend it
with your own functionality.
