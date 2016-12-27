using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ams
{
    [_DebuggerStepThrough]
    public static class DB
    {
        public const string Key1 = "SqlConnection";
        public const string Recog_Key1 = "Recog";
        //public const string DB01R = "DB01R";
        //public const string DB01W = "DB01W";
        //public const string DB02R = "DB02R";
        //public const string DB02W = "DB02W";

        const string _Core01R = "Data Source=db01;Initial Catalog=ams_core;Persist Security Info=True;User ID=sa;Password=sa";
        const string _Core01W = "Data Source=db01;Initial Catalog=ams_core;Persist Security Info=True;User ID=sa;Password=sa";
        public const string DefaultUser01R = "Data Source=db01;Initial Catalog=ams_user;Persist Security Info=True;User ID=sa;Password=sa";
        public const string DefaultUser01W = "Data Source=db01;Initial Catalog=ams_user;Persist Security Info=True;User ID=sa;Password=sa";
        public const string DefaultLog01R = "Data Source=db01;Initial Catalog=ams_log;Persist Security Info=True;User ID=sa;Password=sa";
        public const string DefaultLog01W = "Data Source=db01;Initial Catalog=ams_log;Persist Security Info=True;User ID=sa;Password=sa";

        [ConnectionString, DefaultValue(_Core01R)]
        public static string Core01R { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(_Core01W)]
        public static string Core01W { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(@"Data Source=db01;Initial Catalog=GeniusBullLog;Persist Security Info=True;User ID=sa;Password=sa")]
        public static string GeniusBullLogR { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(@"Data Source=db01;Initial Catalog=GeniusBullLog;Persist Security Info=True;User ID=sa;Password=sa")]
        public static string GeniusBullLogW { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(@"Data Source=db01;Initial Catalog=GameReplay;Persist Security Info=True;User ID=sa;Password=sa")]
        public static string GameReplayR { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(@"Data Source=db01;Initial Catalog=GameReplay;Persist Security Info=True;User ID=sa;Password=sa")]
        public static string GameReplayW { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        //public const string Key_User01R = "User01R";
        //public const string Key_User01W = "User01W";
        //public const string Key_Log01R = "Log01R";
        //public const string Key_Log01W = "Log01W";

        //public static string User01R(UserID corpID) { return SqlConfig.Cache.Value.GetSetting(corpID, 0, Key1, Key_User01R) ?? _User01R; }
        //public static string User01W(UserID corpID) { return SqlConfig.Cache.Value.GetSetting(corpID, 0, Key1, Key_User01W) ?? _User01W; }
        //public static string Log01R(UserID corpID) { return SqlConfig.Cache.Value.GetSetting(corpID, 0, Key1, Key_Log01R) ?? _Log01R; }
        //public static string Log01W(UserID corpID) { return SqlConfig.Cache.Value.GetSetting(corpID, 0, Key1, Key_Log01W) ?? _Log01W; }

        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(_Core01R)]
        public static string LTDB01R { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        [SqlSetting(CorpID = 0, /*PlatformID = 0, */Key1 = DB.Key1), DefaultValue(_Core01R)]
        public static string LTDB01W { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        [_DebuggerStepThrough]
        public static class Redis
        {
            static object _sync = new object();
            public const int CorpID = 0;

            //public const int PlatformID = 0;
            public const string Key1 = "Redis";

            [DefaultValue(@"db01:6379,defaultDatabase=2"), SqlSetting(CorpID = DB.Redis.CorpID, /*PlatformID = DB.Redis.PlatformID, */Key1 = DB.Redis.Key1)]
            public static string UserSession
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            [DefaultValue(@"db01:6379"), SqlSetting(CorpID = DB.Redis.CorpID, /*PlatformID = DB.Redis.PlatformID, */Key1 = DB.Redis.Key1)]
            public static string Message1
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            [DefaultValue(@"db01:6379,defaultDatabase=3"), SqlSetting(CorpID = DB.Redis.CorpID, /*PlatformID = DB.Redis.PlatformID, */Key1 = DB.Redis.Key1)]
            public static string UserData
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            [DefaultValue(@"db01:6379,defaultDatabase=4"), SqlSetting(CorpID = DB.Redis.CorpID, /*PlatformID = DB.Redis.PlatformID, */Key1 = DB.Redis.Key1)]
            public static string Recog
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            static object sync_General = new object();

            [DefaultValue(@"db01:6379,defaultDatabase=1"), SqlSetting(CorpID = DB.Redis.CorpID, /*PlatformID = DB.Redis.PlatformID, */Key1 = DB.Redis.Key1)]
            public static string General
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            //static Dictionary<object, IDatabase> _redis = new Dictionary<object, IDatabase>();
            //static IDatabase GetRedis(object id, string config)
            //{
            //    if (config == null) return null;
            //    IDatabase db;
            //    lock (_redis)
            //    {
            //        id = id ?? _redis;
            //        if (_redis.TryGetValue(id, out db))
            //        {
            //            if (db.Multiplexer.Configuration == config)
            //                return db;
            //            using (db.Multiplexer)
            //                _redis.Remove(id);
            //        }
            //        try { return _redis[id] = ConnectionMultiplexer.Connect(config, new RedisLogWriter()).GetDatabase(); }
            //        catch { }
            //        while (_redis.ContainsKey(id))
            //            using (_redis[id]?.Multiplexer)
            //                _redis.Remove(id);
            //        return null;
            //    }
            //}

            //static IEnumerable<IDatabase> GetDataBase(IDatabase db) { if (db != null) lock (db) yield return db; }
            public static IEnumerable<IDatabase> GetDataBase(string config) => _redis_conn.GetDataBase2(null, config); // GetDataBase(GetRedis(_redis, config));
            public static IEnumerable<IDatabase> GetDataBase<T>(string config) => _redis_conn.GetDataBase2(typeof(T), config); // GetDataBase(GetRedis(typeof(T), config));
            public static IEnumerable<IDatabase> GetDataBase(object id, string config) => _redis_conn.GetDataBase2(id, config);// GetDataBase(GetRedis(id, config));

            public static long Publish(RedisChannel channel, RedisMessage message, CommandFlags flags)
            {
                string id = channel.ToString();
                string s = json.SerializeObject(message);
                foreach (var n in _redis_conn.GetSubscriber2(channel.ToString(), DB.Redis.General))
                    return n.Publish(channel, s, flags);
                return 0;
            }
        }

        class _redis_conn
        {
            static Dictionary<object, _redis_conn> cache = new Dictionary<object, _redis_conn>();

            static _redis_conn GetInstance(object id)
            {
                id = id ?? typeof(_redis_conn);
                _redis_conn ret;
                lock (cache)
                {
                    if (cache.TryGetValue(id, out ret))
                        return ret;
                    return cache[id] = ret = new _redis_conn();
                }
            }

            static T get_obj_1<T>(object id, string config, out _redis_conn obj, Func<ConnectionMultiplexer, T> cb) where T : class
            {
                for (int i = 0; i < 2; i++)
                {
                    obj = GetInstance(id);
                    try
                    {
                        ConnectionMultiplexer conn = obj.GetConnectionMultiplexer(config);
                        if (conn == null) continue;
                        //conn.GetSubscriber
                        T ret = cb(conn);
                        if (ret != null)
                            return ret;
                    }
                    catch { }
                    obj.RemoveConnectionMultiplexer(config);
                }
                obj = null;
                return null;
            }
            static T get_obj_1<T>(object id, string config, Func<ConnectionMultiplexer, T> cb) where T : class
            {
                _redis_conn obj; return get_obj_1<T>(id, config, out obj, cb);
            }

            static IEnumerable<T> get_obj_2<T>(object id, string config, Func<ConnectionMultiplexer, T> cb) where T : class
            {
                _redis_conn obj;
                T t = get_obj_1<T>(id, config, out obj, cb);
                if (t != null)
                    lock (obj)
                        yield return t;
            }

            static IDatabase _GetDataBase(ConnectionMultiplexer conn) => conn.GetDatabase();
            static ISubscriber _GetSubscriber(ConnectionMultiplexer conn) => conn.GetSubscriber();

            public static IDatabase GetDataBase1(object id, string config) => get_obj_1(id, config, _GetDataBase);
            public static IEnumerable<IDatabase> GetDataBase2(object id, string config) => get_obj_2(id, config, _GetDataBase);
            public static ISubscriber GetSubscriber1(object id, string config) => get_obj_1(id, config, _GetSubscriber);
            public static IEnumerable<ISubscriber> GetSubscriber2(object id, string config) => get_obj_2(id, config, _GetSubscriber);



            Dictionary<string, ConnectionMultiplexer> _conn = new Dictionary<string, ConnectionMultiplexer>();

            public ConnectionMultiplexer GetConnectionMultiplexer(string config)
            {
                ConnectionMultiplexer conn;
                lock (_conn)
                {
                    if (_conn.TryGetValue(config, out conn))
                        return conn;
                    return _conn[config] = conn = ConnectionMultiplexer.Connect(config, new RedisLogWriter());
                }
            }

            void RemoveConnectionMultiplexer(string config)
            {
                lock (_conn)
                    while (_conn.ContainsKey(config))
                        using (_conn[config])
                            _conn.Remove(config);
            }
        }

        public static class RedisChannels
        {
            public static readonly RedisChannel TableVer = "TableVer";
            //public static readonly RedisChannel LogService = "LogService";
            public static readonly RedisChannel ams = "ams";
            public static readonly RedisChannel Recog = "recog";
        }



        public static long Publish(this RedisChannel channel, ConnectionMultiplexer redis, RedisMessage message, CommandFlags flags = CommandFlags.None)
        {
            string s = json.SerializeObject(message);
            return redis.GetSubscriber().Publish(channel, s, flags);
        }

        public static void Subscribe(this RedisChannel channel, ConnectionMultiplexer redis, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
        {
            redis.GetSubscriber().Subscribe(channel, handler, flags);
        }

        public static void Subscribe(this RedisChannel channel, ConnectionMultiplexer redis, Action<RedisMessage> handler, CommandFlags flags = CommandFlags.None)
        {
            redis.GetSubscriber().Subscribe(channel, (_channel, value) =>
            {
                if (value.HasValue)
                {
                    try
                    {
                        string s = value;
                        RedisMessage msg = json.DeserializeObject<RedisMessage>(s);
                        msg.Channel = _channel;
                        (handler ?? _null.noop)(msg);
                    }
                    catch { }
                }
            }, flags);
            //redis.GetSubscriber().Subscribe(channel, handler, flags);
        }

        //public static IDisposable GetSqlCmd(out SqlCmd result, SqlCmd existing, string connectionString, params string[] connectionStrings)
        //{
        //    if ((existing != null) && (
        //        (connectionString == existing.ctorConnectionString) ||
        //        (connectionStrings.Contains(existing.ctorConnectionString))))
        //    { result = existing; return null; }
        //    _HttpContext context = _HttpContext.Current;
        //    if (context == null)
        //        return result = new SqlCmd(null, connectionString);
        //    result = context.GetSqlCmd(connectionString);
        //    return null;
        //}



        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public struct RedisMessage
        {
            public RedisChannel Channel;
            [JsonProperty]
            public string Name;
            [JsonProperty]
            public string Message;

            public RedisMessage(string name, object message)
            {
                Channel = default(RedisChannel);
                this.Name = name;
                this.Message = message.ToString();
            }
        }
    }
}