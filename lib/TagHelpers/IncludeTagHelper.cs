using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace InnateGlory.TagHelpers
{
    [HtmlTargetElement("include")]
    public class IncludeTagHelper : TagHelper
    {

        IHtmlHelper html;

        [HtmlAttributeName("src")]
        public string src { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public IncludeTagHelper(IHtmlHelper htmlHelper)
        {
            html = htmlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            (html as IViewContextAware).Contextualize(ViewContext);

            //string src = context.AllAttributes["src"]?.Value?.ToString();
            //output.TagName = null;
            //if (ViewContext.TryResolveUrl(src, out string url))
            //{
            var content = html.Partial(src);
            output.Content.SetHtmlContent(content);
            //}
        }

    }
}