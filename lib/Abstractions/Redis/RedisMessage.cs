using InnateGlory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StackExchange.Redis
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RedisMessage
    {
        [JsonProperty]
        private int msg_id;

        [JsonIgnore]
        public RedisChannel Channel { get; set; }

        [JsonIgnore]
        public RedisValue Value { get; set; }

        protected virtual void OnReset() { }



        public class Pool<TMessage> where TMessage : RedisMessage, new()
        {
            private int _msg_id;

            private Queue<TMessage> _pooling = new Queue<TMessage>();

            public TMessage Alloc()
            {
                TMessage msg;
                lock (_pooling)
                {
                    if (_pooling.Count == 0)
                        msg = new TMessage();
                    else
                        msg = _pooling.Dequeue();
                }
                msg.msg_id = Interlocked.Increment(ref _msg_id);
                msg.msg_id &= 0x7fffffff;
                msg.Channel = default(RedisChannel);
                msg.Value = default(RedisValue);
                return msg;
            }

            public void Release(TMessage msg)
            {
                if (msg == null) return;
                msg.msg_id = 0;
                msg.Channel = default(RedisChannel);
                msg.Value = default(RedisValue);
                try { msg.OnReset(); } catch { }
                lock (_pooling)
                {
                    if (_pooling.Contains(msg))
                        return;
                    else
                        _pooling.Enqueue(msg);
                }
            }


            public IEnumerable<TMessage> Publish(IDatabase db, RedisChannel channel) => Publish(db.Publish, channel);

            public IEnumerable<TMessage> Publish(ISubscriber sub, RedisChannel channel) => Publish(sub.Publish, channel);

            delegate long PublishHandler(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None);

            private IEnumerable<TMessage> Publish(PublishHandler handler, RedisChannel channel)
            {
                var msg = this.Alloc();
                try
                {
                    yield return msg;
                    string json = JsonHelper.SerializeObject(msg);
                    handler(channel, json);
                }
                finally
                {
                    this.Release(msg);
                }
            }
        }
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