using Newtonsoft.Json;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class _ApiResult
    {
        public const string _Status = "Status";
        public const string _Message = "Message";
        public const string _Data = "Data";

        [JsonProperty(_ApiResult._Status)]
        public Status Status;
        [JsonProperty(_ApiResult._Message)]
        public string Message;
        [JsonProperty(_ApiResult._Data)]
        [JsonConverter(typeof(ModelStateJsonConverter))]
        public dynamic Data;
    }
}
