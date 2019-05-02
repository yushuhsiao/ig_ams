using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace InnateGlory
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServerMessage : RedisMessage
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public Guid? from { get; set; }

        [JsonProperty(nameof(to))]
        private Guid[] _to
        {
            get
            {
                if (to == null) return null;
                if (to.Length == 0) return null;
                return to;
            }
        }
        public Guid[] to { get; set; }

        [JsonProperty]
        public object data { get; set; }

        protected override void OnDispose()
        {
            this.Name = null;
            this.from = null;
            this.to = null;
            this.data = null;
        }
    }
}
