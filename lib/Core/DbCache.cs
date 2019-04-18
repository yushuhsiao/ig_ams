using InnateGlory.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class DbCache
    {
        private IServiceProvider _services;
        private ILogger<DbCache> _logger;
        private IConfiguration<DbCache> _config;

        internal DbCache(IServiceProvider services)
        {
            this._services = services;
            this._logger = services.GetRequiredService<ILogger<DbCache>>();
            this._config = services.GetRequiredService<IConfiguration<DbCache>>();
            this._redis = new RedisSubscriber<UpdateMessage2>(this._logger) { GetConfiguration = this.Redis_Main };
        }

        #region Config

        //private object sync_config = new object();
        //private ISqlConfig _config2;
        //internal ISqlConfig GetConfig()
        //{
        //    lock (sync_config)
        //    {
        //        if (_config2 == null)
        //            _config2 = _services.GetSqlConfig<DbCache>();
        //        return _config2;
        //    }
        //}

        #endregion

        #region Items, Add, Get

        private SyncList<IDbCache> items = new SyncList<IDbCache>();

        internal void Add<TValue>(DbCache<TValue> item)
        {
            items.TryAdd(item);
            //lock (items1)
            //{
            //    if (items1.Contains(item))
            //        return;
            //    items1.Add(item);
            //    Interlocked.Exchange(ref _items2, items1.ToArray());
            //}
        }

        public DbCache<TValue> Get<TValue>(DbCache<TValue>.ReadDataHandler readData = null, string name = null)
        {
            return (DbCache<TValue>)items.Find(
                    x => x is DbCache<TValue>,
                    () => new DbCache<TValue>(_services, this, name) { ReadData = readData });
        }

        #endregion

        #region Global Events

        [RedisAction(Channel = _Consts.Redis.Channels.AppControl, Name = nameof(ServerCommands.PurgeCache), Instance = InstanceFlags.FromService)]
        public void PurgeCache(params string[] cacheTypes)
        {
            var list2 = items.ToArray();
            for (int i = 0, n = list2.Length; i < n; i++)
            {
                IDbCache obj = list2[i];
                if (cacheTypes.Contains(obj.Name))
                    obj.PurgeCache();
            }
            _logger.Log(LogLevel.Information, 0, "PurgeCache");
        }

        #endregion

        #region Redis Configuration

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.Main), DefaultValue(_Consts.Redis.DefaultValue)]
        //public string Redis_Main() => GetConfig().GetValue<string>();
        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.Main), DefaultValue(_Consts.Redis.DefaultValue)]
        public string Redis_Main() => _config.GetValue<string>();

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.TableVer), DefaultValue(1)]
        //public int Redis_TableVer() => GetConfig().GetValue<int>();
        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.TableVer), DefaultValue(1)]
        public int Redis_TableVer() => _config.GetValue<int>();

        #endregion

        #region redis / sql

        private object sync_redis = new object();
        private object sync_sql = new object();
        private RedisSubscriber<UpdateMessage2> _redis;
        //private ConnectionMultiplexer _mux;
        private IDatabase _db;
        //private ISubscriber _sub;

        //private ConnectionMultiplexer GetMultiplexer()
        //{
        //    if (_mux == null)
        //    {
        //        string configuration = Redis_Main();
        //        _mux = ConnectionMultiplexer.Connect(configuration);
        //    }
        //    return _mux;
        //}

        private IDatabase GetDatabase()
        {
            if (_db == null)
            {
                int dbindex = Redis_TableVer();
                //_db = GetMultiplexer().GetDatabase(dbindex);
                _db = _redis.GetDatabase(dbindex);
            }
            return _db;
        }

        private void ResetRedis(string msg, params object[] args)
        {
            _redis.Reset();
            _logger.Log(LogLevel.Error, 0, msg, args);
            _db = null;
        }

        internal bool RedisGetVersion(IDbCacheEntry entry, out long value)
        {
            lock (sync_redis)
            {
                try
                {
                    RedisValue n = this.GetDatabase().StringGet(entry.RedisKey);
                    if (n.HasValue)
                        return n.TryParse(out value);
                }
                catch (Exception ex)
                {
                    ResetRedis($"Redis StringGet : {entry.RedisKey}", ex); //_logger.LogError(ex, "Redis StringGet : {0}", entry.RedisKey);
                }
            }
            return _null.noop(false, out value);
        }

        internal bool RedisSetVersion(IDbCacheEntry entry, long? value)
        {
            lock (sync_redis)
            {
                try
                {
                    if (value.HasValue)
                        return this.GetDatabase().StringSet(entry.RedisKey, value.Value.ToString(), expiry: entry.Parent.RedisKeyExpire);
                    else
                        return this.GetDatabase().KeyDelete(entry.RedisKey);
                }
                catch (Exception ex)
                {
                    ResetRedis($"Redis StringSet : {entry.RedisKey}, {value}", ex); //_logger.LogError(ex, "Redis StringSet : {0}, {1}", entry.RedisKey, value);
                }
            }
            return false;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class UpdateMessage2 : RedisMessage
        {
            [JsonProperty]
            public string Name { get; set; }

            [JsonProperty]
            public int Index { get; set; }

            [JsonProperty]
            public long? Version { get; set; }
        }

        internal long RedisUpdateVersion(IDbCacheEntry entry, long? value)
        {
            lock (sync_redis)
            {
                try
                {
                    foreach (var m in _redis.Publish(_Consts.Redis.Channels.TableVer))
                    {
                        m.Name = entry.Parent.Name;
                        m.Index = entry.Index;
                        m.Version = value;
                    }
                }
                catch (Exception ex)
                {
                    ResetRedis($"Redis Publish: {entry.RedisKey}, {value}", ex); //_logger.LogError(ex, "Redis StringSet : {0}, {1}", entry.RedisKey, value);
                    _db = null;
                }
            }
            return 0;
        }

        internal bool SqlGetVersion(IDbCacheEntry entry, out long value) => sql_exec(false, $"exec TableVer_get @name='{sql_name(entry.Parent.Name)}', @index={entry.Index}", out value);

        internal bool SqlSetVersion(IDbCacheEntry entry, out long value) => sql_exec(true, $"exec TableVer_set @name='{sql_name(entry.Parent.Name)}', @index={entry.Index}", out value);

        private bool sql_exec(bool isWrite, string sql, out long value)
        {
            for (int r = 0; r < 3; r++)
            {
                var cn = _services.GetService<DataService>()._CoreDB_W; // GetConfig().Root.CoreDB_W;
                try
                {
                    lock (sync_sql)
                    {
                        using (SqlCmd coredb = cn.Open(_services, state: this))
                        {
                            object tmp1 = coredb.ExecuteScalar(sql, transaction: isWrite);
                            SqlTimeStamp tmp2;
                            if (SqlTimeStamp.Create(tmp1, out tmp2))
                            {
                                //log.message("TableVer", $"{RedisKey} = {(long)tmp2}");
                                value = tmp2;
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SqlCmdPooling.Release(this);
                    _logger.Log(LogLevel.Error, 0, null, ex); //_logger.LogError(ex, null);
                }
            }
            return _null.noop(false, out value);
        }

        [DebuggerStepThrough]
        private static string sql_name(string name)
        {
            if (name.Length > 30)
                return name.Substring(30);
            return name;
        }

        #endregion

        internal bool ReadData<TValue>(
            DbCache<TValue>.Entry sender,
            TValue[] values,
            out TValue[] result,
            DbCache<TValue>.ReadDataHandler readData)
        {
            try
            {
                result = null;
                IEnumerable<TValue> list = readData?.Invoke(sender, values);
                if (list != null)
                {
                    if (list is TValue[])
                        result = (TValue[])list;
                    if (list is List<TValue>)
                        result = ((List<TValue>)list).ToArray();
                    else
                    {
                        List<TValue> tmp = null;
                        foreach (var n in list)
                            _null._new(ref tmp).Add(n);
                        result = tmp?.ToArray();
                    }
                }
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, 0, null, ex); //this._logger.LogError(ex, null);
                return _null.noop(false, out result);
            }
        }
    }

    public static class DbCacheExtensions
    {
        public static DbCache<TValue> GetDbCache<TValue>(this IServiceProvider services, DbCache<TValue>.ReadDataHandler readData = null, string name = null)
            => services.GetService<DbCache>().Get(readData, name);
    }

    internal interface IDbCache
    {
        string Name { get; }
        void PurgeCache();
        TimeSpan RedisKeyExpire { get; }
    }

    internal interface IDbCacheEntry
    {
        IDbCache Parent { get; }
        int Index { get; }
        RedisKey RedisKey { get; }
    }

    public sealed class DbCache<TValue> : IDbCache
    {
        public sealed partial class Entry : IDbCacheEntry
        {
            public DbCache<TValue> Parent { get; }
            public int Index { get; }
            private RedisKey RedisKey;
            private long _version = -1;
            public long Version
            {
                get => Interlocked.Read(ref _version);
                set => Interlocked.Exchange(ref _version, value);
            }

            RedisKey IDbCacheEntry.RedisKey => RedisKey;
            IDbCache IDbCacheEntry.Parent => Parent;

            internal Entry(DbCache<TValue> parent, int index)
            {
                this.Parent = parent;
                this.Index = index;
                this.SetName();
            }

            private TValue[] _values;

            //private ReaderWriterLockSlim sync1 = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            private TimeCounter timer1 = new TimeCounter(false);    // timeout
            private TimeCounter timer2 = new TimeCounter(false);    // timeout for check version from redis


            //bool isBusy;
            //bool isReading;
            object _busy;
            object _reading;
            private bool IsReading
            {
                get => Interlocked.CompareExchange(ref _reading, null, null) != null;
                set => Interlocked.Exchange(ref _reading, value ? this : null);
            }

            public TValue GetFirstValue(ReadDataHandler readData = null)
            {
                TValue[] values = this.GetValues(readData);
                if (values == null) return default(TValue);
                if (values.Length == 0) return default(TValue);
                return values[0];
            }

            public TValue[] GetValues(ReadDataHandler readData = null)
            {
                if (Monitor.TryEnter(timer1) == false)
                {
                    for (; Monitor.TryEnter(timer1) == false;)
                        Thread.Sleep(10);
                    try { return this._values; }
                    finally { Monitor.Exit(timer1); }
                }

                TValue[] oldValue = this._values;
                try
                {
                    var isBusy = Interlocked.CompareExchange(ref this._busy, this, null) != null;
                    if (isBusy)
                        return oldValue;

                    try
                    {
                        //isBusy = true;

                        if (oldValue == null)
                        {
                            this._values = oldValue = new TValue[0];
                            goto _read;
                        }

                        if (timer1.IsTimeout(Parent.Timeout, reset: false))
                            goto _read;

                        if (timer2.IsTimeout(Parent.RedisInterval, false))
                        {
                            if (!Parent.Root.RedisGetVersion(this, out long version))
                                goto _read;

                            if (version != this.Version)
                                goto _read;

                            timer2.Reset();
                        }
                        return oldValue;

                    _read:
                        try
                        {
                            this.IsReading = true;
                            if (!Parent.Root.ReadData(this, oldValue, out TValue[] newValue, readData ?? Parent.ReadData/*, readData2 ?? Parent.ReadData2*/))
                                return oldValue;
                            this._values = newValue;
                            Parent.Root.SqlGetVersion(this, out long version);
                            this.Version = version;
                            Parent.Root.RedisSetVersion(this, version);
                            oldValue = oldValue ?? _null<TValue>.array;
                            for (int i = 0; i < oldValue.Length; i++)
                            {
                                TValue n = oldValue[i];
                                oldValue[i] = default(TValue);
                                if (!newValue.Contains(n))
                                    using (n as IDisposable)
                                        continue;
                            }
                            timer2.Reset();
                            timer1.Reset();
                            return newValue;
                        }
                        finally
                        {
                            this.IsReading = false;
                        }
                    }
                    finally
                    {
                        Interlocked.Exchange(ref this._busy, null);
                        isBusy = false;
                    }
                }
                finally
                {
                    Monitor.Exit(timer1);
                }
            }

            //public TValue[] GetValues2(ReadDataHandler readData = null)
            //{
            //    TValue[] oldValue;
            //    lock (timer2)
            //    {
            //        oldValue = this._values;
            //        if (oldValue == null)
            //            this._values = new TValue[0];

            //        if (isBusy)
            //            return oldValue;
            //        else
            //            isBusy = true;
            //    }
            //    try
            //    {
            //        if (oldValue == null)
            //            goto _read;

            //        if (timer1.IsTimeout(Parent.Timeout, reset: false))
            //            goto _read;

            //        if (timer2.IsTimeout(Parent.RedisInterval, false))
            //        {
            //            if (!Parent.Root.RedisGetVersion(this, out long version))
            //                goto _read;

            //            if (version != this.Version)
            //                goto _read;

            //            timer2.Reset();
            //        }
            //        return oldValue;

            //    _read:
            //        try
            //        {
            //            lock (timer2)
            //            {
            //                isReading = true;
            //            }
            //            if (Parent.Root.ReadData(this, oldValue, out TValue[] newValue, readData ?? Parent.ReadData/*, readData2 ?? Parent.ReadData2*/))
            //            {
            //                lock (timer2)
            //                {
            //                    this._values = newValue;
            //                }

            //                Parent.Root.SqlGetVersion(this, out long version);
            //                this.Version = version;
            //                Parent.Root.RedisSetVersion(this, version);
            //                oldValue = oldValue ?? _null<TValue>.array;
            //                //ReadOnlyCollection<TValue> n = newValue.AsReadOnly();
            //                for (int i = 0; i < oldValue.Length; i++)
            //                {
            //                    TValue n = oldValue[i];
            //                    oldValue[i] = default(TValue);
            //                    if (!newValue.Contains(n))
            //                        using (n as IDisposable)
            //                            continue;
            //                }
            //                timer2.Reset();
            //                timer1.Reset();
            //                return newValue;
            //            }
            //        }
            //        finally
            //        {
            //            lock (timer2)
            //            {
            //                isReading = false;
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        lock (timer2)
            //        {
            //            isBusy = false;
            //        }
            //    }
            //    return oldValue;
            //}

            void xx1() { }

            //bool _reading = false;
            //private int busy;
            //private int busy_id;

            //public TValue[] GetValues2(ReadDataHandler readData = null)
            //{
            //    int busy_id = Interlocked.Increment(ref this.busy_id);
            //    int busy;
            //    TValue[] oldValue = Interlocked.CompareExchange(ref this._values, _null<TValue>.array, null);
            //    if (oldValue == null)
            //    {
            //        Interlocked.Exchange(ref this.busy, busy_id);
            //    }
            //    else
            //    {
            //        busy = Interlocked.CompareExchange(ref this.busy, busy_id, 0);
            //        if (busy != 0)
            //            return oldValue;
            //    }
            //    busy = busy_id;

            //    try
            //    {
            //        if (oldValue == null)
            //            goto _read;

            //        if (timer1.IsTimeout(Parent.Timeout, reset: false))
            //            goto _read;

            //        if (timer2.IsTimeout(Parent.RedisInterval, false))
            //        {
            //            if (!Parent.Root.RedisGetVersion(this, out long version))
            //                goto _read;
            //            if (version != this.Version)
            //                goto _read;
            //            timer2.Reset();
            //        }
            //        return oldValue;

            //    _read:
            //        lock (timer1)
            //        {
            //            try
            //            {
            //                _reading = true;
            //                if (Parent.Root.ReadData(this, oldValue, out TValue[] newValue, readData ?? Parent.ReadData/*, readData2 ?? Parent.ReadData2*/))
            //                {
            //                    Interlocked.Exchange(ref this._values, newValue);
            //                    Parent.Root.SqlGetVersion(this, out long version);
            //                    this.Version = version;
            //                    Parent.Root.RedisSetVersion(this, version);
            //                    oldValue = oldValue ?? _null<TValue>.array;
            //                    //ReadOnlyCollection<TValue> n = newValue.AsReadOnly();
            //                    for (int i = 0; i < oldValue.Length; i++)
            //                    {
            //                        TValue n = oldValue[i];
            //                        oldValue[i] = default(TValue);
            //                        if (!newValue.Contains(n))
            //                            using (n as IDisposable)
            //                                continue;
            //                    }
            //                    timer2.Reset();
            //                    timer1.Reset();
            //                    return newValue;
            //                }
            //            }
            //            finally
            //            {
            //                _reading = false;
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        Interlocked.CompareExchange(ref this.busy, 0, busy);
            //    }
            //    return oldValue;
            //}

            public void ClearValues() => timer1.ClearTicks();

            public void UpdateVersion()
            {
                Parent.Root.SqlSetVersion(this, out long ver);
                if (!this.IsReading)
                {
                    this.Version = 0;
                    Parent.Root.RedisSetVersion(this, null);
                    timer2.ClearTicks();
                    timer1.ClearTicks();
                }
            }

            //public SqlConfig SqlConfig() => Parent.Root.GetConfig().Root;
        }

        public DbCache Root { get; }
        public Entry Default => this[0];
        private ILogger _logger;

        internal DbCache(IServiceProvider services, DbCache dbCache, string name)
        {
            this._logger = services.GetRequiredService<ILogger<DbCache<TValue>>>();
            this.Root = dbCache;
            this.Root.Add(this);
            Interlocked.Exchange(ref this._entrys2, new Entry[] { new Entry(this, 0) });
            this.SetName(name);
        }

        private readonly object _sync = new object();
        private Entry[] _entrys2;
        public Entry this[int index]
        {
            get
            {
                var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
                for (int i = 0, n = entrys.Length; i < n; i++)
                {
                    var entry = entrys[i];
                    if (entry.Index == index)
                        return entry;
                }
                lock (_sync)
                {
                    var entry = new Entry(this, index);
                    Interlocked.Exchange(ref this._entrys2, entrys.Add(entry));
                    return entry;
                }
                //if (index == 0) return this.Default;
                //return _entrys.GetValue(index, () => new Entry((TDbCache)this, index), true);
                //lock (_entrys)
                //    if (_entrys.TryGetValue(index, out Entry result))
                //        return result;
                //    else
                //        return _entrys[index] = new Entry(this, index);
            }   //
        }

        private string _name;
        public string Name => _name;

        #region SetName

        private void SetName(string value)
        {
            this._name = value.Trim(true) ?? TableName<TValue>.Value.Trim(true) ?? typeof(TValue).Name;
            var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
            for (int i = 0, n = entrys.Length; i < n; i++)
                entrys[i].SetName();
        }
        partial class Entry
        {
            internal void SetName()
            {
                this.RedisKey = $"{Parent.Name}.{Index}";
            }
        }

        #endregion

        private double _Timeout = TimeSpan.FromMinutes(30).TotalMilliseconds;
        public double Timeout
        {
            get => Interlocked.CompareExchange(ref _Timeout, 0, 0).Max(600000);
            set => Interlocked.Exchange(ref _Timeout, value);
        }

        private double _RedisInterval = 1000;
        public double RedisInterval
        {
            get => Interlocked.CompareExchange(ref _RedisInterval, 0, 0).Max(500);
            set => Interlocked.Exchange(ref _RedisInterval, value);
        }

        private Tuple<TimeSpan> _RedisKeyExpire = Tuple.Create(TimeSpan.FromMinutes(10));
        public TimeSpan RedisKeyExpire
        {
            get => Interlocked.CompareExchange(ref _RedisKeyExpire, null, null).Item1;
            set => Interlocked.Exchange(ref _RedisKeyExpire, Tuple.Create(value));
        }

        //public SqlConfig SqlConfig() => _dbCache.GetConfig().Root;

        public delegate IEnumerable<TValue> ReadDataHandler(Entry sender, TValue[] oldValue);

        public ReadDataHandler ReadData { get; set; }
        public event ReadDataHandler ReadDataEvent
        {
            add => ReadData = value;
            remove => ReadData = null;
        }

        public void PurgeCache()
        {
            var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
            for (int i = 0, n = entrys.Length; i < n; i++)
                entrys[i].ClearValues();
        }

        [DebuggerStepThrough]
        public TValue GetFirstValue(ReadDataHandler readData = null, int index = 0) => this[index].GetFirstValue(readData);

        [DebuggerStepThrough]
        public TValue[] GetValues(ReadDataHandler readData = null, int index = 0) => this[index].GetValues(readData);

        [DebuggerStepThrough]
        public void ClearValues(int index = 0) => this[index].ClearValues();

        [DebuggerStepThrough]
        public void UpdateVersion(int index = 0) => this[index].UpdateVersion();

        //#region util

        //[DebuggerStepThrough]
        //void writelog(string message, params object[] args)
        //{
        //    _logger.LogInformation(message, args);
        //}

        //[DebuggerStepThrough]
        //static object[] _args(object arg)
        //{
        //    if (arg == null) return _null.objects;
        //    return new object[] { arg };
        //}

        //#endregion
    }

    //public sealed class DbCache<TValue> : DbCache<DbCache<TValue>, TValue>
    //{
    //    public DbCache(DbCache provider) : base(provider) { }
    //    protected override IEnumerable<TValue> OnReadData(Entry sender, List<TValue> values) => null;
    //}
}