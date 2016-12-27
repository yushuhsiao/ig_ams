using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Web;
using System;
using LCID = System.Int32;
using System.Web.Mvc;
using ams;
using System.Web.WebPages;

namespace System.Web.Mvc
{
    //[DebuggerStepThrough]
    //public abstract class _WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>, _WebViewLang
    //{
    //    public _trim trim = _trim._instance;

    //    //public LangItem lang
    //    //{
    //    //    get { return LangItem.Cache.Value.GetChild(this.VirtualPath, true); }
    //    //}

    //    public _WebViewLang lang
    //    {
    //        [DebuggerStepThrough]
    //        get { return this; }
    //    }

    //    string _WebViewLang.this[object name, int? lcid]
    //    {
    //        [DebuggerStepThrough]
    //        get { return this.GetLang(name as string, name, lcid); }
    //    }

    //    string _WebViewLang.this[string text, object name, int? lcid]
    //    {
    //        [DebuggerStepThrough]
    //        get { return this.GetLang(text, name, lcid); }
    //    }

    //    public PageObjectID _id
    //    {
    //        get { return PageObjectID.Current; }
    //    }
    //}

    public abstract class _WebViewPage<TModel> : _WebViewPage { }

    //[DebuggerStepThrough]
    public abstract class _WebViewPage : System.Web.Mvc.WebViewPage, _WebViewLang
    {
        public bool IsDetails { get { return (this.ViewContext.Controller as _Controller)?.IsDetails ?? false; } }
        public bool IsAddRow { get { return (this.ViewContext.Controller as _Controller)?.IsAddRow ?? false; } }

        public _trim trim = _trim._instance;

        //public LangItem lang
        //{
        //    get { return LangItem.Cache.Value.GetChild(this.VirtualPath, true); }
        //}

        public _WebViewLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string _WebViewLang.this[object name, int? lcid]
        {
            [DebuggerStepThrough]
            get { return this.GetLang(name as string, name, lcid); }
        }

        string _WebViewLang.this[string text, object name, int? lcid]
        {
            [DebuggerStepThrough]
            get { return this.GetLang(text, name, lcid); }
        }

        public PageObjectID _id
        {
            get { return PageObjectID.Current; }
        }

        public WebPageBase ParentPage { get; private set; }

        protected override void ConfigurePage(WebPageBase parentPage)
        {
            this.ParentPage = parentPage;
            base.ConfigurePage(parentPage);
        }

        protected override void InitializePage()
        {
            //for (int i = 0; i < this.Request.QueryString.Count; i++)
            //{
            //    if (string.Compare(this.Request.QueryString.Get(i), "addrow", true) == 0)
            //        this.IsNew = true;
            //    else if (string.Compare(this.Request.QueryString.Get(i), "details", true) == 0)
            //        this.IsDetail = true;
            //}

            base.InitializePage();
        }

        //public bool IsNew { get; private set; }
        //public bool IsDetail { get; private set; }
    }

    public interface _WebViewLang
    {
        string this[object name, LCID? lcid = null] { get; }
        string this[string text, object name, LCID? lcid = null] { get; }
    }

    public class _trim
    {
        internal static _trim _instance = new _trim();
        public string this[string value] { get { return value.Trim(true); } }
    }

    public sealed class PageObjectID
    {
        public static PageObjectID Current
        {
            get { return _HttpContext.Current.GetData(typeof(PageObjectID).FullName, (key) => new PageObjectID()); }
        }

        Dictionary<object, string> dict = new Dictionary<object, string>();

        public string this[object key, int length = 20]
        {
            get
            {
                string name;
                if (dict.TryGetValue(key, out name))
                    return name;
                else
                {
                    for (;;)
                    {
                        name = RandomValue.GetRandomString(length, RandomValue.LowerLetter);
                        if (!dict.ContainsValue(name))
                            return dict[key] = name;
                    }
                }
            }
        }
    }

    public static partial class util
    {
        public static string GetLang(this WebViewPage page, string text, object name, int? lcid)
        {
            LangItem lang_root = LangItem.Cache.Value;

            string result;
            string _name = name as string;
            string p1 = page.VirtualPath;
            lang_root.GetChild(p1, true).GetValue(_name, lcid, out result);
            if (result == null)
            {
                RazorView r2 = page.ViewContext.View as RazorView;
                if (r2 != null)
                {
                    string p2 = r2.ViewPath;
                    if (!string.IsNullOrEmpty(p2) && (string.Compare(p1, p2, true) != 0))
                        lang_root.GetChild(p2, true).GetValue(_name, lcid, out result);
                }
            }
            if ((name != null) && (result == null))
            {
                Type t = name.GetType();
                if (t.IsEnum)
                    lang_root.GetEnumNode(t).GetValue(text = name.ToString(), lcid, out result, false);
            }

            return result ?? text;
        }

        public static IHtmlString JsonRaw(this HtmlHelper helper, object obj)
        {
            return helper.Raw(json.SerializeObject(obj));
        }
        public static IHtmlString Raw(this HtmlHelper helper, string format, params object[] args)
        {
            return helper.Raw(string.Format(format, args));
        }
        public static IHtmlString Raw(this HtmlHelper helper, string format, object arg0)
        {
            return helper.Raw(string.Format(format, arg0));
        }
        public static IHtmlString Raw(this HtmlHelper helper, string format, object arg0, object arg1)
        {
            return helper.Raw(string.Format(format, arg0, arg1));
        }
        public static IHtmlString Raw(this HtmlHelper helper, string format, object arg0, object arg1, object arg2)
        {
            return helper.Raw(string.Format(format, arg0, arg1, arg2));
        }
        public static IHtmlString RawN(this HtmlHelper helper, string value)
        {
            if (value == null) return null;
            return helper.Raw(value);
        }
        public static IHtmlString RawN(this HtmlHelper helper, object value)
        {
            if (value == null) return null;
            return helper.Raw(value);
        }
    }
}