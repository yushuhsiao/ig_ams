using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace InnateGlory
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiException : Exception, IApiResult
    {
        public ApiException(Status statusCode)
        {
            this.StatusCode = statusCode;
        }
        public ApiException(Status statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.ResultMessage = message;
        }
        public ApiException(Status statusCode, string message, Exception innerException) : base("", innerException)
        {
            this.StatusCode = statusCode;
            this.ResultMessage = message;
        }

        [JsonProperty(_Consts.Api.Field_StatusCode)]
        public Status StatusCode { get; set; }

        [JsonProperty(_Consts.Api.Field_StatusText)]
        public string StatusText => StatusCode.ToString();

        [JsonProperty(_Consts.Api.Field_Message)]
        string IApiResult.Message
        {
            get => ResultMessage ?? base.Message;
            set => ResultMessage = value;
        }

        [JsonProperty(_Consts.Api.Field_Data)]
        object IApiResult.Data
        {
            get => this.Data;
            set { }
        }

        [JsonProperty(_Consts.Api.Field_Error)]
        public IDictionary<string, object> Errors
        {
            get => ModelError;//  as IDictionary<string, object>;
            set { }
        }

        public string ContentType { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        async Task IActionResult.ExecuteResultAsync(ActionContext context) => await ApiResultExecutor.ExecuteResultAsync(context, this);

        public string ResultMessage { get; set; }

        public SerializableError ModelError { get; set; }
    }
}