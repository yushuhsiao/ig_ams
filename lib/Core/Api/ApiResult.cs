using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InnateGlory
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiResult : IApiResult, IDisposable
    {
        public ApiResult(object data)
        {
            this.Data = data;
            this.StatusCode = Status.Unknown;
        }

        [JsonProperty(_Consts.Api.Field_StatusCode)]
        public Status StatusCode { get; set; } = Status.Unknown;

        [JsonProperty(_Consts.Api.Field_StatusText)]
        public string StatusText => StatusCode.ToString(); //{ get; set; }

        [JsonProperty(_Consts.Api.Field_Message)]
        public string Message { get; set; }

        [JsonProperty(_Consts.Api.Field_Data)]
        public object Data { get; set; }

        //[JsonProperty(_Consts.Api.Field_Error)]
        //public IDictionary<string, ApiErrorEntry> Errors { get; set; }

        public string ContentType { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        async Task IActionResult.ExecuteResultAsync(ActionContext context) => await ApiResultExecutor.ExecuteResultAsync(context, this);


        //private static List<IApiResult> _staticResults = new List<IApiResult>();
        //private static IApiResult GetStaticResult(Status statusCode, HttpStatusCode? http = System.Net.HttpStatusCode.OK)
        //{
        //    lock (_staticResults)
        //    {
        //        for (int i = 0, n = _staticResults.Count; i < n; i++)
        //        {
        //            var nn = _staticResults[i];
        //            if (nn.StatusCode == statusCode &&
        //                nn.HttpStatusCode == http)
        //                return nn;
        //        }
        //        var ret = new ApiResult(null)
        //        {
        //            StatusCode = statusCode,
        //            HttpStatusCode = http
        //        };
        //        _staticResults.Add(ret);
        //        return ret;
        //    }
        //}

        //public static IApiResult OK => GetStaticResult(Status.Success);
        //public static IApiResult Forbidden => GetStaticResult(Status.Forbidden, System.Net.HttpStatusCode.Forbidden);
        //public static IApiResult Failed(Status statusCode = Status.Unknown) => GetStaticResult(statusCode);

        private static ApiResult _empty = new ApiResult(null) { StatusCode = Status.Success };
        public static IApiResult Success(object value = null)
        {
            if (value == null)
                return _empty;
            return new ApiResult(value)
            {
                StatusCode = Status.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }




        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class tmp<T>
        {
            [JsonProperty(_Consts.Api.Field_Data)]
            public T data;
        }

        private string _data_json;
        private object _data;

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
                catch { }
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex);
                //}
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
                catch { }
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex);
                //}
            }
            return _data.TryCast(out data);
        }

        //internal bool GetError(string name, out ApiErrorEntry result)
        //{
        //    if (Errors != null && name != null)
        //        return Errors.TryGetValue(name, out result);
        //    result = default(ApiErrorEntry);
        //    return false;
        //}

        void IDisposable.Dispose()
        {
            this._data_json = null;
            //this.Errors?.Clear();
            //this.Errors = null;
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
}