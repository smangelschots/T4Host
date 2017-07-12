# T4Host
Start

T4 host lets you render T4 templates using C#

## Installation

https://www.nuget.org/packages/T4Host/  

## Features

- Render T4 templates in C#

## Docs & Community

* [Website and Documentation](http://devthings.net/)

## Quick Start


Create a unit test project
Add nuget package
```bash  
PM> Install-Package T4Host 
```

Create a t4 template HelloWorld.tt save it in Templates folder
In the properties of the tt file remove the text if there in the Custom Tool property
Copy this code 
```bash  
<#@ template language="C#" debug="true" #>

<#@ output extension=".html" #>
<#@ parameter name="parameter1" type="System.String" #>

<p> Hello world this is <#= this.parameter1 #> </p>
```

Go to the unitTest and copy this code
```bash  
var template = new TemplateManager([replace with path to template]\HelloWorld.tt));
template.SetParameter("parameter1", "T4Host");
var result = template.RenderTemplateToFile(@"C:\Temp\hello.html",true);
```

if you want to add assemblies use
```bash  
  template.AssemblyReferences.AddRange(AssembliesFileList);
  or
  template.RenderTemplateToFile(@"C:\Temp\hello.html",true,AssembliesFileList);
```

if you get the error
"There was a problem getting an AppDomain to run the transformation from the host"
Look in the C:\Windows\Microsoft.NET\assembly\GAC_MSIL what version is in there for 
  Microsoft.VisualStudio.TextTemplating.*.0 
  and
  Microsoft.VisualStudio.TextTemplating.Interfaces.*.0
  
  or add them to the GAC don't know wy they need to be in the GAC....

Have fun T4'ing


If you would like to add your output files to your Visual studio project, take al look at https://github.com/smangelschots/ProjectVs



