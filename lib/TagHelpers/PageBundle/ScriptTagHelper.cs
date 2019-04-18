﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;

namespace InnateGlory.TagHelpers
{
    [HtmlTargetElement("script", Attributes = key_auto)]
    [HtmlTargetElement("script", Attributes = "inline")]
    public class ScriptTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper, IPageBundleTagHelpers
    {
        public ScriptTagHelper(IHostingEnvironment hostingEnvironment, TagHelperMemoryCacheProvider cacheProvider, IFileVersionProvider fileVersionProvider, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory)
            : base(hostingEnvironment, cacheProvider, fileVersionProvider, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
            this.fileVersionProvider = fileVersionProvider;
        }

        bool IPageBundleTagHelpers.TryResolveUrl(string url, out string resolvedUrl) => base.TryResolveUrl(url, out resolvedUrl);
        IFileVersionProvider fileVersionProvider;
        IFileVersionProvider IPageBundleTagHelpers.FileVersionProvider => fileVersionProvider;
        IHostingEnvironment IPageBundleTagHelpers.HostingEnvironment => base.HostingEnvironment;
        IMemoryCache IPageBundleTagHelpers.Cache => base.Cache;

        internal const string ext = "js";
        internal const string key_auto = "src-auto";
        internal const string key_src = "src";

        string IPageBundleTagHelpers.Key_Auto => key_auto;
        string IPageBundleTagHelpers.Key_Src => key_src;
        string IPageBundleTagHelpers.Ext => ext;
        string IPageBundleTagHelpers.Inline_TagName => "script";
        string IPageBundleTagHelpers.Inline_Type => "text/javascript";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            PageBundleFileProvider map = this.ViewContext.HttpContext.RequestServices.GetService<PageBundleFileProvider>();
            if (map.Process(this, context, output))
                base.Process(context, output);
        }
    }
}