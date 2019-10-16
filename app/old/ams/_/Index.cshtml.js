/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2018
 * @compiler Bridge.NET 16.8.2
 */
Bridge.assembly("ams.js", function ($asm, globals) {
    "use strict";

    Bridge.define("Pages.LoginComponent", {
        inherits: [InnateGlory.VueComponent],
        fields: {
            apiResult: null,
            closable: false,
            busy: false
        },
        ctors: {
            init: function () {
                this.apiResult = _null$1(InnateGlory.ApiResult).value;
            }
        },
        methods: {
            Init: function () {
                this.el = "#frm_login";
                this.data = { CorpName: "root", UserName: "root", Password: "root", LoginType: "Agent" };
                this.methods = { };
                this.methods.showLogin = Bridge.fn.bind(this, function (args) {
                    return this.ShowLogin(args);
                }); // ((Action<bool?>)this.ShowLogin).As<methodsConfig.keyFn>();
                this.methods.hideLogin = Bridge.fn.cacheBind(this, this.HideLogin); //args => this.HideLogin(); // ((Action)this.HideLogin).As<methodsConfig.keyFn>();
                this.methods.execLogin = Bridge.fn.cacheBind(this, this.ExecLogin); // ((Action)this.ExecLogin).As<methodsConfig.keyFn>();
                this.methods.isBusy = Bridge.fn.bind(this, function (args) {
                    return this.busy;
                });
                this.methods.closable = Bridge.fn.bind(this, function (args) {
                    return this.closable;
                });
                this.methods.clearError = Bridge.fn.bind(this, function (args) {
                    return this.clearError(args);
                });
                this.methods.getError = Bridge.fn.bind(this, function (args) {
                    return this.getError(args, true);
                }); //((Func<string, object>)getError).as
                this.methods.getErrorMsg = Bridge.fn.bind(this, function (args) {
                    return this.getError(args, false);
                }); //((Func<string, object>)getError).as
                this.methods.getApiResult = Bridge.fn.bind(this, function (args) {
                    return this.apiResult;
                });
                this.computed = { };
            },
            clearError: function (name) {
                var entry = { };
                if (util.GetError(Bridge.global.InnateGlory.ApiErrorEntry, this.apiResult, name, entry)) {
                    entry.v.Status = 0;
                    entry.v.Hidden = true;
                    this.vm.$forceUpdate();
                }
                return null;
            },
            getError: function (name, getStatus) {
                var entry = { };
                if (!util.GetError(Bridge.global.InnateGlory.ApiErrorEntry, this.apiResult, name, entry)) {
                    entry.v = _null$1(InnateGlory.ApiErrorEntry).value;
                }
                if (getStatus) {
                    return entry.v.Status;
                } else {
                    return entry.v.Message;
                }
            },
            ShowLogin: function (closable) {
                var obj = jQuery("#frm_login");
                var opts = { };
                this.closable = true;
                if (System.Nullable.eq(closable, false)) {
                    opts.backdrop = "static";
                    this.closable = false;
                }
                obj.modal(opts);
                this.vm.$forceUpdate();
                return null;
            },
            HideLogin: function () {
                if (this.closable) {
                    jQuery("#frm_login").modal("hide");
                }
            },
            ExecLogin: function () {
                var url = jQuery("#frm_login").data("url");
                var _data = this.vm.$data;
                if (System.String.isNullOrEmpty(_data.UserName) || System.String.isNullOrEmpty(_data.Password)) {
                    return;
                }
                //dom.console.log(url);
                util.api(url, _data, { BeforeSend: Bridge.fn.cacheBind(this, this.BeforeSend), ApiSuccess: Bridge.fn.bind(this, function (data, result) { return this.ApiSuccess(System.Object, data, result); }), ApiFailed: Bridge.fn.cacheBind(this, this.ApiFailed), ApiComplete: Bridge.fn.cacheBind(this, this.ApiComplete) });
            },
            BeforeSend: function () {
                this.busy = true;
                this.apiResult = _null$1(InnateGlory.ApiResult).value;
                this.vm.$forceUpdate();
            },
            ApiSuccess: function (TData, data, result) {
                this.apiResult = result;
            },
            ApiFailed: function (status, result) {
                this.apiResult = result;
            },
            ApiComplete: function (status, result) {
                this.apiResult = result;
                this.busy = false;
                this.vm.$forceUpdate();
            }
        }
    });

    Bridge.define("Pages.SidebarComponent", {
        inherits: [InnateGlory.VueComponent],
        methods: {
            Init: function () {
                this.el = ".p00";
                this.methods = { };
                this.methods.showLogin = Pages.Index._login.methods.showLogin; //((Action<bool?>)index._login.ShowLogin).As<methodsConfig.keyFn>();
                this.methods.setPanel = Bridge.fn.cacheBind(this, this.SetPanel);
            },
            SetClass: function (_obj, name, value) {
                if (value === true) {
                    return _obj.addClass(name);
                }
                if (value === false) {
                    return _obj.removeClass(name);
                }
                if (value === -1) {
                    return _obj.toggleClass(name);
                }
                return _obj;
            },
            SetPanel: function (dock_left, dock_right, show_left, show_right) {
                var obj = jQuery(".p20");
                this.SetClass(obj, "dock-left", dock_left);
                this.SetClass(obj, "dock-right", dock_right);
                this.SetClass(obj, "show-left", show_left);
                this.SetClass(obj, "show-right", show_right);
            }
        }
    });

    Bridge.define("Pages.Index", {
        statics: {
            fields: {
                _login: null,
                _sidebar: null
            },
            methods: {
                Init: function () {
                    Pages.Index._login = new Pages.LoginComponent();
                    Pages.Index._sidebar = new Pages.SidebarComponent();
                    if (jQuery("#frm_login").data("isGuest") === 1) {
                        Pages.Index._login.ShowLogin(false);
                    }
                }
            }
        }
    });

    Bridge.init(function () { Pages.Index.Init(); });

    var $m = Bridge.setMetadata,
        $n = [InnateGlory,System,Pages];
    $m("Pages.LoginComponent", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":1,"n":"ApiComplete","t":8,"pi":[{"n":"status","pt":$n[0].Status,"ps":0},{"n":"result","pt":$n[0].ApiResult,"ps":1}],"sn":"ApiComplete","rt":$n[1].Void,"p":[$n[0].Status,$n[0].ApiResult]},{"a":1,"n":"ApiFailed","t":8,"pi":[{"n":"status","pt":$n[0].Status,"ps":0},{"n":"result","pt":$n[0].ApiResult,"ps":1}],"sn":"ApiFailed","rt":$n[1].Void,"p":[$n[0].Status,$n[0].ApiResult]},{"a":1,"n":"ApiSuccess","t":8,"pi":[{"n":"data","pt":System.Object,"ps":0},{"n":"result","pt":$n[0].ApiResult,"ps":1}],"tpc":1,"tprm":["TData"],"sn":"ApiSuccess","rt":$n[1].Void,"p":[System.Object,$n[0].ApiResult]},{"a":1,"n":"BeforeSend","t":8,"sn":"BeforeSend","rt":$n[1].Void},{"a":2,"n":"ExecLogin","t":8,"sn":"ExecLogin","rt":$n[1].Void},{"a":2,"n":"HideLogin","t":8,"sn":"HideLogin","rt":$n[1].Void},{"ov":true,"a":2,"n":"Init","t":8,"sn":"Init","rt":$n[1].Void},{"a":2,"n":"ShowLogin","t":8,"pi":[{"n":"closable","pt":$n[1].Nullable$1(System.Boolean),"ps":0}],"sn":"ShowLogin","rt":$n[1].Object,"p":[$n[1].Nullable$1(System.Boolean)]},{"a":2,"n":"clearError","t":8,"pi":[{"n":"name","pt":$n[1].String,"ps":0}],"sn":"clearError","rt":$n[1].Object,"p":[$n[1].String]},{"a":2,"n":"getError","t":8,"pi":[{"n":"name","pt":$n[1].String,"ps":0},{"n":"getStatus","pt":$n[1].Boolean,"ps":1}],"sn":"getError","rt":$n[1].Object,"p":[$n[1].String,$n[1].Boolean]},{"a":2,"n":"_selector","is":true,"t":4,"rt":$n[1].String,"sn":"_selector"},{"a":1,"n":"apiResult","t":4,"rt":$n[0].ApiResult,"sn":"apiResult"},{"a":1,"n":"busy","t":4,"rt":$n[1].Boolean,"sn":"busy","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":1,"n":"closable","t":4,"rt":$n[1].Boolean,"sn":"closable","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}}]}; });
    $m("Pages.SidebarComponent", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"ov":true,"a":2,"n":"Init","t":8,"sn":"Init","rt":$n[1].Void},{"a":1,"n":"SetClass","t":8,"pi":[{"n":"_obj","pt":Bridge.virtualc("JQuery"),"ps":0},{"n":"name","pt":$n[1].String,"ps":1},{"n":"value","pt":System.Object,"ps":2}],"sn":"SetClass","rt":Bridge.virtualc("JQuery"),"p":[Bridge.virtualc("JQuery"),$n[1].String,System.Object]},{"a":2,"n":"SetPanel","t":8,"pi":[{"n":"dock_left","pt":System.Object,"ps":0},{"n":"dock_right","pt":System.Object,"ps":1},{"n":"show_left","pt":System.Object,"ps":2},{"n":"show_right","pt":System.Object,"ps":3}],"sn":"SetPanel","rt":$n[1].Void,"p":[System.Object,System.Object,System.Object,System.Object]},{"a":2,"n":"_selector","is":true,"t":4,"rt":$n[1].String,"sn":"_selector"}]}; });
    $m("Pages.Index", function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":1,"n":"Init","is":true,"t":8,"sn":"Init","rt":$n[1].Void},{"a":2,"n":"_login","is":true,"t":4,"rt":$n[2].LoginComponent,"sn":"_login"},{"a":2,"n":"_sidebar","is":true,"t":4,"rt":$n[2].SidebarComponent,"sn":"_sidebar"}]}; });
});
