using Bridge;
using Bridge.Html5;
using InnateGlory;
using InnateGlory.Models;
using Retyped;
using System;
using System.Collections.Generic;
using webix = Retyped.webix.webix2;

namespace Pages
{
    [FileName(util.Pages + "Login.cshtml.js")]
    public class _Login
    {
        public string Url { get; set; }
        private string id { get; set; } = "form_login";
        public class langs
        {
            public string CorpName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string LoginType { get; set; }
            public string LoginType_Agent { get; set; }
            public string LoginType_Admin { get; set; }
            public string LoginType_Member { get; set; }
            public string Login { get; set; }
            public string Clear { get; set; }
        }

        private webix.ui.form _form => webix.DollarDollar(id).As<webix.ui.form>();
        public langs lang { get; set; }

        [ObjectLiteral]
        class _rules
        {
            public Func<bool> UserName;
            public Func<bool> Password;
        }

        webix.ui.formConfig formConfig() => new webix.ui.formConfig
        {
            container = "login_container",
            view = "form",
            id = this.id,
            scroll = false,
            maxWidth = 300,
            rules = new _rules
            {
                UserName = webix.rulesInstance.isNotEmpty,
                Password = webix.rulesInstance.isNotEmpty
            },
            elements = new object[]
                {
                    new webix.ui.textConfig { view = "text", name = nameof(LoginModel.CorpName), label = lang.CorpName, value = "root", },
                    new webix.ui.textConfig { view = "text", name = nameof(LoginModel.UserName), label = lang.UserName, value = "root", },
                    new webix.ui.textConfig { view = "text", name = nameof(LoginModel.Password), label = lang.Password, value = "root", type = "password" },
                    new webix.ui.segmentedConfig
                    {
                        view = "segmented", label = lang.LoginType, name = nameof(LoginModel.LoginType), multiview = true, value = nameof(UserType.Agent),
                        options = new object[]
                        {
                            new { id = nameof(UserType.Agent), value = lang.LoginType_Agent },
                            new { id = nameof(UserType.Admin), value = lang.LoginType_Admin },
                            new { id = nameof(UserType.Member), value = lang.LoginType_Member }
                        }
                    },
                    new webix.ui.multiviewConfig
                    {
                        cols = new object[]
                        {
                            new webix.ui.viewConfig { },
                            new webix.ui.buttonConfig { view = "button", label = lang.Login, click = btn_Login_click },
                            new webix.ui.buttonConfig { view = "button", label = lang.Clear, click = btn_Clear_click }
                        }
                    },
                    new webix.ui.labelConfig { view = "label", id = "err_msg", label = "", align = "right" }
                },
            on = new _webix.formEventHash
            {
                onSubmit = onSubmit
            }
        };

        public void Init()
        {
            _Login _login = this;
            webix.ui2(formConfig());
            webix.@event(Window.Instance.As<dom.HTMLElement>(), "resize", resize);
        }

        private void onSubmit(webix.ui.form view, KeyboardEvent e)
        {
            var _login = this;
            var form = this._form;
            var msg = webix.DollarDollar("err_msg").As<webix.ui.label>(); ;
            if (form.validate())
            {
                msg.setValue("");
                dynamic data = form.getValues();

                form.disable();
                util.api(_login.Url, data, (Action<ApiResult>)(result =>
                {
                    form.enable();
                    if (result.IsSuccess)
                    {
                        Window.Instance.Location.Reload(true);
                    }
                    else
                    {
                        msg.setValue(result.Message);
                        foreach (var n in result.Errors)
                        {
                            form.markInvalid(n.Key, n.Value.Message);
                        }
                    }
                }));
            }
        }

        private object resize(params object[] args)
        {
            _form.adjust();
            return null;
        }

        private object btn_Login_click(params object[] args)
        {
            var form = this._form;
            form.callEvent(nameof(webix.ui.formEventName.onSubmit), null);
            return null;
        }

        private object btn_Clear_click(params object[] args)
        {
            var form = this._form;
            form.clear();
            form.markInvalid(nameof(LoginModel.UserName), false);
            form.markInvalid(nameof(LoginModel.Password), false);
            webix.DollarDollar("err_msg").As<webix.ui.label>().setValue("");
            return null;
        }
    }
}