using Bridge;
using Bridge.jQuery2;
using Bridge.Html5;
using InnateGlory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using static Retyped.webix;
using webix = Retyped.webix.webix2;


[Priority(-1)]
public static class _null
{
}
[Priority(-1)]
public static class _null<T> where T : new()
{
    public static T value = new T();
}
[Priority(-1)]
public static partial class util
{
    [Template("window.jQuery")]
    private static bool has_jquery() => false;
    [InlineConst]
    public const string wwwroot = "../../../";
    [InlineConst]
    public const string Pages = wwwroot + "Pages/";

    public static class sizes
    {
        public static int xs = 0;
        public static int sm = 576;
        public static int md = 768;
        public static int lg = 992;
        public static int xl = 1200;
    }

    //[Init(InitPosition.After)]
    //public static void Main()
    //{
    //        //init_vue();
    //        //_null<ApiErrorEntry>.value.Hidden = true;
    //        //var p = new requirejs.RequireConfig.pathsConfig();
    //        //p["vue"] = "/lib/vue/dist/vue.min";
    //        //requirejs.require.config(new requirejs.RequireConfig()
    //        //{
    //        //    paths = p
    //        //});
    //}

    //[Template("window.vue = Vue;")]
    //static void init_vue() { }

    //private static ApiResult DeserializeObject(object response_data, jqXHR jqXHR, string xhrStatus)
    //{
    //    ApiResult result = JsonConvert.DeserializeObject<ApiResult>(response_data.As<string>());
    //    if (jqXHR != null)
    //    {
    //        result.jqXHR = jqXHR;
    //        result.HttpStatus = (int)jqXHR.Status;
    //        result.HttpStatusText = jqXHR.StatusText;
    //    }
    //    if (xhrStatus != null)
    //        result.XhrStatus = xhrStatus;
    //    return result;
    //}

    //private static ApiResult GetErrorData(this ApiResult result)
    //{
    //    if (result == null)
    //        return result;
    //    try
    //    {
    //        var tmp1 = Bridge.Html5.JSON.Stringify(result.Data);
    //        var tmp2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(tmp1);
    //        result.Errors = tmp2;
    //    }
    //    catch { }
    //    return result;
    //}

    //public static bool GetError<T>(this ApiResult src, string name, out T result)
    //{
    //    if (src != null && src.Errors != null && name != null)
    //    {
    //        object obj;
    //        if (src.Errors.TryGetValue(name, out obj))
    //        {
    //            if (obj is T)
    //            {
    //                result = (T)obj;
    //                return true;
    //            }
    //            try
    //            {
    //                string tmp1 = JSON.Stringify(obj);
    //                T tmp2 = JsonConvert.DeserializeObject<T>(tmp1);
    //                src.Errors[name] = result = tmp2;
    //                return true;
    //            }
    //            catch { }
    //        }
    //    }
    //    result = default(T);
    //    return false;
    //}

    //private static void _null_ApiResult(ApiResult result) { }
    //private static void _null_ApiSuccess(object data, ApiResult result) { }
    //private static void _null_ApiFailed(Status status, ApiResult result) { }
    //private static void _null_ApiComplete(Status status, ApiResult result) { }
    //private static void _null_BeforeSend() { }


