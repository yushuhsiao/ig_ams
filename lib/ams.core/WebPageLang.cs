using ams;
using ams;
using System.Web;

namespace ams
{
    public class WebPageLang
    {
        System.Web.Mvc.WebViewPage page;
        internal WebPageLang(System.Web.Mvc.WebViewPage page)
        {
            this.page = page;
        }
        public string this[string name, string text = null, string path = null, int lcid = 0]
        {
            get
            {
                Lang lang = Lang.GetInstance();
                string s;
                if (string.IsNullOrEmpty(path))
                    goto get1;
                if (lang.GetLang(path, name, lcid, out s))
                    return s;
                get1:
                string p1 = VirtualPathUtility.RemoveTrailingSlash(page.VirtualPath);
                if (lang.GetLang(p1 = VirtualPathUtility.RemoveTrailingSlash(page.VirtualPath)
                    , name, lcid, out s))
                    return s;
                string p2 = VirtualPathUtility.RemoveTrailingSlash(page.Context.Request.AppRelativeCurrentExecutionFilePath);
                if (string.Compare(p1, p2, true) == 0)
                    goto get2;
                if (lang.GetLang(p2, name, lcid, out s))
                    return s;
                get2:
                return text ?? name;
            }
        }
    }
}