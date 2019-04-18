using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchange.Redis
{
    public delegate void _RedisMessageHandler(_RedisSubscriber sender, DateTime time, RedisChannel channel, RedisValue value);
    public class _RedisSubscriber : IRedis, IRedisAsync
    {
        private ISubscriber _subscriber;
        private ISubscriber subscriber => Interlocked.CompareExchange(ref this._subscriber, null, null);

        #region IRedis, IRedisAsync

        public ConnectionMultiplexer Multiplexer => subscriber.Multiplexer;

        public TimeSpan Ping(CommandFlags flags = CommandFlags.None) => subscriber.Ping(flags);

        public async Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None) => await subscriber.PingAsync(flags);

        public bool TryWait(Task task) => subscriber.TryWait(task);

        public void Wait(Task task) => subscriber.Wait(task);

        public T Wait<T>(Task<T> task) => subscriber.Wait(task);

        public void WaitAll(params Task[] tasks) => subscriber.WaitAll(tasks);

        #endregion

        public _RedisSubscriber() { }
        public _RedisSubscriber(RedisChannel channel, _RedisMessageHandler handler)
        {
            this.Subscribe(channel);
            this.Message += handler;
        }

        private List<RedisChannel> _channels = new List<RedisChannel>();
        private List<_RedisMessageHandler> _message1 = new List<_RedisMessageHandler>();
        private _RedisMessageHandler[] _message2 = _null<_RedisMessageHandler>.array;

        public event _RedisMessageHandler Message
        {
            add
            {
                lock (_message1)
                {
                    _message1.Add(value);
                    Interlocked.Exchange(ref _message2, _message1.ToArray());
                }
            }
            remove
            {
                lock (_message1)
                {
                    _message1.Remove(value);
                    Interlocked.Exchange(ref _message2, _message1.ToArray());
                }
            }
        }

        private void _Message(RedisChannel channel, RedisValue value)
        {
            DateTime t = DateTime.Now;
            foreach (var handler in Interlocked.CompareExchange(ref _message2, null, null))
                handler(this, t, channel, value);
        }

        public void SetMultiplexer(ConnectionMultiplexer multiplexer)
        {
            lock (_channels)
            {
                var subscriber = multiplexer.GetSubscriber();
                using (Interlocked.Exchange(ref this._subscriber, subscriber)?.Multiplexer)
                {
                    foreach (var ch in _channels)
                        subscriber.Subscribe(ch, _Message);
                }
            }
        }

        public void Subscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            lock (_channels)
            {
                if (!_channels.Contains(channel))
                {
                    _channels.Add(channel);
                    subscriber?.Subscribe(channel, _Message, flags: flags);
                }
            }
        }

        public void Unsubscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            lock (_channels)
            {
                if (_channels.Contains(channel))
                {
                    _channels.Remove(channel);
                    subscriber?.Unsubscribe(channel, flags: flags);
                }
            }
        }
    }
}