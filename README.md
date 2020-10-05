
# DalSoft.RazorComponents

## Why

Since Blazor Server-Side has been shipped with ASP.NET Core 3.0, you have also been able to [use Razor Components in ASP.NET Razor Pages or View](https://ml-software.ch/posts/using-razor-components-in-razor-pages-or-mv-views).

You do it like this in your Razor Page (for a Razor Component named HelloWorld):

```html
<component type="typeof(HelloWorld)" render-mode="Static" param-message='$"Hello World {DateTime.Now}"' />
```
This package is a simple tag library that allows you to use friendly markup for your Component in a Razor Pages project, exactly how you would in a Blazor app.

```html
<hello-world message='$"Hello World {DateTime.Now}"' />
```

## How to use 

### Create a Razor Component

> First of all to use Razor Components, you need a ASP.NET Core 3.0 Razor Pages or MVC project.

Use an existing Razor pages project or create a new one using Visual Studio or the dotnet CLI:

```bash
> dotnet new razor
```

Add a Razor component using either Visual Studio or the dotnet CLI
```bash
> dotnet new razorcomponent --name MyComponent
```

Add your code to your Razor Component,  for example:
```razor
@code {

    [Parameter]
    public string Message { get; set; }
}

<h3>@Message</h3>
```

### Using DalSoft.RazorComponents with your Razor Component

Now you have a Razor Pages or MVC project - your ready to use DalSoft.RazorComponents with your Razor Component.

#### Install DalSoft.RazorComponents package via DotNet CLI

```bash
> dotnet add package DalSoft.RazorComponents
```

#### Create a Components mapping class

This is very simple class that maps our Razor component to our Tag Library using a base class. 

You can create this class anywhere in your Razor pages project.

```cs
namespace MyRazorPages.Pages._Components
{
    [HtmlTargetElement(tag:"hello-world", TagStructure = TagStructure.WithoutEndTag)]
    public class TheClassNameIsNotImportant : ComponentTagHelperBase<Components.MyComponent> { }
}
```
This is a simple class but lets take a moment to understand what is going on:

The components mapping class inherits from ComponentTagHelperBase  - the important thing to note is the generic parameter, this parameter is the  Razor component we created in the last step. 

How do you know what the generated Razor component namespace / class name is? 

Simple - the namespace is your project name followed by the directory structure your Razor component is in. The class name is the file name of your Razor component.

Lastly the HtmlTargetElement attribute is important and required, in this example we are saying we want the tagname to be `<hello-world>`, and that we don't require a end tag.

#### Add our project to the registered tag libraries

Now all we need to do is register our tag library. We do this by adding `@addTagHelper *, DalSoft.RazorComponents.Example` to _ViewImports.cshtml. 

It will look like this for our example:

> Note MyRazorPages is the **Assembly Name**
```razor
@using MyRazorPages
@namespace MyRazorPages.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, MyRazorPages
```
#### Profit

Now in your Razor pages you can use the friendlier tag name for your component that you specified in your components mapping class. 

It now looks like how you would use a Razor component in a  Blazor app.
```razor 
<hello-world message='$"Hello World {DateTime.Now}"' />
```

#### Child Content / Components

Child components are supported and give you the opportunity to create terser razor for thing like layouts. For example:

```razor
<grid>
   <row>
      <col align="right">Hello from col 1</col>
      <col algn="left">Hello from col 2</col>
    </row>
</grid>
```

See `<ChildDemo>` in this [example](https://github.com/DalSoft/DalSoft.RazorComponents/blob/master/DalSoft.RazorComponents.Example/Pages/Index.cshtml) for how it works.

#### Bonus

This package works perfectly with [Razor components class libraries](https://docs.microsoft.com/en-us/aspnet/core/blazor/class-libraries?view=aspnetcore-3.1&tabs=visual-studio); making it trivial to create reusable components for Razor pages using friendly markup.

#### Limitations
Thanks to this [PR](https://github.com/DalSoft/DalSoft.RazorComponents/pull/3) by @pwhe23 this is no longer a limitation.

 ~~You can't nest components in Razor pages (you can in Blazor) this is a Razor pages limitation.~~

 ~~This won't error but won't work as you expect in Razor pages:~~

```html
<!-- <mycontainer>
    <hello-world message='$"Hello World {DateTime.Now}"' />
</mycontainer> -->
 ```
 

