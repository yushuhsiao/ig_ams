Bridge.assembly("ams.js", function ($asm, globals) {
    "use strict";

    Bridge.define("Pages._Index", {
        fields: {
            _show_left: false,
            _show_right: false,
            _dock_left: false,
            _dock_right: false
        },
        props: {
            show_left: {
                get: function () {
                    return this._show_left;
                },
                set: function (value) {
                    this._show_left = this.toggle_css("show-left", this._show_left, value);
                }
            },
            show_right: {
                get: function () {
                    return this._show_right;
                },
                set: function (value) {
                    this._show_right = this.toggle_css("show-right", this._show_right, value);
                }
            },
            dock_left: {
                get: function () {
                    return this._dock_left;
                },
                set: function (value) {
                    this._dock_left = this.toggle_css("dock-left", this._dock_left, value);
                }
            },
            dock_right: {
                get: function () {
                    return this._dock_right;
                },
                set: function (value) {
                    this._dock_right = this.toggle_css("dock-right", this._dock_right, value);
                }
            },
            UserName: {
                get: function () {
                    return webix.$$("username").config.label;
                },
                set: function (value) {
                    webix.$$("username").config.label = value;
                    webix.$$("username").refresh();
                }
            }
        },
        methods: {
            toggle_css: function (cssName, flag, op) {
                if (op == null) {
                    return flag;
                }
                var e = document.getElementsByClassName("p00")[0];

                var hasClass = System.String.contains(e.className,cssName);

                if (Bridge.referenceEquals(op, "")) {
                    flag = !hasClass;
                } else {
                    if (op === true) {
                        flag = true;
                    } else {
                        if (op === false) {
                            flag = false;
                        }
                    }
                }


                if (flag) {
                    if (!hasClass) {
                        webix.html.addCss(e, cssName);
                    }
                } else {
                    if (hasClass) {
                        webix.html.removeCss(e, cssName);
                    }
                }
                return flag;
            },
            Init: function () {
                var $t;
                this.dock_left = ($t = (this.show_right = true, true), this.show_left = $t, $t);
                //dock_left = true;
                //show_right = true;
                webix.ui({ view: "toolbar", id: "toolbar", container: "p10", borderless: true, margin: 0, padding: 0, height: 35, cols: System.Array.init([{ view: "icon", icon: "bars", borderless: true, click: Bridge.fn.bind(this, function (o) {
                    this.show_left = "";
                    return null;
                }) }, { }, { view: "label", label: "" }, { }, { view: "button", type: "icon", icon: "user", id: "username", label: "user", borderless: true, autowidth: true, click: Bridge.fn.bind(this, function (o) {
                    this.show_right = true;
                    return null;
                }) }, { view: "icon", icon: "bars", borderless: true, click: Bridge.fn.bind(this, function (o) {
                    this.show_right = true;
                    return null;
                }) }], System.Object) });

                webix.event(window, "resize", function (o) {
                    webix.$$("toolbar").adjust();
                    return null;
                });
                webix.event(document.getElementsByClassName("p22x")[0], "click", Bridge.fn.bind(this, function (o) {
                    this.dock_right = (this.show_right = false, false);
                    return null;
                }));
            }
        }
    });
});
