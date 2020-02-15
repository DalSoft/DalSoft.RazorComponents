using DalSoft.RazorComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;

// ReSharper disable once CheckNamespace
namespace RazorPages.WebPack.Pages._Components
{
    [HtmlTargetElement("hello-world", TagStructure = TagStructure.WithoutEndTag)]
    public class Demo : ComponentTagHelperBase<DalSoft.RazorComponents.Example.Pages.Components.Demo> { }

    [HtmlTargetElement("component-library-test", TagStructure = TagStructure.WithoutEndTag)]
    public class ComponentLibraryTest : ComponentTagHelperBase<DalSoft.RazorClassLibrary.Example.ComponentLibraryTest> { }
}