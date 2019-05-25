using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using LCID = System.Int32;

namespace InnateGlory
{
    internal class ViewLang : IViewLang
    {
        private LangService _langService;
        public PlatformId PlatformId { get; set; }
        private string resPath;
        private LangItem _langItem;

        public string ResPath
        {
            get => resPath;
            set
            {
                resPath = value;
                _langItem = null;
            }
        }
        //internal string ViewEnginePath { get; set; }
        //internal string FullPath { get; set; }

        internal static IViewLang GetInstance(IServiceProvider service) => new ViewLang(service.GetService<DataService>());

        private ViewLang(DataService service)
        {
            service.GetService(ref _langService);
        }

        private IEnumerable<LangItem> GetLangItem()
        {
            if (_langItem == null)
            {
                var n01 = _langService.GetCacheEntry(this.PlatformId);
                LangItem n0 = n01.GetFirstValue();

                _langItem = n0.GetChild(this.ResPath.Trim(false), true);
            }
            if (_langItem != null)
                yield return _langItem;
        }

        //private IEnumerable<LangItem> GetLangItem()
        //{
        //    var n01 = _langService.GetEntry(this.PlatformId);
        //    LangItem n0 = n01.GetFirstValue();

        //    LangItem n1 = n0.GetChild(this.ResPath.Trim(false));
        //    if (n1 != null)
        //        yield return n1;

        //    LangItem n2 = n0.GetChild(this.ViewEnginePath);
        //    if (n2 == n1) n2 = null;
        //    if (n2 != null)
        //        yield return n2;

        //    LangItem n3 = n0.GetChild(this.FullPath);
        //    if (n3 == n1 || n3 == n2) n3 = null;
        //    if (n3 != null)
        //        yield return n3;
        //}

        private HtmlString GetText(string key, LCID? lcid) => this.GetEnum(key, key, lcid);
        private HtmlString GetText(object key, LCID? lcid) => this.GetEnum(key, key?.ToString(), lcid);
        private HtmlString GetEnum(string key, string text, LCID? lcid)
        {
            foreach (var n in this.GetLangItem())
                if (n.GetText(this.PlatformId, key, lcid, text, out var _html))
                    return _html;
            return new HtmlString(text);
        }
        private HtmlString GetEnum(object key, string text, LCID? lcid)
        {
            foreach (var n in this.GetLangItem())
                if (n.GetEnum(this.PlatformId, key, lcid, text, out var _html))
                    return _html;
            return new HtmlString(text);
        }

        IHtmlContent IViewLang.this[string key, LCID? lcid] => this.GetEnum(key, key, lcid);
        IHtmlContent IViewLang.this[object key, LCID? lcid] => this.GetEnum(key, key?.ToString(), lcid);
        IHtmlContent IViewLang.this[string key, string text, LCID? lcid] => this.GetEnum(key, text, lcid);
        IHtmlContent IViewLang.this[object key, string text, LCID? lcid] => this.GetEnum(key, text, lcid);
    }
}
