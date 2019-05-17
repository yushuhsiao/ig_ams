using InnateGlory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchange.Redis
{
    public class RedisSubscriber<TMessage> : IRedis
        where TMessage : RedisMessage, new()
    {
        public delegate void MessageHandler(RedisSubscriber<TMessage> sender, TMessage message);

        private object _sync;
        private ILogger _logger;
        private ISubscriber _subscriber;
        private List<_Channel> _channels1 = new List<_Channel>();
        private _Channel[] _channels2;
        private TimeCounter _timer = new TimeCounter(false);
        private RedisMessage.Pool<TMessage> _msg_pool = new RedisMessage.Pool<TMessage>();
        public TimeCounter Timer => _timer;

        public RedisSubscriber(ILogger logger)
        {
            _sync = _channels1;
            _logger = logger;
        }

        public Func<string> GetConfiguration { get; set; }

        private _Channel[] GetChannels()
        {
            _Channel[] result = Interlocked.CompareExchange(ref _channels2, null, null);
            if (result == null)
            {
                lock (_channels1)
                {
                    result = _channels1.ToArray();
                    Interlocked.Exchange(ref _channels2, result);
                }
            }
            return result;
        }

        ConnectionMultiplexer IRedisAsync.Multiplexer => _subscriber?.Multiplexer;

        public bool IsConected => _subscriber?.Multiplexer.IsConnected ?? false;

        public IDatabase GetDatabase(int db = -1, object asyncState = null) => GetSubscriber().Multiplexer.GetDatabase(db, asyncState);


        public void WatchDog(double timeout)
        {
            if (this.Timer.IsTimeout(timeout, true) || this.IsConected == false)
            {
                this.GetSubscriber(true);
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                using (_subscriber?.Multiplexer)
                {
                    _subscriber = null;
                    if (_subscriber != null)
                    {
                        _logger.Log(LogLevel.Information, 0, $"Reset Multiplexer : {_subscriber.Multiplexer.Configuration}..."); //_logger.LogInformation($"Multiplexer : {configuration}...");
                    }
                }
            }
        }

        public ISubscriber GetSubscriber(bool reset = false)
        {
            lock (_sync)
            {
                if (reset)
                    Reset();
                if (_subscriber == null)
                {
                    string configuration = GetConfiguration();
                    if (string.IsNullOrEmpty(configuration))
                        _logger.Log(LogLevel.Warning, 0, $"Multiplexer : Configuration is empty"); //_logger.LogWarning($"Multiplexer : Configuration is empty");
                    else
                    {
                        _logger.Log(LogLevel.Information, 0, $"Multiplexer : {configuration}..."); //_logger.LogInformation($"Multiplexer : {configuration}...");
                        var mux = ConnectionMultiplexer.Connect(configuration);
                        this.Timer.Reset();
                        _subscriber = mux.GetSubscriber();
                        Resubscribe(_subscriber);
                    }
                }
                return _subscriber;
            }
        }

        public void Subscribe(RedisChannel channel, MessageHandler handler, CommandFlags flags = CommandFlags.None)
        {
            bool isNewChannel = true;
            lock (_sync)
            {
                foreach (var c in _channels1)
                {
                    if (c.Name != channel)
                    {
                        isNewChannel = false;

                        if (c.Handler == handler)
                            return;
                    }
                }
                _channels1.Add(new _Channel() { Name = channel, Handler = handler });
                Interlocked.Exchange(ref _channels2, null);
            }
            if (isNewChannel)
            {
                GetSubscriber().Subscribe(channel, OnMessage, flags);
                this.Timer.Reset();
            }
        }

        public void Unsubscribe(RedisChannel channel)
        {
            lock (_sync)
            {
                _channels1.RemoveWhen(n => n.Name == channel);
                Interlocked.Exchange(ref _channels2, null);
            }
            _subscriber.Unsubscribe(channel);
        }

        public void Unsubscribe(MessageHandler handler)
        {
            lock (_sync)
            {
                _channels1.RemoveWhen(n => n.Handler == handler);
                Interlocked.Exchange(ref _channels2, null);
            }
        }

        private void Resubscribe(ISubscriber subscriber)
        {
            List<RedisChannel> ch = null;
            foreach (var n in GetChannels())
            {
                if (ch == null)
                    ch = new List<RedisChannel>();
                if (!ch.Contains(n.Name))
                    ch.Add(n.Name);
            }
            if (ch == null) return;

            foreach (var channel in ch)
            {
                try { subscriber.Subscribe(channel, OnMessage); }
                catch { }
            }
        }

        private void OnMessage(RedisChannel channel, RedisValue value)
        {
            TMessage msg = _msg_pool.Alloc();
            try
            {
                this.Timer.Reset();
                int count = 0;
                JsonHelper.PopulateObject(value, msg);
                msg.Channel = channel;
                msg.Value = value;
                foreach (var n in GetChannels())
                {
                    try
                    {
                        count++;
                        n.Handler(this, msg);
                    }
                    catch { }
                }
                if (count == 0)
                {
                    try { GetSubscriber().Unsubscribe(channel); }
                    catch { }
                }
            }
            catch { }
            finally
            {
                _msg_pool.Release(msg);
            }
        }

        public IEnumerable<TMessage> Publish(RedisChannel channel) => _msg_pool.Publish(GetSubscriber(), channel);

        private class _Channel
        {
            public RedisChannel Name { get; set; }
            public MessageHandler Handler { get; set; }
        }


        #region IRedis, IRedisAsync

        TimeSpan IRedis.Ping(CommandFlags flags) => _subscriber.Ping(flags);

        Task<TimeSpan> IRedisAsync.PingAsync(CommandFlags flags) => _subscriber.PingAsync(flags);

        bool IRedisAsync.TryWait(Task task) => _subscriber.TryWait(task);

        void IRedisAsync.Wait(Task task) => _subscriber.Wait(task);

        T IRedisAsync.Wait<T>(Task<T> task) => _subscriber.Wait(task);

        void IRedisAsync.WaitAll(params Task[] tasks) => _subscriber.WaitAll(tasks);

        #endregion
    }

    //public class RedisSubscriberEx<TMessage> : IRedis, IRedisAsync where TMessage : RedisMessageEx, new()
    //{
    //    public delegate void MessageHandler(RedisSubscriberEx<TMessage> sender, TMessage message);
    //    private ILogger _logger;
    //    private ISubscriber _subscriber;
    //    private _Queue _recvQueue;
    //    private _Queue _sendQueue;
    //    private List<_Channel> _channels1 = new List<_Channel>();
    //    private _Channel[] _channels2;
    //    private Dictionary<int, IDatabase> db = new Dictionary<int, IDatabase>();

    //    public RedisSubscriberEx(ILogger logger)
    //    {
    //        _logger = logger;
    //        _recvQueue = new _Queue(RecvProc);
    //        _sendQueue = new _Queue(SendProc);
    //    }

    //    private _Channel[] GetChannels()
    //    {
    //        var n = Interlocked.CompareExchange(ref _channels2, null, null);
    //        if (n == null)
    //            lock (_channels1)
    //                Interlocked.Exchange(ref _channels2, n = _channels1.ToArray());
    //        return n;
    //    }
    //    private ISubscriber Subscriber => Interlocked.CompareExchange(ref this._subscriber, null, null);

    //    public Func<string> GetConfiguration { get; set; }
    //    private readonly object _sync_connect = new object();
    //    public ConnectionMultiplexer Connect(string configuration = null)
    //    {
    //        lock (_sync_connect)
    //        {
    //            try
    //            {
    //                configuration = configuration ?? GetConfiguration?.Invoke();
    //                if (string.IsNullOrEmpty(configuration))
    //                    _logger.Log(LogLevel.Warning, 0, $"Multiplexer : Configuration is empty"); //_logger.LogWarning($"Multiplexer : Configuration is empty");
    //                else
    //                {
    //                    _logger.Log(LogLevel.Information, 0, $"Multiplexer : {configuration}..."); //_logger.LogInformation($"Multiplexer : {configuration}...");
    //                    return this.Multiplexer = ConnectionMultiplexer.Connect(configuration);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.Log(LogLevel.Error, 0, "Multiplexer", ex); //_logger.LogError(ex, "Multiplexer");
    //            }
    //        }
    //        return null;
    //    }

    //    public ConnectionMultiplexer Multiplexer
    //    {
    //        get
    //        {
    //            try { return Subscriber.Multiplexer; }
    //            catch
    //            {
    //                using (Subscriber?.Multiplexer)
    //                    Interlocked.Exchange(ref _subscriber, null);
    //                return null;
    //            }
    //        }
    //        set
    //        {
    //            lock (db)
    //                db.Clear();
    //            lock (_channels1)
    //            {
    //                var subscriber = value?.GetSubscriber();
    //                using (Interlocked.Exchange(ref this._subscriber, subscriber)?.Multiplexer)
    //                    foreach (var ch in _channels1)
    //                        subscriber?.Subscribe(ch.Name, RecvMessage);
    //            }
    //        }
    //    }

    //    public IDatabase GetDatabase(int index, string configuration = null)
    //    {
    //        lock (this.db)
    //        {
    //            if (this.db.TryGetValue(index, out IDatabase db))
    //                return db;
    //            IDatabase tmp =
    //                Multiplexer?.GetDatabase(index) ??
    //                Connect(configuration)?.GetDatabase(index);
    //            if (tmp == null)
    //                return null;
    //            return this.db[index] = new RedisDatabaseWithLock(tmp);
    //        }
    //    }

    //    public void Subscribe(RedisChannel channel, MessageHandler handler, CommandFlags flags = CommandFlags.None)
    //    {
    //        lock (_channels1)
    //        {
    //            var n = _channels1.Find(c => c.Name == channel && c.Handler == handler);
    //            if (n == null)
    //            {
    //                _channels1.Add(new _Channel() { Name = channel, Handler = handler });
    //                Interlocked.Exchange(ref _channels2, null);
    //                Subscriber?.Subscribe(channel, RecvMessage, flags: flags);
    //            }
    //        }
    //    }

    //    public void Unsubscribe(RedisChannel channel, CommandFlags flags = CommandFlags.None)
    //    {
    //        lock (_channels1)
    //        {
    //            for (int i = _channels1.Count - 1; i >= 0; i--)
    //                if (_channels1[i].Name == channel)
    //                    _channels1.RemoveAt(i);
    //            Interlocked.Exchange(ref _channels2, null);
    //            Subscriber?.Unsubscribe(channel, flags: flags);
    //        }
    //    }

    //    public IEnumerable<TMessage> SendMessage(RedisChannel channel)
    //    {
    //        var msg = _sendQueue.Alloc();
    //        msg.Channel = channel;
    //        yield return msg;
    //        _sendQueue.Enqueue(msg);
    //    }

    //    private void RecvMessage(RedisChannel channel, RedisValue value)
    //    {
    //        var msg = _recvQueue.Alloc();
    //        msg.Channel = channel;
    //        msg.Value = value;
    //        _recvQueue.Enqueue(msg);
    //    }

    //    private bool SendProc(TMessage msg)
    //    {
    //        var subscriber = this.Subscriber;
    //        if (subscriber == null)
    //            return false;
    //        try
    //        {
    //            msg.Value = JsonHelper.SerializeObject(msg);
    //            TimeSpan t = DateTime.Now - msg.Time;
    //            _logger.Log(LogLevel.Information, 0, $"SendMessage : {(int)t.TotalMilliseconds}ms, {msg.Channel} {msg.Value}"); //_logger.LogInformation($"SendMessage : {(int)t.TotalMilliseconds}ms, {msg.Channel} {msg.Value}");
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, null);
    //            return true;
    //        }
    //        try
    //        {
    //            subscriber.Publish(msg.Channel, msg.Value);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, null);
    //            this.Multiplexer = null;
    //            return true;
    //        }
    //        return true;
    //    }

    //    private bool RecvProc(TMessage msg)
    //    {
    //        TimeSpan t = DateTime.Now - msg.Time;
    //        _logger.Log(LogLevel.Information, 0, $"RecvMessage : {(int)t.TotalMilliseconds}ms, {msg.Channel} {msg.Value}"); //_logger.LogInformation($"RecvMessage : {(int)t.TotalMilliseconds}ms, {msg.Channel} {msg.Value}");
    //        try { JsonHelper.PopulateObject(msg.Value, msg); }
    //        catch (Exception ex)
    //        {
    //            _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, "Deserialize Json");
    //        }
    //        foreach (var ch in this.GetChannels())
    //            if (ch.Name == msg.Channel)
    //                ch.Handler(this, msg);
    //        return true;
    //    }

    //    class _Channel
    //    {
    //        public RedisChannel Name { get; set; }
    //        public MessageHandler Handler { get; set; }
    //    }

    //    class _Queue
    //    {
    //        private int msg_id;
    //        private Func<TMessage, bool> _handler;
    //        private Queue<TMessage> _pooling = new Queue<TMessage>();
    //        private List<TMessage> _queue = new List<TMessage>();
    //        public _Queue(Func<TMessage, bool> handler)
    //        {
    //            this._handler = handler;
    //            Tick.OnTick += this.OnTick;
    //        }

    //        [Tick(MaxThread = 1)]
    //        private bool OnTick()
    //        {
    //            bool _lock = false;
    //            try
    //            {
    //                for (; ; )
    //                {
    //                    if (!_Monitor.TryEnterN(this, ref _lock))
    //                        break;
    //                    if (_queue.Count == 0)
    //                        break;
    //                    TMessage msg = _queue[0];
    //                    if (msg.msg_id == 0)
    //                    {
    //                        _queue.RemoveAt(0);
    //                        _pooling.Enqueue(msg);
    //                    }
    //                    else
    //                    {
    //                        _Monitor.ExitN(this, ref _lock);
    //                        if (_handler(msg))
    //                        {
    //                            using (msg)
    //                                continue;
    //                        }
    //                    }

    //                }
    //            }
    //            finally
    //            {
    //                _Monitor.ExitN(this, ref _lock);
    //            }
    //            return true;
    //        }

    //        public TMessage Alloc()
    //        {
    //            DateTime time = DateTime.Now;
    //            TMessage msg;
    //            lock (this)
    //                if (_pooling.Count > 0)
    //                    msg = _pooling.Dequeue();
    //                else
    //                    msg = new TMessage();
    //            msg.Time = time;
    //            int msg_id = Interlocked.Increment(ref this.msg_id);
    //            msg_id &= 0x7fffffff;
    //            msg.Init(msg_id);
    //            return msg;
    //        }

    //        public void Enqueue(TMessage msg)
    //        {
    //            lock (this)
    //                _queue.Add(msg);
    //        }
    //    }

    //    #region IRedis, IRedisAsync

    //    public TimeSpan Ping(CommandFlags flags = CommandFlags.None) => Subscriber.Ping(flags);

    //    public async Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None) => await Subscriber.PingAsync(flags);

    //    public bool TryWait(Task task) => Subscriber.TryWait(task);

    //    public void Wait(Task task) => Subscriber.Wait(task);

    //    public T Wait<T>(Task<T> task) => Subscriber.Wait(task);

    //    public void WaitAll(params Task[] tasks) => Subscriber.WaitAll(tasks);

    //    #endregion
    //}
}