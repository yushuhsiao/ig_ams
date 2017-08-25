using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Configuration;
using BundleTransformer.Core.FileSystem;
using BundleTransformer.Core.HttpHandlers;
using BundleTransformer.Core.Minifiers;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Less.Translators;
using BundleTransformer.MicrosoftAjax.Minifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;
using Tools;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Diagnostics;
using System.Web.Caching;
using System.Linq;
using BundleTransformer.Core.Translators;
using System.Security.Cryptography;

namespace ams
{
    public class Bundles
    {
        interface _item { }
        abstract class _item<T> : _item where T : _item<T>, new()
        {
            protected _item(string[] prefix, string[] ext)
            {
                this.prefix = prefix;
                this.ext = ext;
            }
            readonly string[] prefix;
            readonly string[] ext;
            string[] _url = new string[2];
            bool[] _reg = new[] { false, false };
            //public string[] files { get; protected set; }

            List<string> files2 = new List<string>();

            public IHtmlString Render(bool minify)
            {
                int n = minify ? 0 : 1;
                if (this._url[n] == null) return null;
                if (this.files2.Count == 0) return null;
                lock (this)
                {
                    if (this._reg[n] == false)
                    {
                        BundleTable.Bundles.Add(this.CreateBundle(this._url[n], minify).Include(this.files2.ToArray()));
                        this._reg[n] = true;
                    }
                }
                return Render(this._url[n]);
            }

            protected abstract Bundle CreateBundle(string url, bool minify);

            protected abstract IHtmlString Render(string url);

            static Dictionary<string, Bundles> items = new Dictionary<string, Bundles>();

            public static T _new(string url, params object[] files)
            {
                T obj = new T();
                string[] prefix = obj.prefix;
                if (url != null)
                {
                    obj._url[0] = prefix[0] + url;
                    obj._url[1] = prefix[1] + url;
                }
                foreach (object f2 in files)
                    obj.add_file(f2);
                return obj;
            }
            void add_file(object file, WebViewPage check_exist = null)
            {
                if (file is string)
                {
                    string f = (string)file;
                    if (this.files2.Contains(f, true))
                        return;
                    if (check_exist != null)
                    {
                        if (!File.Exists(check_exist.Server.MapPath(f)))
                            return;
                    }
                    this.files2.Add(f);
                }
                else if (file is T)
                {
                    foreach (string s in ((T)file).files2)
                        this.add_file(s);
                }
                else if (file is Bundles)
                {
                    Bundles b = (Bundles)file;
                    this.add_file(b.css);
                    this.add_file(b.js);
                }
            }

            public static Bundles GetItem(WebViewPage page, string path, string[] common_files, params object[] files)
            {
                VirtualPath p0 = VirtualPath.GetPath(page.VirtualPath);
                Bundles b;
                lock (items)
                {
                    if (items.TryGetValue(page.VirtualPath, out b)) return b;
                    if (path == null)
                    {
                        _Controller c = page.ViewContext.Controller as _Controller;
                        if (c != null)
                        {
                            path = c.GetActionUrl(c.ActionDescriptor);
                            if (path != null)
                                path = VirtualPathUtility.ToAbsolute(path).Substring(1);
                        }
                        path = path ?? page.VirtualPath.Substring(2);
                    }
                    T obj = _new(path);
                    foreach (object f1 in files)
                    {
                        obj.add_file(f1);
                        if (f1 is string) obj.add_file(f1);
                        else if (f1 is Bundles)
                        {
                            Bundles b1 = (Bundles)f1;
                            obj.add_file(b1.js);
                            obj.add_file(b1.css);
                        }
                    }
                    foreach (string f2 in common_files)
                        obj.add_file(f2, page);
                    foreach (string f3 in obj.ext)
                    {
                        obj.add_file(Path.ChangeExtension(page.VirtualPath, f3), page);
                        foreach (VirtualPath p1 in p0.GetParents(false, false))
                            obj.add_file(p1.FullPath + "/" + p1.Name + "." + f3, page);
                    }
                    return items[page.VirtualPath] = new Bundles(obj);
                }
            }
        }
        class _css : _item<_css>
        {
            public _css() : base(new[] { "~/css/", "~/css/_/" }, new[] { "less" }) { }

            protected override Bundle CreateBundle(string virtualPath, bool minify)
            {
                return new Bundle(virtualPath, null, new[] { new StyleTransformer(
                    BundleTransformerContext.Current.Styles.GetMinifierInstance(minify ? typeof(MicrosoftAjaxCssMinifier).Name : typeof(NullMinifier).Name),
                    //_LessTranslator.GetTranslators(),
                    BundleTransformerContext.Current.Styles.GetDefaultTranslatorInstances(),
                    BundleTransformerContext.Current.Styles.GetDefaultPostProcessorInstances())})
                { Builder = new NullBuilder(), Orderer = new NullOrderer() };
            }

