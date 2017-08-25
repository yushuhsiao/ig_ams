using Newtonsoft.Json;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http.ModelBinding;

namespace ams
{
    //public static class ErrorStrings
    //{
    //    public const string _Status = "Status";
    //    public const string _Message = "Message";
    //    public const string _Data = "Data";

    //    //public const string InvalidValue = "invalid value";
    //    //public const string ValueIsNullOrEmpty = "value is null or empty";
    //    //public const string ContainsInvalidChar = "value is contains invalid char";
    //    //public const string InvalidConnectionString= "invalid connect string";
    //}

    #region class Exception

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class _Exception : Exception
    {
        [JsonProperty(_ApiResult._Status)]
        public Status StatusCode { get; set; }

        [JsonProperty(_ApiResult._Message)]
        public override string Message
        {
            get { return base.Message; }
        }

        [JsonProperty(_ApiResult._Data)]
        [JsonConverter(typeof(ModelStateJsonConverter))]
        public dynamic _Data { get; set; }

        public override IDictionary Data
        {
            get { return (this._Data as IDictionary) ?? base.Data; }
        }

        //[JsonProperty("_elapsed")]
        public double _elapsed { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public _Exception(Status status, string message = null, Exception innerException = null)
            : base(message ?? status.ToString(), innerException)
        {
            this.StatusCode = status;
            this.HttpStatusCode = HttpStatusCode.BadRequest;
        }

        public _Exception(ModelStateDictionary modelstate)
            : this(Status.InvalidParameter, null, null)
        {
            this.HttpStatusCode = HttpStatusCode.BadRequest;
            this._Data = modelstate;
        }

        public static void HasError(SqlDataReader r, string name= "_Error")
        {
            int n;
            if (r.GetInt32(name, out n))
                throw new _Exception((Status)n, r.GetStringN("msg"));
        }
    }

    #endregion
}