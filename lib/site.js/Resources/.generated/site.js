/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2018
 * @compiler Bridge.NET 17.5.0
 */
Bridge.assembly("site.js", function ($asm, globals) {
    "use strict";

    Bridge.define("InnateGlory.ApiErrorEntry", {
        $kind: "struct",
        statics: {
            methods: {
                getDefaultValue: function () { return new InnateGlory.ApiErrorEntry(); }
            }
        },
        fields: {
            StatusCode: 0,
            Message: null
        },
        props: {
            StatusText: {
                get: function () {
                    return System.Enum.toString(InnateGlory.Status, this.StatusCode);
                }
            }
        },
        ctors: {
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            getHashCode: function () {
                var h = Bridge.addHash([5004677489, this.StatusCode, this.Message]);
                return h;
            },
            equals: function (o) {
                if (!Bridge.is(o, InnateGlory.ApiErrorEntry)) {
                    return false;
                }
                return Bridge.equals(this.StatusCode, o.StatusCode) && Bridge.equals(this.Message, o.Message);
            },
            $clone: function (to) {
                var s = to || new InnateGlory.ApiErrorEntry();
                s.StatusCode = this.StatusCode;
                s.Message = this.Message;
                return s;
            }
        }
    });

    Bridge.define("InnateGlory.ApiResult", {
        props: {
            StatusCode: 0,
            StatusText: {
                get: function () {
                    return System.Enum.toString(InnateGlory.Status, this.StatusCode);
                }
            },
            Message: null,
            Data: null,
            Errors: null,
            httpStatus: 0,
            httpStatusText: null,
            IsSuccess: {
                get: function () {
                    return this.StatusCode === InnateGlory.Status.success;
                }
            }
        },
        ctors: {
            init: function () {
                this.StatusCode = InnateGlory.Status.unknown;
            },
            ctor: function () {
                this.$initialize();
                this.StatusCode = InnateGlory.Status.unknown;
            }
        },
        methods: {
            EnumErrors: function (cb) {
                var $t;
                $t = Bridge.getEnumerator(this.Errors, System.Collections.Generic.KeyValuePair$2(System.String,InnateGlory.ApiErrorEntry));
                try {
                    while ($t.moveNext()) {
                        var n = $t.Current;
                        cb(n.key, n.value.$clone());
                    }
                } finally {
                    if (Bridge.is($t, System.IDisposable)) {
                        $t.System$IDisposable$Dispose();
                    }
                }
            }
        }
    });

    Bridge.define("InnateGlory.Status", {
        $kind: "enum",
        statics: {
            fields: {
                unknown: 0,
                success: 1,
                noResult: 2,
                requiredParameter: 3,
                invalidParameter: 4,
                parameterNotAllow: 5,
                modelStateError: 6,
                accessDenied: 7,
                forbidden: 403,
                _1000: 1000,
                userTypeNotAllow: 1001,
                _2000: 2000,
                unableAllocateUserID: 2001,
                corpNotExist: 2002,
                corpAlreadyExist: 2003,
                corpDisabled: 2004,
                parentDisabled: 2005,
                parentNotExist: 2006,
                agentAlreadyExist: 2007,
                agentNotExist: 2008,
                agentDisabled: 2009,
                adminAlreadyExist: 2010,
                adminNotExist: 2011,
                adminDisabled: 2012,
                memberAlreadyExist: 2013,
                memberNotExist: 2014,
                memberDisabled: 2015,
                userAlreadyExist: 2016,
                userNotExist: 2017,
                userDisabled: 2018,
                passwordNotFound: 2019,
                passwordDisabled: 2020,
                passwordExpired: 2021,
                passwordNotMatch: 2022,
                maxDepthLimit: 2023,
                maxAgentLimit: 2024,
                maxAdminLimit: 2025,
                maxMemberLimit: 2026
            }
        }
    });

    Bridge.define("util.sizes", {
        $kind: "nested class",
        statics: {
            fields: {
                xs: 0,
                sm: 0,
                md: 0,
                lg: 0,
                xl: 0
            },
            ctors: {
                init: function () {
                    this.xs = 0;
                    this.sm = 576;
                    this.md = 768;
                    this.lg = 992;
                    this.xl = 1200;
                }
            }
        }
    });

    Bridge.define("util.webix_ajax_data", {
        $kind: "nested class",
        methods: {
            json: function () {
                return null;
            },
            text: function () {
                return null;
            },
            rawxml: function () {
                return null;
            },
            xml: function () {
                return null;
            }
        }
    });

    Bridge.define("_null");

    Bridge.define("_null$1", function (T) { return {
        statics: {
            fields: {
                value: null
            },
            ctors: {
                init: function () {
                    this.value = Bridge.createInstance(T);
                }
            }
        }
    }; });

    Bridge.define("util", {
        statics: {
            methods: {
                api: function (url, data, callback, opts) {
                    if (opts === void 0) { opts = null; }
                    if (window.jQuery) {
                        util.api_jquery(url, data, callback, opts);
                    } else {
                        util.api_webix(url, data, callback);
                    }
                },
                api_jquery: function (url, data, callback, opts) {
                    if (opts == null) {
                        opts = { };
                    }
                    opts.url = url;
                    opts.data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    opts.contentType = "application/json";
                    opts.type = "post";
                    opts.dataType = "text";
                    opts.cache = false;
                    opts.async = true;
                    var result = null;
                    opts.beforeSend = $asm.$.util.f1;
                    opts.success = function (response_data, textStatus, jqXHR) {
                        try {
                            //result = DeserializeObject(response_data, jqXHR, textStatus);
                            result = Newtonsoft.Json.JsonConvert.DeserializeObject(Bridge.unbox(response_data), InnateGlory.ApiResult);
                            //result.jqXHR = jqXHR;
                            result.httpStatus = jqXHR.status;
                            result.httpStatusText = jqXHR.statusText;
                            //result.XhrStatus = textStatus;
                        } catch ($e1) {
                            $e1 = System.Exception.create($e1);
                        }
                    };
                    opts.error = function (jqXHR, textStatus, errorThrown) {
                        try {
                            //result = DeserializeObject(jqXHR.Response, jqXHR, textStatus);
                            result = Newtonsoft.Json.JsonConvert.DeserializeObject(Bridge.unbox(jqXHR.response), InnateGlory.ApiResult);
                            //result.jqXHR = jqXHR;
                            result.httpStatus = jqXHR.status;
                            result.httpStatusText = jqXHR.statusText;
                            //result.ErrorThrown = errorThrown;
                        } catch ($e1) {
                            $e1 = System.Exception.create($e1);
                        }
                    };
                    opts.complete = function (jqXHR, textStatus) {
                        result = result || new InnateGlory.ApiResult();
                        try {
                            callback(result);
                        } catch ($e1) {
                            $e1 = System.Exception.create($e1);
                        }
                    };
                    $.ajax(opts);
                },
                api_webix: function (url, data, callback) {
                    //webix.DollarDollar()
                    try {
                        var data_json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                        var p = webix.ajax().headers({ "Content-Type": "application/json", AuthKey: "xxxxxxxxxxxxxxx" }).post(url, data);

                        p.then(function (n) {
                            try {
                                var json = n.text();
                                var result = Newtonsoft.Json.JsonConvert.DeserializeObject(json, InnateGlory.ApiResult);
                                result.httpStatus = 200;
                                result.httpStatusText = "OK";
                                try {
                                    callback(result);
                                } catch ($e1) {
                                    $e1 = System.Exception.create($e1);
                                }
                            } catch ($e2) {
                                $e2 = System.Exception.create($e2);
                            }
                            return null;
                        }, function (_xhr) {
                            var $t;
                            var xhr = Bridge.unbox(_xhr);
                            var result;
                            try {
                                var json = xhr.responseText;
                                result = Newtonsoft.Json.JsonConvert.DeserializeObject(json, InnateGlory.ApiResult);
                                result.httpStatus = xhr.status;
                                result.httpStatusText = xhr.statusText;
                            } catch ($e1) {
                                $e1 = System.Exception.create($e1);
                                result = null;
                            }
                            result = result || ($t = new InnateGlory.ApiResult(), $t.StatusCode = InnateGlory.Status.unknown, $t);
                            try {
                                callback(result);
                            } catch ($e2) {
                                $e2 = System.Exception.create($e2);
                            }
                            return null;
                        });
                        //await p.ToTask();
                    } catch ($e1) {
                        $e1 = System.Exception.create($e1);
                    }
                }
            }
        }
    });

    Bridge.ns("util", $asm.$);

    Bridge.apply($asm.$.util, {
        f1: function (jqXHR, obj) {
            jqXHR.setRequestHeader("AuthKey", "xxxxx");
            return true;
        }
    });
});
Bridge.assembly("site.js", function ($asm, globals) {
    "use strict";


    var $m = Bridge.setMetadata,
        $n = ["System","InnateGlory","System.Collections.Generic"];
    $m("_null", function () { return {"att":1048961,"a":2,"s":true}; }, $n);
    $m("_null$1", function (T) { return {"att":1048961,"a":2,"s":true,"m":[{"a":2,"n":"value","is":true,"t":4,"rt":T,"sn":"value"}]}; }, $n);
    $m("util", function () { return {"nested":[util.sizes,System.Object,util.webix_ajax_data],"att":1048961,"a":2,"s":true,"m":[{"a":2,"n":"api","is":true,"t":8,"pi":[{"n":"url","pt":$n[0].String,"ps":0},{"n":"data","pt":System.Object,"ps":1},{"n":"callback","pt":Function,"ps":2},{"n":"opts","dv":null,"o":true,"pt":System.Object,"ps":3}],"sn":"api","rt":$n[0].Void,"p":[$n[0].String,System.Object,Function,System.Object]},{"a":2,"n":"api_jquery","is":true,"t":8,"pi":[{"n":"url","pt":$n[0].String,"ps":0},{"n":"data","pt":System.Object,"ps":1},{"n":"callback","pt":Function,"ps":2},{"n":"opts","pt":System.Object,"ps":3}],"sn":"api_jquery","rt":$n[0].Void,"p":[$n[0].String,System.Object,Function,System.Object]},{"a":2,"n":"api_webix","is":true,"t":8,"pi":[{"n":"url","pt":$n[0].String,"ps":0},{"n":"data","pt":System.Object,"ps":1},{"n":"callback","pt":Function,"ps":2}],"sn":"api_webix","rt":$n[0].Void,"p":[$n[0].String,System.Object,Function]},{"a":1,"n":"has_jquery","is":true,"t":8,"tpc":0,"def":function () { return window.jQuery; },"rt":$n[0].Boolean,"box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},{"a":2,"n":"Pages","is":true,"t":4,"rt":$n[0].String,"sn":"Pages"},{"a":2,"n":"wwwroot","is":true,"t":4,"rt":$n[0].String,"sn":"wwwroot"}]}; }, $n);
    $m("util.sizes", function () { return {"td":util,"att":1048962,"a":2,"s":true,"m":[{"a":2,"n":"lg","is":true,"t":4,"rt":$n[0].Int32,"sn":"lg","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":2,"n":"md","is":true,"t":4,"rt":$n[0].Int32,"sn":"md","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":2,"n":"sm","is":true,"t":4,"rt":$n[0].Int32,"sn":"sm","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":2,"n":"xl","is":true,"t":4,"rt":$n[0].Int32,"sn":"xl","box":function ($v) { return Bridge.box($v, System.Int32);}},{"a":2,"n":"xs","is":true,"t":4,"rt":$n[0].Int32,"sn":"xs","box":function ($v) { return Bridge.box($v, System.Int32);}}]}; }, $n);
    $m("util.webix_ajax_data", function () { return {"td":util,"att":1048579,"a":1,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"json","t":8,"sn":"json","rt":$n[0].Object},{"a":2,"n":"rawxml","t":8,"sn":"rawxml","rt":$n[0].String},{"a":2,"n":"text","t":8,"sn":"text","rt":$n[0].String},{"a":2,"n":"xml","t":8,"sn":"xml","rt":$n[0].String}]}; }, $n);
    $m("InnateGlory.Status", function () { return {"att":257,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"AccessDenied","is":true,"t":4,"rt":$n[1].Status,"sn":"accessDenied","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AdminAlreadyExist","is":true,"t":4,"rt":$n[1].Status,"sn":"adminAlreadyExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AdminDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"adminDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AdminNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"adminNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AgentAlreadyExist","is":true,"t":4,"rt":$n[1].Status,"sn":"agentAlreadyExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AgentDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"agentDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"AgentNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"agentNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"CorpAlreadyExist","is":true,"t":4,"rt":$n[1].Status,"sn":"corpAlreadyExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"CorpDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"corpDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"CorpNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"corpNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"Forbidden","is":true,"t":4,"rt":$n[1].Status,"sn":"forbidden","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"InvalidParameter","is":true,"t":4,"rt":$n[1].Status,"sn":"invalidParameter","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MaxAdminLimit","is":true,"t":4,"rt":$n[1].Status,"sn":"maxAdminLimit","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MaxAgentLimit","is":true,"t":4,"rt":$n[1].Status,"sn":"maxAgentLimit","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MaxDepthLimit","is":true,"t":4,"rt":$n[1].Status,"sn":"maxDepthLimit","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MaxMemberLimit","is":true,"t":4,"rt":$n[1].Status,"sn":"maxMemberLimit","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MemberAlreadyExist","is":true,"t":4,"rt":$n[1].Status,"sn":"memberAlreadyExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MemberDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"memberDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"MemberNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"memberNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"ModelStateError","is":true,"t":4,"rt":$n[1].Status,"sn":"modelStateError","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"NoResult","is":true,"t":4,"rt":$n[1].Status,"sn":"noResult","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"ParameterNotAllow","is":true,"t":4,"rt":$n[1].Status,"sn":"parameterNotAllow","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"ParentDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"parentDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"ParentNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"parentNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"PasswordDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"passwordDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"PasswordExpired","is":true,"t":4,"rt":$n[1].Status,"sn":"passwordExpired","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"PasswordNotFound","is":true,"t":4,"rt":$n[1].Status,"sn":"passwordNotFound","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"PasswordNotMatch","is":true,"t":4,"rt":$n[1].Status,"sn":"passwordNotMatch","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"RequiredParameter","is":true,"t":4,"rt":$n[1].Status,"sn":"requiredParameter","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"Success","is":true,"t":4,"rt":$n[1].Status,"sn":"success","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"UnableAllocateUserID","is":true,"t":4,"rt":$n[1].Status,"sn":"unableAllocateUserID","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"Unknown","is":true,"t":4,"rt":$n[1].Status,"sn":"unknown","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"UserAlreadyExist","is":true,"t":4,"rt":$n[1].Status,"sn":"userAlreadyExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"UserDisabled","is":true,"t":4,"rt":$n[1].Status,"sn":"userDisabled","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"UserNotExist","is":true,"t":4,"rt":$n[1].Status,"sn":"userNotExist","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"UserTypeNotAllow","is":true,"t":4,"rt":$n[1].Status,"sn":"userTypeNotAllow","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"_1000","is":true,"t":4,"rt":$n[1].Status,"sn":"_1000","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},{"a":2,"n":"_2000","is":true,"t":4,"rt":$n[1].Status,"sn":"_2000","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}}]}; }, $n);
    $m("InnateGlory.ApiErrorEntry", function () { return {"att":1048841,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("StatusText")],"a":2,"n":"StatusText","t":16,"rt":$n[0].String,"g":{"a":2,"n":"get_StatusText","t":8,"rt":$n[0].String,"fg":"StatusText"},"fn":"StatusText"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("Message")],"a":2,"n":"Message","t":4,"rt":$n[0].String,"sn":"Message"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("StatusCode")],"a":2,"n":"StatusCode","t":4,"rt":$n[1].Status,"sn":"StatusCode","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}}]}; }, $n);
    $m("InnateGlory.ApiResult", function () { return {"att":1048577,"a":2,"m":[{"a":2,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"EnumErrors","t":8,"pi":[{"n":"cb","pt":Function,"ps":0}],"sn":"EnumErrors","rt":$n[0].Void,"p":[Function]},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("Data")],"a":2,"n":"Data","t":16,"rt":$n[0].Object,"g":{"a":2,"n":"get_Data","t":8,"rt":$n[0].Object,"fg":"Data"},"s":{"a":2,"n":"set_Data","t":8,"p":[$n[0].Object],"rt":$n[0].Void,"fs":"Data"},"fn":"Data"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("Errors")],"a":2,"n":"Errors","t":16,"rt":$n[2].IDictionary$2(System.String,InnateGlory.ApiErrorEntry),"g":{"a":2,"n":"get_Errors","t":8,"rt":$n[2].IDictionary$2(System.String,InnateGlory.ApiErrorEntry),"fg":"Errors"},"s":{"a":2,"n":"set_Errors","t":8,"p":[$n[2].IDictionary$2(System.String,InnateGlory.ApiErrorEntry)],"rt":$n[0].Void,"fs":"Errors"},"fn":"Errors"},{"at":[new Newtonsoft.Json.JsonIgnoreAttribute()],"a":2,"n":"HttpStatus","t":16,"rt":$n[0].Int32,"g":{"a":2,"n":"get_HttpStatus","t":8,"rt":$n[0].Int32,"fg":"httpStatus","box":function ($v) { return Bridge.box($v, System.Int32);}},"s":{"a":2,"n":"set_HttpStatus","t":8,"p":[$n[0].Int32],"rt":$n[0].Void,"fs":"httpStatus"},"fn":"httpStatus"},{"at":[new Newtonsoft.Json.JsonIgnoreAttribute()],"a":2,"n":"HttpStatusText","t":16,"rt":$n[0].String,"g":{"a":2,"n":"get_HttpStatusText","t":8,"rt":$n[0].String,"fg":"httpStatusText"},"s":{"a":2,"n":"set_HttpStatusText","t":8,"p":[$n[0].String],"rt":$n[0].Void,"fs":"httpStatusText"},"fn":"httpStatusText"},{"at":[new Newtonsoft.Json.JsonIgnoreAttribute()],"a":2,"n":"IsSuccess","t":16,"rt":$n[0].Boolean,"g":{"a":2,"n":"get_IsSuccess","t":8,"rt":$n[0].Boolean,"fg":"IsSuccess","box":function ($v) { return Bridge.box($v, System.Boolean, System.Boolean.toString);}},"fn":"IsSuccess"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("Message")],"a":2,"n":"Message","t":16,"rt":$n[0].String,"g":{"a":2,"n":"get_Message","t":8,"rt":$n[0].String,"fg":"Message"},"s":{"a":2,"n":"set_Message","t":8,"p":[$n[0].String],"rt":$n[0].Void,"fs":"Message"},"fn":"Message"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("StatusCode")],"a":2,"n":"StatusCode","t":16,"rt":$n[1].Status,"g":{"a":2,"n":"get_StatusCode","t":8,"rt":$n[1].Status,"fg":"StatusCode","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}},"s":{"a":2,"n":"set_StatusCode","t":8,"p":[$n[1].Status],"rt":$n[0].Void,"fs":"StatusCode"},"fn":"StatusCode"},{"at":[new Newtonsoft.Json.JsonPropertyAttribute.$ctor1("StatusText")],"a":2,"n":"StatusText","t":16,"rt":$n[0].String,"g":{"a":2,"n":"get_StatusText","t":8,"rt":$n[0].String,"fg":"StatusText"},"fn":"StatusText"},{"a":1,"n":"__Property__Initializer__StatusCode","t":4,"rt":$n[1].Status,"sn":"__Property__Initializer__StatusCode","box":function ($v) { return Bridge.box($v, InnateGlory.Status, System.Enum.toStringFn(InnateGlory.Status));}}]}; }, $n);
});
