using Bridge;
using Bridge.Html5;
using Retyped;
using System;
using webix = Retyped.webix.webix2;

namespace Pages
{
    [FileName(util.Pages + "Index.cshtml.js1")]
    public class _Index
    {
        [InlineConst] const string sidebar = "sidebar";
        [InlineConst] const string sidemenu = "sidemenu";
        [InlineConst] const string popupA = "popupA";
        [InlineConst] const string popupA_btn = "popupA_btn";
        [InlineConst] const string popupB = "popupB";
        [InlineConst] const string popupB_btn = "popupB_btn";

        public string UserName
        {
            get => webix.DollarDollar(popupA_btn).As<webix.ui.button>().config.label;
            set
            {
                var btn = webix.DollarDollar(popupA_btn).As<webix.ui.button>();
                btn.config.label = value;
                btn.refresh();
            }
        }
        public string Url_Logout { get; set; }

        private DateTime sidebar_t = DateTime.Now;
        private object sidebar_toggle(object o)
        {
            DateTime t1 = DateTime.Now;
            TimeSpan t2 = t1 - sidebar_t;
            sidebar_t = t1;
            if (t2.TotalMilliseconds > 100)
            {
                var obj = webix.DollarDollar(sidebar).As<webix.ui.sidebar>();
                if (obj.config.hidden == true)
                    obj.show();
                else
                    obj.hide();
            }
            return null;
        }

        public void Init()
        {
            #region left sidebar

            var _sidebar1 = new webix.ui.iconConfig
            {
                view = "icon",
                icon = "bars",
                click = sidebar_toggle
            };

            var _sidebar2 = new webix.ui.sidebarConfig
            {
                id = sidebar,
                view = sidebar,
                width = 300,
                minWidth = 100,
                maxWidth = 500,
                borderless = true,
                data = new object[]
                {
                    new { id = "aa", value = "aa", icon = "", url = "123" },
                    new { id = "bb", value = "bb", icon = "dashboard", url = "456" },
                    "a","b"
                }
                , click = (webix.WebixCallback)(o =>
                {
                    var id = Arguments.GetArgument(0).As<string>();
                    var _this = Bridge.This.Instance.As<webix.ui.sidebar>();
                    Console.WriteLine(_this.getItem(id));
                    Console.WriteLine(Arguments.ToArray());
                    return null;
                })
            };

            #endregion

            #region sidemenu (unused)

            var _sidemenu1 = new webix.ui.iconConfig
            {
                view = "icon",
                icon = "bars",
                hidden = true,
                click = o =>
                {
                    var obj = webix.DollarDollar(sidemenu).As<webix.ui.sidemenu>();
                    if (obj.config.hidden == true)
                        obj.show();
                    else
                        obj.hide();
                    return null;
                }
            };

            var _sidemenu2 = new webix.ui.sidemenuConfig
            {
                id = sidemenu,
                view = sidemenu,
                width = 300,
                position = "left",
                state = o =>
                {
                    var h = webix.DollarDollar("toolbar")._height;
                    var state = o.As<webix.ui.sidemenuConfig>();
                    state.top = h;
                    state.height -= h;
                    return null;
                }
            };

            #endregion

            #region popupA, user menu

            var _popupA1 = new //webix.ui.buttonConfig
            {
                id = popupA_btn,
                view = "button",
                type = "icon",
                icon = "user",
                popup = popupA,
                label = "???",
                autowidth = true,
                //click = (webix.WebixCallback)(o =>
                //{
                //    var obj = webix.DollarDollar(popupA).As<webix.ui.popup>();
                //    if (obj.config.hidden == true)
                //    {
                //        var e = webix.DollarDollar(popupA_btn)._view.As<dom.HTMLElement>();
                //        obj.show(e, new { pos = "bottom" });
                //    }
                //    else obj.hide();
                //    return null;
                //})
            }.As<webix.ui.buttonConfig>();

            var _popupA2 = new webix.ui.contextmenuConfig
            {
                id = popupA,
                view = "contextmenu",
                data = new object[]
                {
                    new { id = "logout", value = "Logout", open = 1 }
                },
                on = new _webix.contextmenuEventHash
                {
                    onItemClick = (id, e, node) =>
                    {
                        switch (id)
                        {
                            case "logout":
                                util.api(Url_Logout, null, result =>
                                {
                                    if (result.IsSuccess)
                                        Window.Instance.Location.Reload(true);
                                });
                                break;
                        }
                        //Console.WriteLine(Arguments.ToArray());
                    }
                }
            };
            //var _popupA2 = new webix.ui.popupConfig
            //{
            //    view = "popup",
            //    id = popupA,
            //};

            //webix.ui2(new webix.ui.menuConfig
            //{
            //    view = "menu"
            //});

            #endregion

            #region popupB, message menu

            var _popupB1 = new //webix.ui.iconConfig
            {
                id = popupB_btn,
                view = "icon",
                icon = "envelope",
                popup = popupB,
                badge = "0",
                //click = o =>
                //{
                //    var obj = webix.DollarDollar(popupA).As<webix.ui.popup>();
                //    if (obj.config.hidden == true)
                //    {
                //        var e = webix.DollarDollar(popupA_btn)._view.As<dom.HTMLElement>();
                //        obj.show(e, new { pos = "bottom" });
                //    }
                //    else obj.hide();
                //    return null;
                //}
            };

            var _popupB2 = new webix.ui.popupConfig
            {
                id = popupB,
                view = "popup",
                body = new webix.ui.menuConfig
                {
                    view = "list",
                    select = true,
                    borderless = false,
                    autoheight = true,
                    autowidth = true,
                    scroll = false,
                    data = new object[]
                    {
                        "a",
                        "b"
                    }
                }.As<webix.ui.baseview>()
            };

            #endregion

            var _toolbar = new webix.ui.toolbarConfig
            {
                view = "toolbar",
                id = "toolbar",
                height = 35,
                padding = 0,
                margin = 0,
                elements = new object[]
                {
                    _sidebar1,
                    _sidemenu1,
                    new { },
                    //new { view = "button", type = "icon", icon = "bars" },
                    new { },
                    _popupA1,
                    _popupB1,
                }
            };

            webix.ui2(new webix.ui.layoutConfig
            {
                id = "page",
                rows = new object[]
                {
                    _toolbar,
                    //new webix.ui.resizerConfig { view = "resizer", height = 1 },
                    new
                    {
                        cols = new object[]
                        {
                            _sidebar2,
                            new webix.ui.resizerConfig { view = "resizer", width = 1 },
                            new webix.ui.iframeConfig { view = "iframe" }
                        }
                    },
                    new webix.ui.resizerConfig { view = "resizer", height = 1 },
                    new webix.ui.templateConfig { height = 10 }
                    }
            });
            webix.ui2(_sidemenu2);
            webix.ui2(_popupA2);
            webix.ui2(_popupB2);
            webix.@event(Window.Instance.As<dom.HTMLElement>(), "resize", o =>
            {
                webix.DollarDollar("page").adjust();
                webix.DollarDollar(sidemenu).adjust();
                //webix.DollarDollar("toolbar").adjust();
                return null;
            });
        }
    }
    //[FileName(util.Pages + "Index.cshtml.js")]
    //public class _Index
    //{
    //    private bool toggle_css(string cssName, bool flag, Union<bool, string> op)
    //    {
    //        if (op == null)
    //            return flag;
    //        var e = dom.document.getElementsByClassName("p00")[0].As<dom.HTMLElement>();

