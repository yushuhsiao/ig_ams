using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public struct ApiErrorEntry
    {
        [JsonProperty(_Consts.Api.Field_StatusCode)]
        public Status StatusCode;
        [JsonProperty(_Consts.Api.Field_StatusText)]
        public string StatusText => StatusCode.ToString();
        [JsonProperty(_Consts.Api.Field_Message)]
        public string Message;

    }
//#else
//    public struct ApiErrorEntry
//    {
//        public Status Status
//        {
//            get
//            {
//                if (status.HasValue)
//                    return (Status)status.Value;
//                return Status.Unknown;
//            }
//        }
//        public string Message => msg;
//
//        public long? status { get; set; }
//        public string msg { get; set; }
//    }
//#endif
}