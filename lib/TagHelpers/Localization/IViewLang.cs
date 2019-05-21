using Microsoft.AspNetCore.Html;
using LCID = System.Int32;

namespace InnateGlory
{
    public interface IViewLang
    {
        PlatformId PlatformId { get; set; }
        string ResPath { get; set; }
        IHtmlContent this[string key, LCID? lcid = null] { get; }
        IHtmlContent this[string key, string text, LCID? lcid = null] { get; }
        IHtmlContent this[object key, LCID? lcid = null] { get; }
        IHtmlContent this[object key, string text, LCID? lcid = null] { get; }
    }
}
