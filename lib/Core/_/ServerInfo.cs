using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.ObjectPool;
using System.IO;
using static InnateGlory.Json;
using Microsoft.Extensions.DependencyModel;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Linq;
// redis
//  1.TableVer
//  2.SysApi
//  3.keep alive

namespace InnateGlory
{
    public class ServerInfo
    {
        public Guid Id { get; } = Guid.NewGuid();
        private readonly IServiceProvider _serviceProvider;
        private readonly DbConfig _config;
        private readonly ILogger<ServerInfo> _logger;
        private RedisKey _key => $"{nameof(ServerInfo)}:{this.Id}";
        private ServerInfoData _data;
        private TimeCounter _time1 = new TimeCounter(false);
        private TimeCounter _time2 = new TimeCounter(false);
        private ISubscriber _sub = null;
        private IDatabase _db = null;
        private _Queue _SendQueue;
        private _Queue _RecvQueue;
        private ApplicationPartManager _applicationPartManager;

        [DbConfig(Key1 = "Redis", Key2 = "ServerInfo.KeepAlive"), DefaultValue(5000)]
        public double KeepAlive => this._config.GetValue<double>(this).Max(1000);

        [DbConfig(Key1 = "Redis", Key2 = "ServerInfo.Reconnect"), DefaultValue(30 * 60 * 1000)]
        public double Reconnect => this._config.GetValue<double>(this).Max(15000);

        public ServerInfo(IServiceProvider serviceProvider, DbConfig dbConfig, ILogger<ServerInfo> logger, ApplicationPartManager applicationPartManager)
        {
            this._serviceProvider = serviceProvider;
            this._config = dbConfig;
            this._logger = logger;
            this._applicationPartManager = applicationPartManager;
            this._data = new ServerInfoData(true);
            Tick.OnTick += KeepAlive_Proc;
            this._SendQueue = new _Queue(SendQueue_Proc);
            this._RecvQueue = new _Queue(RecvQueue_Proc);
        }

        public IEnumerable<ServerInfo> GetServerList()
        {
            yield break;
        }

