using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InnateGlory.TagHelpers
{
    [HtmlTargetElement("*", Attributes = asp_visible)]
    public class VisibleTagHelper : TagHelper
    {
        const string asp_visible = "asp-visible";

        [HtmlAttributeName(asp_visible)]
        public bool Visible { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Visible)
                output.SuppressOutput();
            base.Process(context, output);
        }
    }
}