using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;

namespace InnateGlory.TagHelpers
{
    internal interface IPageBundleTagHelpers
    {
        ViewContext ViewContext { get; }
        bool TryResolveUrl(string url, out string resolvedUrl);
        bool? AppendVersion { get; set; }
        IFileVersionProvider FileVersionProvider { get; }
        IHostingEnvironment HostingEnvironment { get; }
        IMemoryCache Cache { get; }

        string Key_Auto { get; }
        string Key_Src { get; }
        string Ext { get; }
        string Inline_TagName { get; }
        string Inline_Type { get; }
    }
}
