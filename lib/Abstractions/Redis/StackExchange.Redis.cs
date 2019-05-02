using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchange.Redis
{
    public static class RedisExtensions
    {
        public static IEnumerable<string> Keys(this IDatabase db, string pattern)
        {
            var s1 = db.Multiplexer.GetEndPoints();
            if (s1.Length == 0)
                yield break;
            var s2 = db.Multiplexer.GetServer(s1[0]);
            for (int pageOffset = 0, pageSize = 100; ; pageOffset++)
            {
                int count = 0;
                foreach (var n in s2.Keys(db.Database, pattern, pageSize: pageSize, pageOffset: pageOffset))
                {
                    count++;
                    yield return n;
                }
                if (count < pageSize)
                    break;
            }
        }
        //[DebuggerStepThrough]
        //public static void Subscribe(this ISubscriber subscriber, RedisChannel channel, Action<RedisMessage> handler, CommandFlags flags = CommandFlags.None)
        //{
        //    subscriber?.Subscribe(channel, (c, v) => handler(new RedisMessage(c, v)), flags);
        //}
        //[DebuggerStepThrough]
        //public static async Task SubscribeAsync(this ISubscriber subscriber, RedisChannel channel, Action<RedisMessage> handler, CommandFlags flags = CommandFlags.None)
        //{
        //    await subscriber?.SubscribeAsync(channel, (c, v) => handler(new RedisMessage(c, v)), flags);
        //}

        //public static RedisSubscriber Subscribe(this ISubscriber subscriber, RedisChannel channel, CommandFlags flags = CommandFlags.None)
        //    => RedisSubscriber.Create(subscriber, channel, flags);
        //public static async Task<RedisSubscriber> SubscribeAsync(this ISubscriber subscriber, RedisChannel channel, CommandFlags flags = CommandFlags.None)
        //    => await RedisSubscriber.CreateAsync(subscriber, channel, flags);
    }

    //public class RedisSubscriber : IRedis, IRedisAsync
    //{
    //    public static RedisSubscriber Create(ISubscriber subscriber, RedisChannel channel, CommandFlags flags = CommandFlags.None)
    //    {
    //        RedisSubscriber n = new RedisSubscriber(subscriber);
    //        subscriber.Subscribe(channel, n._Handler, flags);
    //        return n;
    //    }
    //    public async static Task<RedisSubscriber> CreateAsync(ISubscriber subscriber, RedisChannel channel, CommandFlags flags = CommandFlags.None)
    //    {
    //        RedisSubscriber n = new RedisSubscriber(subscriber);
    //        await subscriber.SubscribeAsync(channel, n._Handler, flags);
    //        return await Task.FromResult(n);
    //    }

    //    public ISubscriber Subscriber { get; }

    //    private RedisSubscriber(ISubscriber subscriber)
    //    {
    //        this.Subscriber = subscriber;
    //    }

    //    private Queue<Message> _messages = new Queue<Message>();

    //    private void _Handler(RedisChannel channel, RedisValue value) => _messages.Enqueue(new Message(channel, value), true);

    //    public bool HasMessage
    //    {
    //        get
    //        {
    //            if (Monitor.TryEnter(_messages))
    //            {
    //                try { return _messages.Count > 0; }
    //                finally { Monitor.Exit(_messages); }
    //            }
    //            return false;
    //        }
    //    }

    //    public async Task<Message> GetMessageAsync()
    //    {
    //        Message message;
    //        while (!this.GetMessage(out message))
    //            await Task.Delay(1);
    //        return await Task.FromResult(message);
    //    }

    //    public bool GetMessage(out Message message)
    //    {
    //        if (Monitor.TryEnter(_messages))
    //        {
    //            try
    //            {
    //                if (_messages.Count > 0)
    //                {
    //                    message = _messages.Dequeue();
    //                    return true;
    //                }
    //            }
    //            finally { Monitor.Exit(_messages); }
    //        }
    //        message = default(Message);
    //        return false;
    //    }

    //    #region IRedis, IRedisAsync

    //    TimeSpan IRedis.Ping(CommandFlags flags) => Subscriber.Ping(flags);

    //    public ConnectionMultiplexer Multiplexer => Subscriber?.Multiplexer;

    //    async Task<TimeSpan> IRedisAsync.PingAsync(CommandFlags flags) => await Subscriber.PingAsync(flags);

    //    bool IRedisAsync.TryWait(Task task) => Subscriber.TryWait(task);

    //    void IRedisAsync.Wait(Task task) => Subscriber.Wait(task);

    //    T IRedisAsync.Wait<T>(Task<T> task) => Subscriber.Wait(task);

    //    void IRedisAsync.WaitAll(params Task[] tasks) => Subscriber.WaitAll(tasks);

    //    #endregion

    //    [System.Diagnostics.DebuggerDisplay("Channel:{Channel}, Value:{Value}")]
    //    public struct Message
    //    {
    //        public RedisChannel Channel { get; set; }
    //        public RedisValue Value { get; set; }

    //        public Message(RedisChannel channel, RedisValue value)
    //        {
    //            this.Channel = channel;
    //            this.Value = value;
    //        }
    //    }
    //}

    //[System.Diagnostics.DebuggerDisplay("Channel:{Channel}, Value:{Value}")]
    //public struct RedisMessage
    //{
    //    public RedisChannel Channel { get; set; }
    //    public RedisValue Value { get; set; }

    //    public RedisMessage(RedisChannel channel, RedisValue value)
    //    {
    //        this.Channel = channel;
    //        this.Value = value;
    //    }
    //}
}