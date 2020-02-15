using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace DalSoft.RazorComponents
{
    public abstract class ComponentTagHelperBase<TComponent> : TagHelper where TComponent : IComponent
    {
        private const string RenderModeName = "render-mode";
        private IDictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets or sets values for component parameters.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = "")]
        public IDictionary<string, object> Parameters
        {
            get
            {
                _parameters ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                return _parameters;
            }
            set => _parameters = value;
        }

        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var htmlHelper = ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper>();
            
            (htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

            var renderMode = RenderMode.Static;

            if (_parameters.ContainsKey(RenderModeName) && _parameters[RenderModeName] is RenderMode)
                renderMode = (RenderMode) _parameters[RenderModeName];
            else if (_parameters.ContainsKey(RenderModeName))
                throw new ArgumentException($"{RenderModeName} must be {RenderMode.Static}, {RenderMode.Server} or {RenderMode.ServerPrerendered}");

            var parametersWithoutRenderMode = _parameters.Where(x => x.Key != RenderModeName).ToDictionary(x => x.Key.Replace("-", string.Empty), x => x.Value);

            var result = await htmlHelper.RenderComponentAsync<TComponent>(renderMode, parametersWithoutRenderMode);
            
            output.TagName = null; // Reset the TagName. We don't want `component` to render.
            
            output.Content.SetHtmlContent(result);
        }
    }
}