    //public static void _api(string url, dynamic data, ApiOptions opts) //=> api<object>(url, data, opts);
    ////public static void api<TData>(string url, dynamic data, ApiOptions<TData> opts)
    //{
    //    if (opts == null)
    //        opts = new ApiOptions();
    //    opts.Url = url;
    //    opts.Data = JsonConvert.SerializeObject(data);
    //    opts.ContentType = "application/json";
    //    opts.Type = "post";
    //    opts.DataType = "text";
    //    opts.Cache = false;
    //    opts.Async = true;
    //    var _Success = opts.ApiSuccess ?? _null_ApiSuccess;
    //    var _Failed = opts.ApiFailed ?? _null_ApiFailed;
    //    var _Complete = opts.ApiComplete ?? _null_ApiComplete;
    //    //var _BeforeSend = opts.BeforeSend ?? _null_BeforeSend;
    //    Script.Delete(opts.ApiSuccess);
    //    Script.Delete(opts.ApiFailed);
    //    Script.Delete(opts.ApiComplete);
    //    //Script.Delete(opts.BeforeSend);
    //    ApiResult result = null;
    //    //opts.BeforeSend = (@this, jqXHR, settings) =>
    //    //{
    //    //    _BeforeSend();
    //    //    return null;
    //    //};
    //    opts.Success = (response_data, textStatus, jqXHR) =>
    //    {
    //        try
    //        {
    //            //result = DeserializeObject(response_data.As<string>(), jqXHR, textStatus);
    //            result = JsonConvert.DeserializeObject<ApiResult>(response_data.As<string>());
    //            result.jqXHR = jqXHR;
    //            result.HttpStatus = jqXHR.Status;
    //            result.HttpStatusText = jqXHR.StatusText;
    //            result.XhrStatus = textStatus;
    //            if (result.Status == Status.Success)
    //                _Success(result.Data, result);
    //            else
    //                _Failed(result.Status, result);
    //        }
    //        catch
    //        {
    //            result = result ?? new ApiResult();
    //            _Failed(Status.Unknown, result);
    //        }
    //    };
    //    opts.Error = (jqXHR, textStatus, errorThrown) =>
    //    {
    //        try
    //        {
    //            //result = DeserializeObject(jqXHR.Response, jqXHR, textStatus);
    //            result = JsonConvert.DeserializeObject<ApiResult>(jqXHR.Response.As<string>());
    //            result.jqXHR = jqXHR;
    //            result.HttpStatus = jqXHR.Status;
    //            result.HttpStatusText = jqXHR.StatusText;
    //            result.XhrStatus = textStatus;
    //            result.ErrorThrown = errorThrown;
    //            _Failed(result.Status, result);
    //        }
    //        catch
    //        {
    //            result = result ?? new ApiResult();
    //            _Failed(Status.Unknown, result);
    //        }
    //        //return null;
    //    };
    //    opts.Complete = (jqXHR, textStatus) =>
    //    {
    //        try
    //        {
    //            _Complete(result.Status, result);
    //        }
    //        catch
    //        {
    //            result = result ?? new ApiResult();
    //            _Complete(Status.Unknown, result);
    //        }
    //        //return null;
    //    };
    //    jQuery.Ajax(opts);
    //}

    public static void api(string url, dynamic data, Action<ApiResult> callback, AjaxOptions opts = null)
    {
        if (has_jquery())
        {
            api_jquery(url, data, callback, opts);
        }
        else
        {
            api_webix(url, data, callback);
        }
    }

    public static void api_jquery(string url, dynamic data, Action<ApiResult> callback, AjaxOptions opts)
    {
        if (opts == null)
            opts = new AjaxOptions();
        opts.Url = url;
        opts.Data = JsonConvert.SerializeObject(data);
        opts.ContentType = "application/json";
        opts.Type = "post";
        opts.DataType = "text";
        opts.Cache = false;
        opts.Async = true;
        ApiResult result = null;
        opts.BeforeSend = (jqXHR, obj) =>
        {
            jqXHR.SetRequestHeader("AuthKey", "xxxxx");
            return true;
        };
        opts.Success = (response_data, textStatus, jqXHR) =>
        {
            try
            {
                //result = DeserializeObject(response_data, jqXHR, textStatus);
                result = JsonConvert.DeserializeObject<ApiResult>(response_data.As<string>());
                //result.jqXHR = jqXHR;
                result.HttpStatus = (int)jqXHR.Status;
                result.HttpStatusText = jqXHR.StatusText;
                //result.XhrStatus = textStatus;
            }
            catch { }
        };
        opts.Error = (jqXHR, textStatus, errorThrown) =>
        {
            try
            {
                //result = DeserializeObject(jqXHR.Response, jqXHR, textStatus);
                result = JsonConvert.DeserializeObject<ApiResult>(jqXHR.Response.As<string>());
                //result.jqXHR = jqXHR;
                result.HttpStatus = (int)jqXHR.Status;
                result.HttpStatusText = jqXHR.StatusText;
                //result.ErrorThrown = errorThrown;
            }
            catch { }
        };
        opts.Complete = (jqXHR, textStatus) =>
        {
            result = result ?? new ApiResult();
            try { callback(result); }
            catch { }
        };
        jQuery.Ajax(opts);
    }

