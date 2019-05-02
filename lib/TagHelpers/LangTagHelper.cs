using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory.TagHelpers
{
    // <lang></lang>
    // <div lang=""></div>
    [HtmlTargetElement("lang")]
    public class Lang1TagHelper : TagHelper
    {
        public Lang1TagHelper()
        {
        }

        [HtmlAttributeName("key")]
        public string Key { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Init(TagHelperContext context)
        {
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("*", Attributes = "lang")]
    public class Lang2TagHelper : TagHelper
    {
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
        }
    }
}