        private void InitSubscriber()
        {
            try
            {
                lock (_time1)
                {
                    if (_time1.IsTimeout(Reconnect, true) || _sub == null)
                    {
                        _logger.LogInformation($"Subscribe : {this._config.Redis_ServerInfo}...");
                        ISubscriber sub = ConnectionMultiplexer.Connect(_config.Redis_ServerInfo).GetSubscriber();
                        sub.Subscribe("*", RecvMessage);
                        using (_sub?.Multiplexer)
                            _sub = sub;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Subscribe");
            }
        }

        private IDatabase GetDatabase()
        {
            lock (_time2)
            {
                if (_db == null)
                    _db = ConnectionMultiplexer.Connect(this._config.Redis_ServerInfo).GetDatabase();
                return _db;
            }
        }

        private void ResetDatabase(Exception ex, string message, params object[] args)
        {
            lock (_time2)
            {
                this._logger.LogError(ex, message, args);
                using (this._db?.Multiplexer)
                    this._logger.LogError(ex, null);
                this._db = null;
            }
        }

        [Tick(MaxThread = 1)]
        private bool KeepAlive_Proc()
        {
            InitSubscriber();
            if (this._time2.IsTimeout(KeepAlive, true))
            {
                try
                {
                    IDatabase db = GetDatabase();
                    lock (db) db.StringSet(this._key, this._data.JsonString, expiry: TimeSpan.FromMilliseconds(KeepAlive + 2000));
                    _logger.LogInformation($"KeepAlive : {DateTime.Now.ToString(_DateTimeExtensions.DateTimeFormatEx)}");
                }
                catch (Exception ex) { ResetDatabase(ex, "KeepAlive"); }
            }
            return true;
        }

        public void SendMessage(RedisChannel channel, object data, params Guid[] to)
        {
            _Message msg = _SendQueue.Alloc(channel, true);
            msg.from = this.Id;
            msg.to = to;
            msg.data = data;
            _SendQueue.Add(msg);
        }

        private bool SendQueue_Proc(_Message msg)
        {
            InitSubscriber();
            try
            {
                msg.Value = Json.SerializeObject(msg);
                IDatabase db = GetDatabase();
                lock (db) db.Publish(msg.Channel, msg.Value);
                TimeSpan t = DateTime.Now - msg.Time;
                _logger.LogInformation("SendMessage : {0}ms, {1}", (int)t.TotalMilliseconds, msg.Value);
                return true;
            }
            catch (Exception ex) { ResetDatabase(ex, "SendMessage"); }
            return false;
        }

        private void RecvMessage(RedisChannel channel, RedisValue value)
        {
            _Message msg = _RecvQueue.Alloc(channel, false);
            msg.Value = value;
            _RecvQueue.Add(msg);
            _time1.Reset();
        }

        private bool RecvQueue_Proc(_Message msg)
        {
            try { Json.PopulateObject(msg.Value, msg); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deserialize Json");
                return true;
            }
            object data = msg.data;
            foreach (var p in _applicationPartManager.ApplicationParts.OfType<AssemblyPart>())
            {
                foreach (var t in p.Types)
                {
                    ;
                }
            }
            return true;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        private class _Message : IDisposable
        {
            [JsonIgnore]
            public DateTime Time { get; set; }

            [JsonIgnore]
            public RedisChannel Channel { get; set; }

            [JsonIgnore]
            public RedisValue Value { get; set; }

            [JsonProperty]
            public int msg_id { get; set; }

            [JsonProperty]
            public Guid? from { get; set; }

            [JsonProperty]
            public Guid[] to { get; set; }

            [JsonProperty]
            public object data { get; set; }

            void IDisposable.Dispose()
            {
                this.Channel = default(RedisChannel);
                this.Value = default(RedisValue);
                this.msg_id = 0;
                this.from = Guid.Empty;
                this.to = null;
                this.data = null;
            }
        }

        private class _Queue
        {
            private int msg_id = 0;
            private List<_Message> _items = new List<_Message>();
            private Queue<_Message> _pooling = new Queue<_Message>();
            private Func<_Message, bool> proc;

            public _Queue(Func<_Message, bool> proc)
            {
                this.proc = proc;
                Tick.OnTick += this.Proc;
            }

            [Tick(MaxThread = 1)]
            private bool Proc()
            {
                for (; ; )
                {
                    _Message msg;
                    if (Monitor.TryEnter(this, 10))
                    {
                        try
                        {
                            if (_items.Count == 0)
                                return true;
                            msg = _items[0];
                            _items.RemoveAt(0);
                        }
                        finally { Monitor.Exit(this); }
                        bool success = proc(msg);
                        lock (this)
                        {
                            if (success) using (msg) _pooling.Enqueue(msg);
                            else if (!_items.Contains(msg))
                                _items.Insert(0, msg);
                        }
                    }
                }
            }

            public _Message Alloc(RedisChannel channel, bool msg_id)
            {
                DateTime time = DateTime.Now;
                _Message msg;
                lock (this)
                {
                    if (_pooling.Count > 0)
                        msg = _pooling.Dequeue();
                    else
                        msg = new _Message();
                }
                msg.Time = time;
                msg.Channel = channel;
                if (msg_id)
                {
                    int _msg_id = Interlocked.Increment(ref this.msg_id);
                    _msg_id &= 0x7fffffff;
                    msg.msg_id = _msg_id;
                }
                return msg;
            }

            public void Add(_Message msg)
            {
                lock (this)
                {
                    if (!_items.Contains(msg))
                        _items.Add(msg);
                }
            }
        }
    }

    class xxx
    {
    }

    public class ServerInfoData
    {
        public string Name { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        private string _json;
        [JsonIgnore]
        public string JsonString
        {
            get
            {
                string result = Interlocked.CompareExchange(ref this._json, null, null);
                if (result == null)
                {
                    result = Json.SerializeObject(this);
                    Interlocked.Exchange(ref this._json, result);
                }
                return result;
            }
        }

        public ServerInfoData(bool createInfo = false)
        {
            if (createInfo)
            {
                var asm = Assembly.GetEntryAssembly();
                Name = asm.GetName().Name;
                Product = asm.GetCustomAttribute<AssemblyProductAttribute>().Product;
                Title = asm.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            }
        }
    }

    public class ServerMessage
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbConfig _config;
        private readonly ILogger<ServerMessage> _logger;
        private readonly ApplicationPartManager _applicationPartManager;
        private readonly ServerInfo _serverInfo;
        private _Queue _SendQueue;
        private _Queue _RecvQueue;

        public ServerMessage(IServiceProvider serviceProvider, DbConfig dbConfig, ILogger<ServerMessage> logger, ApplicationPartManager applicationPartManager, ServerInfo serverInfo)
        {
            this._serviceProvider = serviceProvider;
            this._config = dbConfig;
            this._logger = logger;
            this._applicationPartManager = applicationPartManager;
            this._serverInfo = serverInfo;
            Tick.OnTick += KeepAlive_Proc;
            this._SendQueue = new _Queue(SendQueue_Proc);
            this._RecvQueue = new _Queue(RecvQueue_Proc);
        }
    }
}