    [ObjectLiteral]
    class webix_ajax_header
    {
        [Name("Content-Type")]
        public string ContentType { get; set; }
        [Name("AuthKey")]
        public string AuthKey { get; set; }
    }
    class webix_ajax_data
    {
        public object json() => null;
        public string text() => null;
        public string rawxml() => null;
        public string xml() => null;
    }

    public static void api_webix(string url, dynamic data, Action<ApiResult> callback)
    {
        //webix.DollarDollar()
        try
        {
            string data_json = JsonConvert.SerializeObject(data);
            Retyped.es5.Promise<object> p = webix.ajax().headers(new webix_ajax_header
            {
                ContentType = "application/json",
                AuthKey = "xxxxxxxxxxxxxxx",
            }).post(url, data);

            p.As<Retyped.es5.Promise<webix_ajax_data>>().then<object, object>(
                onfulfilled: n =>
                {
                    try
                    {
                        string json = n.text();
                        var result = JsonConvert.DeserializeObject<ApiResult>(json);
                        result.HttpStatus = 200;
                        result.HttpStatusText = "OK";
                        try { callback(result); } catch { }
                    }
                    catch { }
                    return null;
                },
                onrejected: _xhr =>
                {
                    var xhr = _xhr.As<XMLHttpRequest>();
                    ApiResult result;
                    try
                    {
                        string json = xhr.ResponseText;
                        result = JsonConvert.DeserializeObject<ApiResult>(json);
                        result.HttpStatus = xhr.Status;
                        result.HttpStatusText = xhr.StatusText;
                    }
                    catch
                    {
                        result = null;
                    }
                    result = result ?? new ApiResult() { StatusCode = Status.Unknown };
                    try { callback(result); } catch { }
                    return null;
                });
            //await p.ToTask();
        }
        catch
        {
        }
    }

    //public static void api2(string url, dynamic data, Action<ApiResult> callback, AjaxOptions opts)
    //{
    //    if (opts == null)
    //        opts = new AjaxOptions();
    //    opts.Url = url;
    //    opts.Data = JsonConvert.SerializeObject(data);
    //    opts.ContentType = "application/json";
    //    opts.Type = "post";
    //    opts.DataType = "text";
    //    opts.Cache = false;
    //    opts.Async = true;
    //    Task.Run(async () =>
    //    {
    //        ApiResult result;
    //        try
    //        {
    //            var promise = await jQuery.Ajax(opts);
    //            var response_data = promise[0];
    //            var textStatus = promise[1].As<string>();
    //            var jqXHR = promise[2].As<jqXHR>();
    //            result = DeserializeObject(response_data, jqXHR, textStatus);
    //        }
    //        catch (PromiseException ex)
    //        {
    //            var jqXHR = ex.Arguments[0].As<jqXHR>();
    //            var textStatus = ex.Arguments[1].As<string>();
    //            result = DeserializeObject(jqXHR.Response, jqXHR, textStatus);
    //            result.ErrorThrown = ex.Arguments[2].As<string>();
    //        }
    //        catch (Exception ex)
    //        {
    //            result = new ApiResult() { Status = Status.Unknown };
    //        }
    //        callback(result);
    //        return result;
    //    });
    //}
}