            protected override IHtmlString Render(string url) { return Styles.Render(url); }
        }
        class _js : _item<_js>
        {
            public _js() : base(new[] { "~/js/", "~/js/_/" }, new[] { "js" }) { }

            protected override Bundle CreateBundle(string virtualPath, bool minify)
            {
                return new Bundle(virtualPath, null, new[] { new ScriptTransformer(
                    BundleTransformerContext.Current.Scripts.GetMinifierInstance(minify ? typeof(MicrosoftAjaxJsMinifier).Name : typeof(NullMinifier).Name),
                    BundleTransformerContext.Current.Scripts.GetDefaultTranslatorInstances(),
                    BundleTransformerContext.Current.Scripts.GetDefaultPostProcessorInstances()) })
                { Builder = new NullBuilder(), Orderer = new NullOrderer() };
            }

            protected override IHtmlString Render(string url) { return Scripts.Render(url); }
        }

        readonly _js js;
        readonly _css css;
        Bundles(params _item[] _init)
        {
            foreach (_item n in _init)
            {
                if (this.js == null)
                    this.js = n as _js;
                if (this.css == null)
                    this.css = n as _css;
            }
            this.js = js ?? _js._new(null);
            this.css = css ?? _css._new(null);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Clear();

            BundleResolver.Current = new CustomBundleResolver();

            //lock (_all)
            //{
            //    foreach (Bundles n in _all)
            //    {
            //        if (n.css.url != null)
            //            bundles.Add(new _StyleBundle(n.css.url, n.css.minify).Include(n.css.files));
            //        if (n.js.url != null)
            //            bundles.Add(new _ScriptBundle(n.js.url, n.js.minify).Include(n.js.files));
            //    }
            //}
        }

        public IHtmlString RenderStyle(bool minify = true) { return this.css.Render(minify); }
        public IHtmlString RenderScript(bool minify = true) { return this.js.Render(minify); }
        public IHtmlString Render(bool minify = true)
        {
            IHtmlString h1 = this.css.Render(minify);
            IHtmlString h2 = this.js.Render(minify);
            return new HtmlString($"{h1}{h2}");
        }

        public static IHtmlString RenderStyles(WebViewPage page, string path = null, bool minify = true, params object[] files)
        {
            return RenderStyles(page, path, null, minify, files);
        }
        public static IHtmlString RenderStyles(WebViewPage page, string path, string[] common_files, bool minify, params object[] files)
        {
            return _css.GetItem(page, path, common_files ?? new[] { "~/css/site.less" }, files).RenderStyle(minify);
        }

        public static IHtmlString RenderScripts(WebViewPage page, string path = null, bool minify = true, params object[] files)
        {
            return RenderScripts(page, path, null, minify, files);
        }
        public static IHtmlString RenderScripts(WebViewPage page, string path, string[] common_files, bool minify, params object[] files)
        {
            return _js.GetItem(page, path, common_files ?? new[] { "~/js/site.js" }, files).RenderScript(minify);
        }

        public static readonly Bundles jquery = new Bundles(
            _js._new("jquery"
                , "~/Scripts/jquery-{version}.js"));

        public static readonly Bundles jquery_ui = new Bundles(
            _js._new("jquery-ui"
                , "~/Scripts/jquery-ui-{version}.js"));

        public static readonly Bundles knockout = new Bundles(
    _js._new("knockout"
        , "~/Scripts/knockout-{version}.js"));

        public static readonly Bundles bootstrap = new Bundles(
            _js._new("bootstrap"
                , "~/Scripts/bootstrap.js"
                , "~/lib/bootstrap_dropdowns_enhancement/js/dropdowns-enhancement.js"
                , "~/Scripts/respond.js"),
            _css._new("bootstrap"
                , "~/css/bootstrap.less"));

        #region angular js
        /*
~/Scripts/angular-aria.js
~/Scripts/angular-cookies.js
~/Scripts/angular-csp.css
~/Scripts/angular-loader.js
~/Scripts/angular-message-format.js
~/Scripts/angular-messages.js
~/Scripts/angular-mocks.js
~/Scripts/angular-resource.js
~/Scripts/angular-route.js
~/Scripts/angular-sanitize.js
~/Scripts/angular-scenario.js
~/Scripts/angular-touch.js
        */

        public static readonly Bundles angular_core = new Bundles(
            _js._new("angular-core"
                , "~/Scripts/angular.js"));

        public static readonly Bundles angular_animate = new Bundles(
            _js._new("angular-animate"
                , "~/Scripts/angular-animate.js"));

        public static readonly Bundles angular_ui = new Bundles(
            _js._new("angular-ui"
                , "~/Scripts/angular-ui.js"
                , "~/Scripts/angular-ui-ieshiv.js"));

