using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;

namespace InnateGlory.TagHelpers
{
    [HtmlTargetElement("link", Attributes = key_auto)]
    [HtmlTargetElement("link", Attributes = "inline")]
    public class LinkTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.LinkTagHelper, IPageBundleTagHelpers
    {
        public LinkTagHelper(IHostingEnvironment hostingEnvironment, TagHelperMemoryCacheProvider cacheProvider, IFileVersionProvider fileVersionProvider, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory)
            : base(hostingEnvironment, cacheProvider, fileVersionProvider, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
            this.fileVersionProvider = fileVersionProvider;
        }

        bool IPageBundleTagHelpers.TryResolveUrl(string url, out string resolvedUrl) => base.TryResolveUrl(url, out resolvedUrl);
        IFileVersionProvider fileVersionProvider;
        IFileVersionProvider IPageBundleTagHelpers.FileVersionProvider { get; }
        IHostingEnvironment IPageBundleTagHelpers.HostingEnvironment => base.HostingEnvironment;
        IMemoryCache IPageBundleTagHelpers.Cache => base.Cache;
        
        internal const string ext = "css";
        internal const string key_auto = "href-auto";
        internal const string key_src = "href";

        string IPageBundleTagHelpers.Key_Auto => key_auto;
        string IPageBundleTagHelpers.Key_Src => key_src;
        string IPageBundleTagHelpers.Ext => ext;
        string IPageBundleTagHelpers.Inline_TagName => "style";
        string IPageBundleTagHelpers.Inline_Type => "text/css";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            PageBundleFileProvider map = this.ViewContext.HttpContext.RequestServices.GetService<PageBundleFileProvider>();
            if (map.Process(this, context, output))
                base.Process(context, output);
        }
    }

}