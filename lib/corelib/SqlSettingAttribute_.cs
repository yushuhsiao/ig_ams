using casino;
using redis;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace casino
{
    public static partial class SqlConfig
    {
		class command
		{
#pragma warning disable 649
            [api.arg]
			int? corpID;
			[api.arg]
			int? gameID;
			[api.arg]
			string key1;
			[api.arg]
			string key2;
			[api.arg]
			string value;
			[api.arg]
			string description;
#pragma warning restore 649

            SqlBuilder ToSqlBuilder()
			{
				SqlBuilder sql = new SqlBuilder();
				sql["*w", "CorpID		"] = corpID;
				sql["*w", "GameID		"] = gameID;
				sql["*w", "Key1			"] = key1;
				sql["*w", "Key2			"] = key2;
				sql[" u", "Value		"] = value;
				sql["Nu", "Description	"] = description;
				sql.TestMissingFields(true);
				return sql;
			}

			[DebuggerStepThrough, api.http("~/sys/config/get")]
			api.result get() { return Cache.row_op<Item>(this.ToSqlBuilder(), RowFlag.select); }

			[DebuggerStepThrough, api.http("~/sys/config/del")]
			api.result del() { return Cache.row_op<Item>(this.ToSqlBuilder(), RowFlag.delete); }

			[DebuggerStepThrough, api.http("~/sys/config/set")]
			api.result set() { return Cache.row_op<Item>(this.ToSqlBuilder(), RowFlag.insert | RowFlag.update); }
		}
    }
}

//namespace System.Configuration
//{
//    using item0 = Dictionary<int, Dictionary<int, Dictionary<string, Dictionary<string, string>>>>;
//    using item1 = Dictionary<int, Dictionary<string, Dictionary<string, string>>>;
//    using item2 = Dictionary<string, Dictionary<string, string>>;
//    using item3 = Dictionary<string, string>;

//    public class ConfigCache : WebTools.ObjectCache<ConfigCache, item0>
//    {
//        const double DefaultLifeTime = 60 * 60 * 1000;
//        static readonly SqlSettingAttribute cache = new SqlSettingAttribute() { CorpID = 0, GameID = 0, Key1 = "Cache", Key2 = "Config" };

//        [SqlSetting("Cache", "Lang2"), DefaultValue(DefaultLifeTime)]
//        public override double LifeTime
//        {
//            get
//            {
//                string value_str;
//                if (this.GetValue(cache, out value_str))
//                {
//                    double result;
//                    if (value_str.ToDouble(out result))
//                        return result;
//                }
//                return DefaultLifeTime;
//            }
//            set { }
//        }

//        int _check_update = 0;

//        protected override bool CheckUpdate(string key, params object[] args)
//        {
//            return (Interlocked.Exchange(ref this._check_update, 0) > 0) || base.CheckUpdate(key, args);
//        }

//        public override void Update(SqlCmd sqlcmd, string key, params object[] args)
//        {
//            using (DB.Open(DB.DB01, out sqlcmd, sqlcmd))
//            {
//                item0 n0 = new item0(); item1 n1; item2 n2; item3 n3;
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Config nolock"))
//                {
//                    int corpID = r.GetInt32("CorpID");
//                    int gameID = r.GetInt32("GameID");
//                    string key1 = r.GetString("Key1");
//                    string key2 = r.GetString("Key2");
//                    string value = r.GetString("Value");

//                    if (n0.TryGetValue(corpID, out n1) == false)
//                        n1 = n0[corpID] = new item1();
//                    if (n1.TryGetValue(gameID, out n2) == false)
//                        n2 = n1[gameID] = new item2();
//                    if (n2.TryGetValue(key1, out n3) == false)
//                        n3 = n2[key1] = new item3();
//                    n3[key2] = value;
//                }
//                base.Value = n0;
//            }
//        }

//        public bool GetValue(int corpID, int gameID, string key1, string key2, out string value)
//        {
//            item0 n0 = this.Value;
//            item1 n1;
//            item2 n2;
//            item3 n3;
//            if (n0.TryGetValue(corpID, out n1))
//                if (n1.TryGetValue(gameID, out n2))
//                    if (n2.TryGetValue(key1 ?? "", out n3))
//                        return n3.TryGetValue(key2 ?? "", out value);
//            value = null;
//            return false;
//        }
//        public bool GetValue(SqlSettingAttribute attr, out string value)
//        {
//            return this.GetValue(attr.CorpID, attr.GameID, attr.Key1, attr.Key2, out value);
//        }

//        [api("~/sys/config_update")]
//        public static void config_update()
//        {
//            //lock (cache) if (cache.Count > 0) cache[item.sn] = item;
//        }

//        [api("~/sys/config_reload")]
//        public static void config_reload()
//        {
//            Interlocked.Increment(ref ConfigCache.Instance._check_update);
//            //lock (cache) cache.Clear();
//        }
//    }

//    public class ConfigCache : WebTools.TableVer_Cache<ConfigCache, item0>
//    {
//        public ConfigCache() : base("Config") { }

//        public const int _CorpID = 0;
//        public const int _GameID = 0;
//        public const string _Key1 = "Redis";
//        public const string _Key2 = "General";

//        protected override RedisConnection GetRedis()
//        {
//            string connectionString;
//            if (this.GetValue(ConfigCache._CorpID, ConfigCache._GameID, ConfigCache._Key1, ConfigCache._Key2, out connectionString))
//                return RedisConnection.GetConnection(null, connectionString);
//            return null;
//        }

//        public override void Update(SqlCmd sqlcmd, string key, params object[] args)
//        {
//            base.Update(sqlcmd, key, args);
//        }

//        protected override item0 ReadData(SqlCmd sqlcmd, string key, params object[] args)
//        {
//            using (DB.Open(DB.DB01R, out sqlcmd, sqlcmd))
//            {
//                item0 n0 = new item0(); item1 n1; item2 n2; item3 n3;
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Config nolock"))
//                {
//                    int corpID = r.GetInt32("CorpID");
//                    int gameID = r.GetInt32("GameID");
//                    string key1 = r.GetString("Key1");
//                    string key2 = r.GetString("Key2");
//                    string value = r.GetString("Value");

//                    if (n0.TryGetValue(corpID, out n1) == false)
//                        n1 = n0[corpID] = new item1();
//                    if (n1.TryGetValue(gameID, out n2) == false)
//                        n2 = n1[gameID] = new item2();
//                    if (n2.TryGetValue(key1, out n3) == false)
//                        n3 = n2[key1] = new item3();
//                    n3[key2] = value;
//                }
//                return n0;
//            }
//        }

//        public bool GetValue(int corpID, int gameID, string key1, string key2, out string value)
//        {
//            item0 n0 = this.Value; item1 n1; item2 n2; item3 n3;
//            if (n0.TryGetValue(corpID, out n1))
//                if (n1.TryGetValue(gameID, out n2))
//                    if (n2.TryGetValue(key1 ?? "", out n3))
//                        return n3.TryGetValue(key2 ?? "", out value);
//            value = null;
//            return false;
//        }
//        public bool GetValue(SqlSettingAttribute attr, out string value)
//        {
//            if (attr == null) { value = null; return false; }
//            return this.GetValue(attr.CorpID, attr.GameID, attr.Key1, attr.Key2, out value);
//        }
//    }


//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class ConfigItem
//    {
//        [DbImport]
//        public int sn;
//        [DbImport]
//        public int CorpID;
//        [DbImport]
//        public int GameID;
//        [DbImport]
//        public string Key1;
//        [DbImport]
//        public string Key2;
//        [DbImport]
//        public string Value;
//    }
//}
