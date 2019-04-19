using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections;
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

        private string statusText;
        [JsonProperty(_Consts.Api.Field_StatusText)]
        public string StatusText
        {
            get => statusText ?? StatusCode.ToString();
            set => statusText = value;
        }

        public string ResultMessage { get; set; }

        [JsonProperty(_Consts.Api.Field_Message)]
        string IApiResult.Message
        {
            get => ResultMessage ?? StatusCode.ToString();
            set => ResultMessage = value;
        }


        static object _value = new object();

        [JsonProperty(_Consts.Api.Field_Data)]
        object IApiResult.Data
        {
            get => this.ModelError ?? this.Data ?? _value;
            set { }
        }

        public SerializableError ModelError { get; set; }

        [JsonProperty(_Consts.Api.Field_Error)]
        public virtual IDictionary<string, ApiErrorEntry> Errors
        {
            get => _value as IDictionary<string, ApiErrorEntry>;
            set { }
        }

        public string ContentType { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }


        Task IActionResult.ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            return context.HttpContext.RequestServices.GetRequiredService<ApiResultExecutor>().ExecuteAsync(context, this);
        }
    }
}