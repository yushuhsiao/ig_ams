using Newtonsoft.Json;
using System;
using System.Threading;

namespace StackExchange.Redis
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RedisMessage : IDisposable
    {
        [JsonProperty]
        internal int msg_id { get; set; }

        [JsonIgnore]
        public RedisChannel Channel { get; set; }

        [JsonIgnore]
        public RedisValue Value { get; set; }

        void IDisposable.Dispose()
        {
            this.msg_id = 0;
            this.Channel = default(RedisChannel);
            this.Value = default(RedisValue);
            this.OnDispose();
        }

        protected virtual void OnDispose() { }
    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //public class RedisMessageEx : IDisposable
    //{
    //    private int _msg_id;

    //    [JsonProperty]
    //    public int msg_id
    //    {
    //        get => Interlocked.CompareExchange(ref this._msg_id, 0, 0);
    //        set => Interlocked.Exchange(ref this._msg_id, value);
    //    }

    //    [JsonIgnore]
    //    public DateTime Time { get; set; }

    //    [JsonIgnore]
    //    public RedisChannel Channel { get; set; }

    //    [JsonIgnore]
    //    public RedisValue Value { get; set; }

    //    internal void Init(int msg_id)
    //    {
    //        this.msg_id = msg_id;
    //    }

    //    void IDisposable.Dispose()
    //    {
    //        this.msg_id = 0;
    //        this.OnDispose();
    //    }

    //    protected virtual void OnDispose() { }
    //}
}
