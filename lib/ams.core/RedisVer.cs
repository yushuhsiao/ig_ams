using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    public class RedisVer<TValue> where TValue : class
    {
        [DebuggerStepThrough]
        public class List : RedisVer<List<TValue>> { public List(string name, string tableVer = null, int index = 0) : base(name, tableVer, index) { } }

        [_DebuggerStepThrough]
        public class Dict
        {
            readonly Dictionary<int, RedisVer<TValue>> dict = new Dictionary<int, RedisVer<TValue>>();

            readonly string Name;
            readonly string TableVer;
            public Dict(string name, string tableVer = null)
            {
                this.Name = name;
                this.TableVer = tableVer;
            }
            public Func<SqlCmd, int, TValue> ReadData;
            public Func<TValue, int, bool> CheckUpdate;

            public RedisVer<TValue> Default
            {
                get { return this[0]; }
            }

            public RedisVer<TValue> this[int index]
            {
                get
                {
                    RedisVer<TValue> item;
                    lock (dict)
                    {
                        if (dict.TryGetValue(index, out item))
                            return item;
                        return dict[index] = new RedisVer<TValue>(this.Name, this.TableVer, index) { ReadData = ReadData, CheckUpdate = CheckUpdate };
                    }
                }
            }

            public long UpdateVersion(int index, SqlCmd sqlcmd = null, string name = null)
            {
                RedisVer<TValue> item;
                lock (dict) dict.TryGetValue(index, out item);
                return item?.UpdateVersion(sqlcmd, name) ?? 0;
            }
        }

        public RedisVer(string name, string tableVer = null, int index = 0)
        {
            this.Index = index;
            this.TableVer = tableVer.Trim(true) ?? "TableVer";
            this.Name = name.Trim(true);
            //if (index == 0)
            //    this.RedisKey = $"{TableVer}:{Name}"; // redis_key_a.FormatWith(this);
            //else
            this.RedisKey = $"{Index}:{TableVer}.{Name}"; //redis_key_b.FormatWith(this);
        }

        public readonly RedisKey RedisKey;
        public readonly string TableVer;
        public string Name;

        string sql_get_ver { get { return $"select _ver from {TableVer} nolock where _name='{Name}' and _index={Index}"; } }
        string sql_set_ver { get { return $@"if exists (select _ver from {TableVer} nolock where _name='{Name}' and _index={Index})
     update {TableVer} set _time=getdate() where _name='{Name}' and _index={Index}
else insert into {TableVer} (_name, _index) values ('{Name}', {Index})
{sql_get_ver}"; } }

        //public Func<string> RedisConfiguration = () => DB.Redis.General;
        //public Func<string> SqlReadConnectionString = () => DB.Core01R;
        //public Func<string> SqlWriteConnectionString = () => DB.Core01W;
        public Func<SqlCmd, int, TValue> ReadData;

        public readonly int Index;
        long version;
        public long Version
        {
            get { return Interlocked.Read(ref this.version); }
            private set { Interlocked.Exchange(ref this.version, value); }
        }

        //public void Purge()
        //{
        //    this.Version--;
        //}

        //public bool SkipOnSameThread = true;
        Thread last_check_thread;
        long last_check_httpcontext_timestamp;

        long last_check;
        long _CheckInterval = 10;
        public long CheckInterval
        {
            get { return Interlocked.CompareExchange(ref this._CheckInterval, 0, 0); }
            set { Interlocked.Exchange(ref this._CheckInterval, value); }
        }
        bool IsSkipCheck()
        {
            long t1, t2, t3, t4;

            long checkInterval = this.CheckInterval;
            if (checkInterval > 0)
            {
                t1 = DateTime.Now.Ticks;
                t2 = Interlocked.Exchange(ref this.last_check, t1);
                t3 = t1 - t2;
                t4 = checkInterval;
                return t3 < t4;
            }

            Thread th1 = Thread.CurrentThread;
            Thread th2 = Interlocked.Exchange(ref last_check_thread, th1);
            HttpContext context = HttpContext.Current;
            if (context != null)
                t1 = context.Timestamp.Ticks;
            else
                t1 = 0;
            t2 = Interlocked.Exchange(ref last_check_httpcontext_timestamp, t1);
            if ((th1 == th2) && (t1 == t2))
                return true;

            return false;
        }

        TValue _value;
        public TValue Value
        {
            get
            {
                _HttpContext context = _HttpContext.Current;
                if (context == null)
                    return GetValue();
                return context.GetData($"{this.GetType().Name}_{this.RedisKey}", (s) => GetValue(), false);
            }
        }

        public long GetVersion(SqlCmd sqlcmd)
        {
            using (_HttpContext.GetSqlCmd(out sqlcmd, sqlcmd, DB.Core01R))
            {
                SqlTimeStamp _tmp;
                if (SqlTimeStamp.Create(sqlcmd.ExecuteScalar(this.sql_get_ver), out _tmp))
                    return _tmp;
            }
            return 0;
        }

        TValue ReadValue(SqlCmd sqlcmd, TValue old_value)
        {
            using (_HttpContext.GetSqlCmd(out sqlcmd, null, DB.Core01R))
            {
                TValue new_value = this.ReadData(sqlcmd, this.Index);
                if (new_value == null) return old_value;
                using (old_value as IDisposable)
                {
                    long version_new = this.GetVersion(sqlcmd);
                    this.version = version_new;
                    this._value = new_value;
                    foreach (IDatabase redis in DB.Redis.GetDataBase(this, DB.Redis.General))
                        redis.StringSet(this.RedisKey, version_new);
                }
                return new_value;
            }
        }

        public Func<TValue, int, bool> CheckUpdate;

        bool busy;
        public TValue GetValue(bool check_update = true, bool force_update = false)
        {
            lock (this)
            {
                if (this._value == null) return ReadValue(null, null);
                if (busy) return this._value;
                busy = true;
                try
                {
                    if (force_update == false && CheckUpdate != null)
                        force_update = CheckUpdate(this._value, this.Index);
                    if (force_update) return ReadValue(null, null);
                    if (check_update)
                    {
                        if (this.IsSkipCheck()) return this._value;
                        foreach (IDatabase redis in DB.Redis.GetDataBase(this, DB.Redis.General))
                        {
                            long version_chk;
                            if (redis.StringGet(this.RedisKey).ToInt64(out version_chk) && (this.version == version_chk))
                                return this._value;
                        }
                        return ReadValue(null, this._value);
                    }
                    else return this._value;
                }
                finally
                {
                    busy = false;
                }
            }
        }

        public long UpdateVersion(SqlCmd sqlcmd = null, string name = null)
        {
            lock (this)
            {
                using (_HttpContext.GetSqlCmd(out sqlcmd, sqlcmd, DB.Core01W))
                {
                    try
                    {
                        long ver = 0; SqlTimeStamp _tmp;
                        if (SqlTimeStamp.Create(sqlcmd.ExecuteScalar(sqlcmd.Transaction == null, this.sql_set_ver), out _tmp))
                            ver = _tmp;
                        foreach (IDatabase redis in DB.Redis.GetDataBase(this, DB.Redis.General))
                        {
                            redis.KeyDelete(this.RedisKey);
                            DB.RedisChannels.TableVer.Publish(redis.Multiplexer, new DB.RedisMessage(name ?? this.Name, ver));
                        }
                        return ver;
                    }
                    catch { return 0; }
                }
            }
        }

    }
}