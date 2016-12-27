using StackExchange.Redis;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    public abstract partial class TableVer<T, TObj>
        where T : TableVer<T, TObj>, new()
        where TObj : class, new()
    {
        const string sqlstr1 = "select v from {Table} nolock where n='{Name}' and i={Index}";
        const string sqlstr2 = @"
if exists (" + sqlstr1 + @")
	update {Table} set t=getdate() where n='{Name}' and i={Index}
else
	insert into {Table} (n, i) values ('{Name}', {Index})
" + sqlstr1;

        public readonly string Table;
        public readonly string Name;
        public readonly int Index;
        public readonly string redis_key;
        private long _version;
        private object _reload;
        private readonly object _sync = new object();
		//private int _busy;
        protected internal static readonly T Instance = new T();

		protected TableVer(string table, string name, int index)
        {
            this.Table = table ?? "TableVer";
            this.Name = name;
            this.Index = index;
            this.redis_key = "{Table}.{Name}.{Index}".FormatWith(this);
            Interlocked.Exchange(ref this._reload, this);
        }

        TObj _value = new TObj();
        public TObj Value
        {
            get { return Interlocked.CompareExchange(ref this._value, null, null); }
        }

		public static T GetInstance()
		{
			Instance.CheckUpdate();
			return Instance;
		}

		long last_check;
		static readonly long check_interval = new TimeSpan(0, 0, 0, 0, 1).Ticks;
		void CheckUpdate()
		{
			_HttpContext context = _HttpContext.Current;
            ConnectionMultiplexer redis = null;
            IDisposable _redis = null;
			bool update = true;
			long ver2;
			try
			{
				Monitor.Enter(this._sync);
				//if (Interlocked.Increment(ref this._busy) > 1) return;
				// check read data
				if (Interlocked.Exchange(ref this._reload, null) == null)
				{
					long ver1 = Interlocked.Read(ref this._version);
					long t1 = DateTime.Now.Ticks;
					long t2 = Interlocked.Exchange(ref this.last_check, t1);
					long t3 = t1 - t2;
                    if (t3 < check_interval) return;
                    _redis = _HttpContext.GetRedis(out redis, redis, DB.Redis.General);
                    if (redis.GetDatabase().StringGet(this.redis_key).ToInt64(out ver2))
                        update = ver1 != ver2;
                    //if (GetRedis(context, redis, ref _redis).Strings.GET(this.redis_key).ToInt64(out ver2))
					//	update = ver1 != ver2;
				}
				// do read data
				if (update)
				{
					SqlCmd sqlcmd;
                    using (_HttpContext.GetSqlCmd(out sqlcmd, null, DB.DB01R))
                    {
                        TObj value = this.ReadData(sqlcmd);
                        if (value == null) return;
                        using (this.Value as IDisposable)
                            Interlocked.Exchange(ref this._value, value);
                        SqlTimeStamp _tmp;
                        if (SqlTimeStamp.Create(sqlcmd.ExecuteScalar(sqlstr1.FormatWith(this)), out _tmp))
                            ver2 = _tmp;
                        else
                            ver2 = 0;
                    }
                    _redis = _HttpContext.GetRedis(out redis, redis, DB.Redis.General);
                    redis.GetDatabase().StringSet(this.redis_key, ver2);
                    //redis.GetDatabase().StringSet
					//GetRedis(context, redis, ref _redis).Strings.SET(this.redis_key, ver2);
					Interlocked.Exchange(ref this._version, ver2);
				}
			}
			finally
			{
				//Interlocked.Decrement(ref this._busy);
				using (_redis) Monitor.Exit(this._sync);
			}
		}

        protected abstract TObj ReadData(SqlCmd sqlcmd);

        public static void Reset()
        {
            Interlocked.Exchange(ref Instance._reload, Instance);
        }

        public static long UpdateVersion()
        {
            SqlCmd sqlcmd;
            ConnectionMultiplexer redis;
            using (_HttpContext.GetSqlCmd(out sqlcmd, null, DB.DB01R))
            {
                try
                {
                    //object _ver = sqlcmd.ExecuteScalar(sqlcmd.Transaction == null, sqlstr2.FormatWith(Instance));
                    long ver; SqlTimeStamp _tmp;
                    if (SqlTimeStamp.Create(sqlcmd.ExecuteScalar(sqlcmd.Transaction == null, sqlstr2.FormatWith(Instance)), out _tmp))
                        ver = _tmp;
                    else
                        ver = 0;
                    using (_HttpContext.GetRedis(out redis, null, DB.Redis.General))
                        redis.GetDatabase().KeyDelete(Instance.redis_key);
                    //_HttpContext.Current.GetRedis2(null, DB.Redis.General2).Keys.DEL(Instance.redis_key);
                    return ver;
                }
                catch { return 0; }
            }
        }

    }
}