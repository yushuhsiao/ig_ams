using ams;
using ams.Data;
using GeniusBull;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace LogService
{
    static class util
    {
        internal static void TableVer_Message(DB.RedisMessage msg)
        {
            log.message("redis", $"Channel:{msg.Channel}, Name:{msg.Name}, Message:{msg.Message}");
            LogService.Instance.PurgeCache();
            lock (members1) members1.Clear();
        }

        //internal static string sn1(string serialNumber)
        //{
        //    int index = serialNumber.IndexOf('-');
        //    if (index > 0)
        //        return serialNumber.Substring(0, index);
        //    return serialNumber;
        //}
        //internal static string sn2(string serialNumber)
        //{
        //    int index = serialNumber.IndexOf('-');
        //    if (index > 0)
        //        return serialNumber.Substring(index + 1);
        //    return "0";
        //}

        [AppSetting, DefaultValue(1000)]
        public static int TopN
        {
            get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// 複製所有欄位
        /// </summary>
        //internal static SqlBuilder SqlBuilder1(SqlDataReader r)
        //{
        //    SqlBuilder sql = new SqlBuilder();
        //    for (int i = 0; i < r.FieldCount; i++)
        //    {
        //        if (r.IsDBNull(i)) continue;
        //        sql["", r.GetName(i)] = r.GetValue(i);
        //    }
        //    return sql;
        //}

        static Dictionary<int, AgentData> agents1 = new Dictionary<int, AgentData>();
        internal static AgentData GetAgent(this MemberData member)
        {
            lock (agents1)
            {
                AgentData agent;
                if (agents1.TryGetValue(member.ParentID, out agent))
                    return agent;
                return agents1[member.ParentID] = member.GetParent(util.GetSqlCmd(member.CorpInfo.DB_User01R));
            }
        }

        static Dictionary<int, Dictionary<int, MemberData>> members1 = new Dictionary<int, Dictionary<int, MemberData>>();
        internal static MemberData GetMember(this IG01PlatformInfo platform, int playerID)
        {
            MemberData member;
            lock (members1)
            {
                Dictionary<int, MemberData> members2;
                if (!members1.TryGetValue(platform.ID, out members2))
                    members1[platform.ID] = members2 = new Dictionary<int, MemberData>();
                if (members2.TryGetValue(playerID, out member))
                    return member;
                foreach (CorpInfo corp in CorpInfo.Cache.Value)
                {
                    if (corp.ID.IsRoot) continue;
                    SqlCmd userDB = util.GetSqlCmd(corp.DB_User01R);
                    var m2 = platform.GetMemberByDestID(userDB, playerID);
                    if (m2 == null) continue;
                    {
                        member = corp.GetMemberData(m2.MemberID, userDB: userDB);
                        if (member != null)
                            return members2[playerID] = member;
                    }
                }
                return /*members2[playerID] = */null;
            }
        }

        static Dictionary<Thread, Dictionary<string, SqlCmd>> sqlcmds = new Dictionary<Thread, Dictionary<string, SqlCmd>>();
        [DebuggerStepThrough]
        internal static SqlCmd GetSqlCmd(string connectionString)
        {
            Thread thread = Thread.CurrentThread;
            lock (sqlcmds)
            {
                try
                {
                    Dictionary<string, SqlCmd> dict;
                    if (!sqlcmds.TryGetValue(thread, out dict))
                        dict = sqlcmds[thread] = new Dictionary<string, SqlCmd>();
                    SqlCmd sqlcmd;
                    if (dict.TryGetValue(connectionString, out sqlcmd))
                        return sqlcmd;
                    else
                        return dict[connectionString] = new SqlCmd(connectionString);
                }
                catch
                {
                    log.message(null, $"{connectionString}");
                    throw;
                }
            }
        }
        [DebuggerStepThrough]
        internal static void ReleaseSqlCmd()
        {
            Thread thread = Thread.CurrentThread;
            lock (sqlcmds)
            {
                Dictionary<string, SqlCmd> dict;
                if (sqlcmds.TryGetValue(thread, out dict))
                {
                    foreach (SqlCmd obj in dict.Values)
                        using (obj)
                            continue;
                    dict.Clear();
                }
            }
        }

        //public static SqlCmd SqlCmd_GameReplayW
        //{
        //    [DebuggerStepThrough]
        //    get { return Thread.CurrentThread.GetSqlCmd("GameReplayW", ams.DB.GameReplayW); }
        //}
        //public static SqlCmd SqlCmd_GeniusBullLogW
        //{
        //    [DebuggerStepThrough]
        //    get { return Thread.CurrentThread.GetSqlCmd("GeniusBullLogW", ams.DB.GeniusBullLogW); }
        //}
        //public static SqlCmd SqlCmd_GeniusBullLogR
        //{
        //    [DebuggerStepThrough]
        //    get { return Thread.CurrentThread.GetSqlCmd(ams.DB.GeniusBullLogR); }
        //}
        //[DebuggerStepThrough]
        //public static SqlCmd SqlCmd_Source01(IG01PlatformInfo platform) => Thread.CurrentThread.GetSqlCmd($"Source01_{platform.ID}", platform.ApiUrl);
        //[DebuggerStepThrough]
        //public static SqlCmd SqlCmd_Source02(IG01PlatformInfo platform) => Thread.CurrentThread.GetSqlCmd($"Source02_{platform.ID}", platform.ApiUrl);
        //[DebuggerStepThrough]
        //public static SqlCmd SqlCmd_UserLog(CorpInfo corp) => Thread.CurrentThread.GetSqlCmd($"UserLog_{corp.ID}", corp.DB_Log01W);
        //[DebuggerStepThrough]
        //public static SqlCmd SqlCmd_UserData(CorpInfo corp) => Thread.CurrentThread.GetSqlCmd($"UserData_{corp.ID}", corp.DB_User01R);

        [DebuggerStepThrough]
        public static int ExecuteNonQueryNoDuplicateKey(this SqlCmd sqlcmd, string sql)
        {
            try { return sqlcmd.ExecuteNonQuery(sql); }
            catch (SqlException ex) when (ex.IsDuplicateKey()) { return 1; }
        }
        [DebuggerStepThrough]
        public static int ExecuteNonQueryNoDuplicateKey(this SqlCmd sqlcmd, bool transaction, string sql)
        {
            try { return sqlcmd.ExecuteNonQuery(transaction, sql); }
            catch (SqlException ex) when (ex.IsDuplicateKey()) { return 1; }
        }

        public static bool GetMember(this _LogBase data, IG01PlatformInfo platform, out MemberData member, out AgentData agent)
        {
            int playerID = data.GetPlayerID();
            if (playerID > 0)
            {
                member = platform.GetMember(playerID);
                agent = member?.GetAgent();
                return member != null;
            }
            return _null.noop(false, out member, out agent);
        }

        public static bool GetGameInfo(this _LogBase data, IG01PlatformInfo platform, out GameInfo gameInfo, grp_cache grps = null)
        {
            //data.platform = platform;
            int gameID = data.GetGameID(grps);
            if (gameID == 0)
                return _null.noop(false, out gameInfo);
            string originalID = gameID.ToString();
            PlatformGameInfo platformGame = platform.GetPlatformGameInfo(originalID);
            if (platformGame == null)
                return _null.noop(false, out gameInfo, () => log.message("Error", $"{TableNameAttribute.GetTableName(data.GetType())}.GameID={originalID} not define!"));
            gameInfo = GameInfo.GetGameInfo(platformGame.GameID);
            if (gameInfo == null)
                return _null.noop(false, out gameInfo, () => log.message("Error", $"GameInfo:{platformGame.GameID} not define!"));
            return true;
        }

        static object _games_sync = new object();
        static List<GeniusBull.Game> _games;
        static string _sql_games;

        public static string GameIDs(this IG01PlatformInfo platform, bool withJackpot)
        {
            lock (_games_sync)
            {
                var n1 = platform.GetGames();
                if (util._games != n1)
                {
                    List<int> gameIDs = new List<int>();
                    foreach (GeniusBull.Game game in platform.GetGames())
                        if (game.Jackpot == withJackpot)
                            gameIDs.Add(game.Id);
                    _sql_games = gameIDs.ToSqlString();
                }
                if (string.IsNullOrEmpty(_sql_games)) return null;
                return $" and GameId in {_sql_games}";
            }
        }

        //public static decimal? get(this List<JackpotUpdateLog> list, string jackpotType)
        //{
        //    JackpotUpdateLog row = null;
        //    if (list != null)
        //    {
        //        for (int i = 0, n = list.Count; i < n; i++)
        //        {
        //            JackpotUpdateLog _row = list[i];
        //            if (string.Compare(_row.JackpotType, jackpotType, true) == 0)
        //            {
        //                if (row == null)
        //                    row = _row;
        //                //else
        //                //    Debugger.Break();
        //            }
        //        }
        //    }
        //    return row?.Amount;
        //    //return list?.Find((row) => string.Compare(row.JackpotType, jackpotType, true) == 0);
        //}

        //[DebuggerStepThrough]
        //public static bool IsFinished(object data)
        //{
        //    IFinished f = data as IFinished;
        //    if (f == null) return true;
        //    return f.IsFinished;
        //}

        //[DebuggerStepThrough]
        //public static string sql_Finished(object data)
        //{
        //    IFinished f = data as IFinished;
        //    if (f == null) return null;
        //    return $" and {f.FieldName} = 1";
        //}

        //public static void SetSyncFlag<T>(this List<T> list, _Config.Item config, Sync_Flag flag) where T : _LogBase<T>, new()
        //{
        //    foreach (Action commit in config.archiveDB.BeginTran())
        //    {
        //        foreach (T n in list)
        //        {
        //            n.SetSyncFlag(config, flag);
        //        }
        //        commit();
        //    }
        //}

        [DebuggerStepThrough]
        public static string CreateTime<T>() where T : _LogBase => FieldNameAttribute.GetInstance<T>().CreateTime;
        [DebuggerStepThrough]
        public static string GroupID<T>() where T : _LogBase => FieldNameAttribute.GetInstance<T>().GroupID;
        [DebuggerStepThrough]
        public static string Finished<T>() where T : _LogBase => FieldNameAttribute.GetInstance<T>().Finished;
        [DebuggerStepThrough]
        public static string sql_Finished<T>() where T : _LogBase
        {
            string n = util.Finished<T>();
            if (string.IsNullOrEmpty(n)) return null;
            return $" and {n} = 1";
        }

        public static JackpotUpdateLog GRAND(this List<JackpotUpdateLog> list) => list.getItem(JackpotType.GRAND);
        public static JackpotUpdateLog MAJOR(this List<JackpotUpdateLog> list) => list.getItem(JackpotType.MAJOR);
        public static JackpotUpdateLog MINOR(this List<JackpotUpdateLog> list) => list.getItem(JackpotType.MINOR);
        public static JackpotUpdateLog MINI(this List<JackpotUpdateLog> list) => list.getItem(JackpotType.MINI);
        public static bool IsValidate(this List<JackpotUpdateLog> list) => (list.GRAND() != null) && (list.MAJOR() != null) && (list.MINOR() != null) && (list.MINI() != null);
        public static void SetSyncFlag(this List<JackpotUpdateLog> list, _Config.Item config, Sync_Flag? flag) => JackpotUpdateLog.SetSyncFlag(list, config, flag);
        static JackpotUpdateLog getItem(this List<JackpotUpdateLog> list, string jackpotType)
        {
            JackpotUpdateLog row = null;
            foreach (JackpotUpdateLog _row1 in list.FindEach((_row2) => string.Compare(_row2.JackpotType, jackpotType, true) == 0))
            {
                if (row == null)
                    row = _row1;
                else
                    return null;
            }
            return row;
        }
    }
    class FieldNameAttribute : Attribute
    {
        public string CreateTime { get; set; }
        public string GroupID { get; set; }
        public string Finished { get; set; }
        //public string GameReplay { get; set; }

        static Dictionary<Type, FieldNameAttribute> _dict = new Dictionary<Type, FieldNameAttribute>();

        public static FieldNameAttribute GetInstance<T>() where T : _LogBase
        {
            lock (_dict)
            {
                FieldNameAttribute ret;
                if (_dict.TryGetValue(typeof(T), out ret))
                    return ret;
                else
                    return _dict[typeof(T)] = typeof(T).GetCustomAttribute<FieldNameAttribute>() ?? new FieldNameAttribute();
            }
        }
    }
}