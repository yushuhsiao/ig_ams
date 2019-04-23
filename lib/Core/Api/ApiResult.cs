﻿using Bridge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using InnateGlory.Api;
using System.Net.Http;

namespace InnateGlory
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class ApiResult
    {
        [JsonProperty(_Consts.Api.Field_StatusCode)]
        public Status StatusCode { get; set; } = Status.Unknown;

        [JsonProperty(_Consts.Api.Field_StatusText)]
        public string StatusText => StatusCode.ToString(); //{ get; set; }

        [JsonProperty(_Consts.Api.Field_Message)]
        public string Message { get; set; }

        [JsonProperty(_Consts.Api.Field_Data)]
        public object Data { get; set; }

        [JsonProperty(_Consts.Api.Field_Error)]
        public IDictionary<string, ApiErrorEntry> Errors { get; set; }
    }

//#if wasm
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public partial class ApiResult : IApiResult, IDisposable
//    {
//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public class tmp<T>
//        {
//            [JsonProperty(_Consts.Api.Field_Data)]
//            public T data;
//        }

//        private string _data_json;
//        private object _data;

//        public HttpStatusCode? HttpStatusCode { get; set; }

//        public bool IsSuccess => HttpStatusCode == System.Net.HttpStatusCode.OK && StatusCode == Status.Success;

//        public bool GetData<T>(T data)
//        {
//            if (_data_json != null)
//            {
//                try
//                {
//                    var tmp = new tmp<T>() { data = data };
//                    JsonHelper.PopulateObject(this._data_json, tmp);
//                    return true;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex);
//                }
//            }
//            return false;
//        }

//        public bool GetData<T>(out T data)
//        {
//            if (_data_json != null)
//            {
//                try
//                {
//                    //var tmp = JsonUtil.Deserialize<tmp<T>>(this._data_json);
//                    var tmp = JsonHelper.DeserializeObject<tmp<T>>(this._data_json);
//                    _data = tmp.data;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex);
//                }
//            }
//            return _data.TryCast(out data);
//        }

//        internal bool GetError(string name, out ApiErrorEntry result)
//        {
//            if (Errors != null && name != null)
//                return Errors.TryGetValue(name, out result);
//            result = default(ApiErrorEntry);
//            return false;
//        }

//        void IDisposable.Dispose()
//        {
//            this._data_json = null;
//            this.Errors?.Clear();
//            this.Errors = null;
//            this._data = null;
//            this.Message = null;
//        }

//        private static readonly object _null_data = new object();

//        public static async Task<ApiResult> Invoke(HttpClient httpClient, string requestUri, object data = null)
//        {
//            try
//            {
//                //var httpClient = util.services.GetService<HttpClient>();
//                var requestJson = JsonHelper.SerializeObject(data ?? _null_data);
//                using (var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
//                {
//                    var response = await httpClient.PostAsync(requestUri, httpContent);

//                    var responseJson = await response.Content.ReadAsStringAsync();

//                    ApiResult result = JsonHelper.DeserializeObject<ApiResult>(responseJson);
//                    result.HttpStatusCode = response.StatusCode;
//                    result._data_json = responseJson;

//                    //if (result.StatusCode != Status.Success)
//                    //{
//                    //    if (result.GetData(out Dictionary<string, ApiErrorEntry> err))
//                    //        result._errors = err;
//                    //}
//                    return result;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }
//            return null;
//        }
//    }
#if jslib
    public partial class ApiResult
    {
        public ApiResult()
        {
            this.StatusCode = Status.Unknown;
        }

        [JsonIgnore, Name("httpStatus")]
        public int HttpStatus { get; set; }

        [JsonIgnore, Name("httpStatusText")]
        public string HttpStatusText { get; set; }

        //[JsonIgnore, Name("xhrStatus")]
        //public string XhrStatus { get; set; }

        // for jQuery.ajax
        //[JsonIgnore, Name("jqXHR")]
        //public object jqXHR { get; set; }

        // for jQuery.ajax
        //[JsonIgnore, Name("errorThrown")]
        //public string ErrorThrown { get; set; }

        //[JsonIgnore, Name("httpResponse")]
        //public object HttpResponse { get; set; }

        [JsonIgnore]
        public bool IsSuccess => this.StatusCode == Status.Success;

        public void EnumErrors(Action<string, ApiErrorEntry> cb)
        {
            foreach (var n in this.Errors)
            {
                cb(n.Key, n.Value);
            }
        }
    }