        public static readonly Bundles angular_ui_utils = new Bundles(
            _js._new("angular-ui-util"
                , "~/Scripts/angular-ui/ui-utils.js"
                , "~/Scripts/angular-ui/ui-utils-ieshiv.js"));

        public static readonly Bundles angular_ui_bootstrap = new Bundles(
            _js._new("angular-ui-bootstrap"
                , "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

        public static readonly Bundles angular_legacy = new Bundles(
            _js._new("angular-legacy"
                , "~/lib/angular.js.legacy/angular.js"));

        public static readonly Bundles angular = new Bundles(
            _js._new("angular"
                , angular_core.js
                , angular_animate.js
                , angular_ui_bootstrap.js
                , angular_ui.js));

        #endregion

        public static readonly Bundles modernizr = new Bundles(
            _js._new("modernizr"
                , "~/Scripts/modernizr-*"));

        public static readonly Bundles signalr = new Bundles(
            _js._new("signalr"
                , "~/Scripts/jquery.signalr-{version}.js"));

        public static readonly Bundles site = new Bundles(
            _js._new("site"
                , "~/js/_site.js"),
            _css._new("~/css/site"
                , "~/css/_stie.css"));

        public static readonly Bundles json2 = new Bundles(
            _js._new("json2"
                , "~/Scripts/json2.js"));

        public static readonly Bundles string_format = new Bundles(
            _js._new("string-format",
                "~/lib/string-format/dist/string-format.js"));

        public static readonly Bundles webui_popover = new Bundles(
            _js._new(null
                , "~/js/webui-popover/dist/jquery.webui-popover.js"),
            _css._new(null
                , "~/js/webui-popover/dist/jquery.webui-popover.css"));

        public static readonly Bundles webui_popover_ng = new Bundles(
            _js._new(null
                , "~/js/jquery.webui-popover-ng.js"));

        //public static readonly Bundles font_awesome = new Bundles(
        //    _css._new(null
        //        , "~/fonts/Font-Awesome/less/font-awesome.less"));

        //public static readonly Bundles devicons = new Bundles(
        //    _css._new(null
        //        , "~/fonts/devicons/css/devicons.css"));

        //public static readonly Bundles octicons = new Bundles(
        //    _css._new(null
        //        , "~/fonts/octicons/octicons/octicons.less"));

        //public static readonly Bundles supr = new Bundles(
        //    _css._new(null
        //        , "~/css/supr/html/css/icons.css"));

        //public static readonly Bundles fonts = new Bundles(
        //    _css._new("fonts"
        //        , "~/fonts/fonts.less"));

        public static readonly Bundles moment = new Bundles(
            _js._new("moment"
                , "~/lib/moment/moment.js"));

        public static readonly Bundles ngJsTree = new Bundles(
            _js._new(null, "~/lib/ngJsTree/dist/ngJsTree.js"));

        public static readonly Bundles jstree = new Bundles(
            _js._new("jstree"
                , "~/lib/jstree/dist/jstree.js"
                , ngJsTree.js),
            _css._new("jstree"
                , "~/lib/jstree/src/themes/default/style.less"));

        public static readonly Bundles jstree_dark = new Bundles(
            jstree.js,
            _css._new("jstree-dark"
                , "~/lib/jstree/src/themes/default-dark/style.less"));


        #region jqwidgets
        const string _jqwidgets_base_path = "~/js/jqwidgets/";
        const string _jqwidgets_css_path = _jqwidgets_base_path + "styles/";
        public static readonly Bundles jqwidgets = new Bundles(
            _js._new("jqx"
                , _jqwidgets_base_path + "jqxcore.js"
                , _jqwidgets_base_path + "jqxdata.js"
                , _jqwidgets_base_path + "jqxvalidator.js"
                , _jqwidgets_base_path + "jqxbuttons.js"
                , _jqwidgets_base_path + "jqxdropdownbutton.js"
                , _jqwidgets_base_path + "jqxcolorpicker.js"
                , _jqwidgets_base_path + "jqxswitchbutton.js"
                , _jqwidgets_base_path + "jqxscrollbar.js"
                , _jqwidgets_base_path + "jqxpanel.js"
                , _jqwidgets_base_path + "jqxtooltip.js"
                , _jqwidgets_base_path + "jqxcalendar.js"
                , _jqwidgets_base_path + "jqxdatetimeinput.js"
                , _jqwidgets_base_path + "jqxdraw.js"
                , _jqwidgets_base_path + "jqxchart.js"
                , _jqwidgets_base_path + "jqxchart.core.js"
                , _jqwidgets_base_path + "jqxchart.api.js"
                , _jqwidgets_base_path + "jqxchart.waterfall.js"
                , _jqwidgets_base_path + "jqxchart.annotations.js"
                , _jqwidgets_base_path + "jqxchart.rangeselector.js"
                , _jqwidgets_base_path + "jqxgauge.js"
                , _jqwidgets_base_path + "jqxcheckbox.js"
                , _jqwidgets_base_path + "jqxbuttongroup.js"
                , _jqwidgets_base_path + "jqxlistbox.js"
                , _jqwidgets_base_path + "jqxtree.js"
                , _jqwidgets_base_path + "jqxdragdrop.js"
                , _jqwidgets_base_path + "jqxcombobox.js"
                , _jqwidgets_base_path + "jqxdropdownlist.js"
                , _jqwidgets_base_path + "jqxwindow.js"
                , _jqwidgets_base_path + "jqxdocking.js"
                , _jqwidgets_base_path + "jqxdockpanel.js"
                , _jqwidgets_base_path + "jqxmaskedinput.js"
                , _jqwidgets_base_path + "jqxmenu.js"
                , _jqwidgets_base_path + "jqxexpander.js"
                , _jqwidgets_base_path + "jqxnavigationbar.js"
                , _jqwidgets_base_path + "jqxnumberinput.js"
                , _jqwidgets_base_path + "jqxprogressbar.js"
                , _jqwidgets_base_path + "jqxradiobutton.js"
                , _jqwidgets_base_path + "jqxrating.js"
                , _jqwidgets_base_path + "jqxslider.js"
                , _jqwidgets_base_path + "jqxsplitter.js"
                , _jqwidgets_base_path + "jqxtabs.js"
                , _jqwidgets_base_path + "jqxgrid.js"
                , _jqwidgets_base_path + "jqxgrid.selection.js"
                , _jqwidgets_base_path + "jqxgrid.columnsresize.js"
                , _jqwidgets_base_path + "jqxgrid.sort.js"
                , _jqwidgets_base_path + "jqxgrid.filter.js"
                , _jqwidgets_base_path + "jqxgrid.grouping.js"
                , _jqwidgets_base_path + "jqxgrid.pager.js"
                , _jqwidgets_base_path + "jqxgrid.edit.js"
                , _jqwidgets_base_path + "jqxgrid.aggregates.js"
                , _jqwidgets_base_path + "jqxgrid.export.js"
                , _jqwidgets_base_path + "jqxgrid.storage.js"
                , _jqwidgets_base_path + "jqxgrid.columnsreorder.js"
                , _jqwidgets_base_path + "jqxdata.export.js"
                , _jqwidgets_base_path + "jqxlistmenu.js"
                , _jqwidgets_base_path + "jqxknockout.js"
                , _jqwidgets_base_path + "jqxscrollview.js"
                , _jqwidgets_base_path + "jqxtouch.js"
                , _jqwidgets_base_path + "jqxinput.js"
                , _jqwidgets_base_path + "jqxresponse.js"
                , _jqwidgets_base_path + "jqxtreemap.js"
                , _jqwidgets_base_path + "jqxpasswordinput.js"
                , _jqwidgets_base_path + "jqxrangeselector.js"
                , _jqwidgets_base_path + "jqxdatatable.js"
                , _jqwidgets_base_path + "jqxtreegrid.js"
                , _jqwidgets_base_path + "jqxbulletchart.js"
                , _jqwidgets_base_path + "jqxeditor.js"
                , _jqwidgets_base_path + "jqxnotification.js"
                , _jqwidgets_base_path + "jqxangular.js"
                , _jqwidgets_base_path + "jqxtoolbar.js"
                , _jqwidgets_base_path + "jqxcomplexinput.js"
                , _jqwidgets_base_path + "jqxformattedinput.js"
                , _jqwidgets_base_path + "jqxribbon.js"
                , _jqwidgets_base_path + "jqxnavbar.js"
                , _jqwidgets_base_path + "jqxfileupload.js"
                , _jqwidgets_base_path + "jqxloader.js"
                , _jqwidgets_base_path + "jqxtextarea.js"
                , _jqwidgets_base_path + "jqxpopover.js"
                , _jqwidgets_base_path + "jqxlayout.js"
                , _jqwidgets_base_path + "jqxdockinglayout.js"
                , _jqwidgets_base_path + "jqxresponsivepanel.js"
                , _jqwidgets_base_path + "jqxtagcloud.js"
                , _jqwidgets_base_path + "jqxdate.js"
                , _jqwidgets_base_path + "jqxscheduler.js"
                , _jqwidgets_base_path + "jqxscheduler.api.js"
                , _jqwidgets_base_path + "globalization/globalize.js",
                "~/js/jqx.js"),
            _css._new("jqx"
                , _jqwidgets_css_path + "jqx.less"));
        #endregion
    }


    public class JsAssetHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
        }
    }
}