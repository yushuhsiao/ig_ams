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