#else
    public partial class ApiResult : IApiResult
    {
        public ApiResult(object data)
        {
            this.Data = data;
            this.StatusCode = Status.Unknown;
        }

        public string ContentType { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        Task IActionResult.ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            return context.HttpContext.RequestServices.GetRequiredService<ApiResultExecutor>().ExecuteAsync(context, this);
        }


        private static List<IApiResult> _staticResults = new List<IApiResult>();
        //private static Dictionary<Status, IApiResult> _staticResults = new Dictionary<Status, IApiResult>();
        private static IApiResult GetStaticResult(Status statusCode, HttpStatusCode? http = System.Net.HttpStatusCode.OK)
        {
            lock (_staticResults)
            {
                for (int i = 0, n = _staticResults.Count; i < n; i++)
                {
                    var nn = _staticResults[i];
                    if (nn.StatusCode == statusCode &&
                        nn.HttpStatusCode == http)
                        return nn;
                }
                var ret = new ApiResult(null)
                {
                    StatusCode = statusCode,
                    HttpStatusCode = http
                };
                _staticResults.Add(ret);
                return ret;
            }
        }

        //public static IApiResult Unknown => GetStaticResult(Status.Unknown);
        public static IApiResult OK => GetStaticResult(Status.Success);
        public static IApiResult Forbidden => GetStaticResult(Status.Forbidden, System.Net.HttpStatusCode.Forbidden);
        public static IApiResult Failed(Status statusCode = Status.Unknown) => GetStaticResult(statusCode);

        public static IApiResult Success(object value = null)
        {
            if (value == null) return OK;
            return new ApiResult(value)
            {
                StatusCode = Status.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }

        //[DebuggerStepThrough]
        //public static IApiResult IsSuccess(Status statusCode, object value = null)
        //{
        //    if (statusCode == Status.Success)
        //        return Success(value);
        //    else
        //        throw new ApiException(statusCode);
        //}

        public static IActionResult FromActionResult(IActionResult src)
        {
            if (src is IApiResult)
                return src;
            //ApiResult result = new ApiResult(src);
            if (src is JsonResult)
            {
                JsonResult n = (JsonResult)src;
                return new ApiResult(n.Value)
                {
                    StatusCode = Status.Success,
                    ContentType = n.ContentType,
                    HttpStatusCode = (HttpStatusCode?)n.StatusCode
                };
                //result.ContentType = n.ContentType;
                //result.HttpStatusCode = (HttpStatusCode?)n.StatusCode;
            }
            else if (src is ObjectResult)
            {
                ObjectResult n = (ObjectResult)src;
                return new ApiResult(n.Value)
                {
                    StatusCode = Status.Success,
                    HttpStatusCode = (HttpStatusCode?)n.StatusCode
                };
                //result.HttpStatusCode = (HttpStatusCode?)n.StatusCode;
            }
            return new ApiResult(src);
        }
    }
    partial class ApiResult : IDisposable
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class tmp<T>
        {
            [JsonProperty(_Consts.Api.Field_Data)]
            public T data;
        }

        private string _data_json;
        private object _data;

        //public HttpStatusCode? HttpStatusCode { get; set; }

        public bool IsSuccess => HttpStatusCode == System.Net.HttpStatusCode.OK && StatusCode == Status.Success;

        public bool GetData<T>(T data)
        {
            if (_data_json != null)
            {
                try
                {
                    var tmp = new tmp<T>() { data = data };
                    JsonHelper.PopulateObject(this._data_json, tmp);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return false;
        }

        public bool GetData<T>(out T data)
        {
            if (_data_json != null)
            {
                try
                {
                    //var tmp = JsonUtil.Deserialize<tmp<T>>(this._data_json);
                    var tmp = JsonHelper.DeserializeObject<tmp<T>>(this._data_json);
                    _data = tmp.data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return _data.TryCast(out data);
        }

        internal bool GetError(string name, out ApiErrorEntry result)
        {
            if (Errors != null && name != null)
                return Errors.TryGetValue(name, out result);
            result = default(ApiErrorEntry);
            return false;
        }

        void IDisposable.Dispose()
        {
            this._data_json = null;
            this.Errors?.Clear();
            this.Errors = null;
            this._data = null;
            this.Message = null;
        }

        private static readonly object _null_data = new object();

        public static async Task<ApiResult> Invoke(HttpClient httpClient, string requestUri, object data = null)
        {
            try
            {
                //var httpClient = util.services.GetService<HttpClient>();
                var requestJson = JsonHelper.SerializeObject(data ?? _null_data);
                using (var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync(requestUri, httpContent);

                    var responseJson = await response.Content.ReadAsStringAsync();

                    ApiResult result = JsonHelper.DeserializeObject<ApiResult>(responseJson);
                    result.HttpStatusCode = response.StatusCode;
                    result._data_json = responseJson;

                    //if (result.StatusCode != Status.Success)
                    //{
                    //    if (result.GetData(out Dictionary<string, ApiErrorEntry> err))
                    //        result._errors = err;
                    //}
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
#endif
}