    //        bool hasClass = e.className.Contains(cssName);

    //        if (op.As<string>() == "")
    //            flag = !hasClass;
    //        else if (op.As<bool>() == true)
    //            flag = true;
    //        else if (op.As<bool>() == false)
    //            flag = false;


    //        if (flag)
    //        {
    //            if (!hasClass)
    //                webix.htmlInstance.addCss(e, cssName);
    //        }
    //        else
    //        {
    //            if (hasClass)
    //                webix.htmlInstance.removeCss(e, cssName);
    //        }
    //        return flag;
    //    }

    //    private bool _show_left;
    //    public Union<bool, string> show_left
    //    {
    //        get => _show_left;
    //        set => _show_left = toggle_css("show-left", _show_left, value);
    //    }

    //    private bool _show_right;
    //    public Union<bool, string> show_right
    //    {
    //        get => _show_right;
    //        set => _show_right = toggle_css("show-right", _show_right, value);
    //    }

    //    private bool _dock_left;
    //    public Union<bool, string> dock_left
    //    {
    //        get => _dock_left;
    //        set => _dock_left = toggle_css("dock-left", _dock_left, value);
    //    }

    //    private bool _dock_right;
    //    public Union<bool, string> dock_right
    //    {
    //        get => _dock_right;
    //        set => _dock_right = toggle_css("dock-right", _dock_right, value);
    //    }

    //    public void Init()
    //    {
    //        dock_left = show_left = show_right = true;
    //        //dock_left = true;
    //        //show_right = true;
    //        webix.ui2(new webix.ui.toolbarConfig
    //        {
    //            view = "toolbar",
    //            id = "toolbar",
    //            container = "p10",
    //            borderless = true,
    //            margin = 0,
    //            padding = 0,
    //            height = 35,
    //            cols = new object[]
    //            {
    //                new webix.ui.iconConfig { view = "icon", icon = "bars", borderless = true, click = o => {
    //                    show_left = "";
    //                    return null;
    //                } },
    //                //new webix.ui.iconConfig { view = "icon", icon = "bars", borderless = true , click = o => show_left = "" },
    //                //new webix.ui.iconConfig { view = "icon", icon = "thumb-tack", borderless = true , click = o => dock_left = "" },
    //                new { },
    //                new webix.ui.labelConfig { view = "label", label = "" },
    //                new { },
    //                new { view = "button", type = "icon", icon = "user", id = "username", label = "user", borderless = true, autowidth = true, click = (webix.WebixCallback)(o => {
    //                    show_right = true;
    //                    return null;
    //                }) },
    //                new webix.ui.iconConfig { view = "icon", icon = "bars", borderless = true , click = o => {
    //                    show_right = true;
    //                    return null;
    //                } },
    //                //new webix.ui.iconConfig { view = "icon", icon = "bars", borderless = true , click = o => show_right = "" },
    //                //new webix.ui.iconConfig { view = "icon", icon = "thumb-tack", borderless = true , click = o => dock_right = "" },
    //            }
    //        });

    //        webix.@event(Window.Instance.As<dom.HTMLElement>(), "resize", o =>
    //        {
    //            webix.DollarDollar("toolbar").adjust();
    //            return null;
    //        });
    //        webix.@event(dom.document.getElementsByClassName("p22x")[0].As<dom.HTMLElement>(), "click", o =>
    //        {
    //            dock_right = show_right = false;
    //            return null;
    //        });
    //    }

    //    public string UserName
    //    {
    //        get => webix.DollarDollar("username").As<webix.ui.button>().config.label;
    //        set
    //        {
    //            webix.DollarDollar("username").As<webix.ui.button>().config.label = value;
    //            webix.DollarDollar("username").As<webix.ui.button>().refresh();
    //        }
    //    }
    //}
}