using ams;
using ams.Data;
using LogService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tools;

namespace GeniusBull
{
    enum Sync_Flag
    {
        Success = 1, UserNotExist = 2, Failed = 0, Finished_Timeout = 3, Jackpot_Ignore = 4, Jackpot_Error = 5,
        PartialFailed = 6,
        /// <summary>
        /// JackpotLog 沒有對應的 JackpotUpdateLog
        /// </summary>
        Timeout_JackpotUpdateLog_NotFound = 11,
    }
    static class JackpotType
    {
        public const string GRAND = "GRAND";
        public const string MAJOR = "MAJOR";
        public const string MINOR = "MINOR";
        public const string MINI = "MINI";
    }
    interface IGameReplay { string GameReplay { get; } }
    //interface IFinished { string FieldName { get; } bool IsFinished { get; } }

    class _Config
    {
        public IG01PlatformInfo platform;

        Dictionary<Type, Item> config_items = new Dictionary<Type, Item>();

        public void Tick()
        {
            _LogBase<TwMahjongGame, TwMahjongBet>.Proc(this);
            _LogBase<DouDizhuGame, DouDizhuBet>.Proc(this);
            _LogBase<TexasGame, TexasBet>.Proc(this);
            _LogBase<RedDog>.Proc_Slot(this);
            _LogBase<Oasis>.Proc_Slot(this);
            _LogBase<SlotGameLog>.Proc_Slot(this);
            //_LogBase<GameSpin>.Proc_Slot(this);
            //_LogBase<FivePK>.Proc_Slot(this);
            _LogBase_JP<GameSpin>.Proc_Jackpot(this);
        }

        public class Item
        {
            [DbImport]
            public int PlatformID;
            [DbImport]
            public string TableName;
            [DbImport]
            public int Active;
            [DbImport]
            public double Interval;
            [DbImport]
            public double Reserved;
            [DbImport("Timeout")]
            public double SyncTimeout;
            [DbImport]
            public DateTime LastTime;

            public DateTime NewTime;

            public string NewTimeStr { get { return this.NewTime.ToString(SqlBuilder.DateTimeFormatX); } }

            public readonly TimeCounter timer = new TimeCounter(false);

            string sql_load() => $"select * from _config nolock where PlatformID={parent.platform.ID} and TableName='{_TableName}'";

            public void Update()
            {
                if (this.LastTime != this.NewTime)
                {
                    string sqlstr = $@"update _config set LastTime='{this.NewTimeStr}' where PlatformID={parent.platform.ID} and TableName='{_TableName}' {sql_load()}";
                    archiveDB.FillObject(this, true, sqlstr);
                }
            }
            public void Reload()
            {
                string sqlstr = sql_load();
                archiveDB.FillObject(this, sqlstr);
                if (!string.IsNullOrEmpty(this.TableName)) return;
                archiveDB.FillObject(this, true, $@"insert into _config (PlatformID, TableName) values ({parent.platform.ID},'{_TableName}')
{sqlstr}");
            }

            _Config parent;
            string _TableName;
            internal static Item GetConfig<T>(_Config parent)
            {
                Item value;
                Type key = typeof(T);
                lock (parent.config_items)
                {
                    if (parent.config_items.TryGetValue(key, out value))
                        return value;
                    value = parent.config_items[key] = new Item() { parent = parent, _TableName = TableName<T>.Value };
                    value.Reload();
                }
                return value;
            }


            public IG01PlatformInfo platform
            {
                [DebuggerStepThrough]
                get { return parent.platform; }
            }

            public IEnumerable<bool> Tick()
            {
                if (this.Active == 1)
                {
                    foreach (var n in this.timer.Timeout(Math.Max(1000, this.Interval)))
                        yield return true;
                    //return this.timer.TimeoutProc(Math.Max(1000, this.Interval), active);
                }
                else
                {
                    foreach (var n in this.timer.Timeout(60 * 1000))
                        yield return false;
                    //return this.timer.TimeoutProc(60 * 1000, deactive);
                }

            }

            internal SqlCmd gameDB { get { return util.GetSqlCmd(platform.ApiUrl); } }
            internal SqlCmd archiveDB { get { return util.GetSqlCmd(ams.DB.GeniusBullLogW); } }
            internal SqlCmd replayDB { get { return util.GetSqlCmd(ams.DB.GameReplayW); } }
            internal IEnumerable<Item> Lock()
            {
                if (Monitor.TryEnter(this))
                    try { yield return this; }
                    finally { Monitor.Exit(this); }
            }

            internal T _new<T>() where T : _LogBase<T>, new()
            {
                T data = new T();
                data.sql_Archive = new SqlBuilder();
                data.sql_Archive["", "PlatformID"] = this.platform.ID;
                return data;
            }
        }
    }

    public abstract class _LogBase
    {
        [DbImport]
        internal Sync_Flag? _flag;
        [DbImport]
        internal DateTime _sync1;
        [DbImport]
        public double _sync2;

        public abstract long Id
        {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }
        public abstract long GroupID
        {
            [DebuggerStepThrough]
            get;
            [DebuggerStepThrough]
            set;
        }
        internal abstract int GetGameID(grp_cache grps = null);
        public abstract string SerialNumber { get; set; }
        public virtual string sn1
        {
            get
            {
                int index = SerialNumber.IndexOf('-');
                if (index > 0)
                    return SerialNumber.Substring(0, index);
                return SerialNumber;
            }
        }
        public virtual string sn2
        {
            get
            {
                int index = SerialNumber.IndexOf('-');
                if (index > 0)
                    return SerialNumber.Substring(index + 1);
                return "0";
                //return util.sn2(this.SerialNumber);
            }
        }
        public abstract DateTime CreateTime { get; set; }
        public virtual bool IsFinished { get { return true; } }

        internal virtual int GetPlayerID() => 0;
    }

    public abstract class _LogBase<T> : _LogBase, IDbImport where T : _LogBase<T>, new()
    {
        public static T Null = new T();

        protected virtual string gameIDs(IG01PlatformInfo platform) => null;

        internal virtual void SetSyncFlag(_Config.Item config, Sync_Flag flag)
        {
            foreach (Action commit in config.archiveDB.BeginTran())
            {
                config.archiveDB.ExecuteNonQuery($"update {TableName<T>.Value} set _flag={(int)flag} where PlatformID={config.platform.ID} and Id={Id}");
                commit();
            }
        }
        internal static void SetSyncFlag(List<T> list, _Config.Item config, Sync_Flag? flag)
        {
            if (list == null) return;
            if (list.Count == 0) return;
            flag = flag ?? Sync_Flag.Success;
            foreach (Action commit in config.archiveDB.BeginTran())
            {
                List<long> id = new List<long>();
                foreach (T n in list)
                    id.Add(n.Id);
                config.archiveDB.ExecuteNonQuery($"update {TableName<T>.Value} set _flag={(int)flag} where PlatformID={config.platform.ID} and Id in {id.ToSqlString()}");
                commit();
            }
        }

        #region WriteGameLog

        string GameLog_sqlstr;
        internal SqlCmd GameLog_SqlCmd;
        internal Sync_Flag? GameLog_Flag;

        internal Sync_Flag? CreateGameLog(_Config.Item config, _LogBase group = null)
        {
            T data = (T)this;
            GameInfo gameInfo;
            if (data.GetGameInfo(config.platform, out gameInfo))
            {
                MemberData member;
                string sql_GameLog1;
                if (GameLogAttribute.CreateGameLog(config, data, group, config.platform, gameInfo, out member, out sql_GameLog1))
                {
                    data.GameLog_sqlstr = sql_GameLog1;
                    data.GameLog_SqlCmd = util.GetSqlCmd(member.CorpInfo.DB_Log01W);// util.SqlCmd_UserLog(member.CorpInfo);
                    return data.GameLog_Flag = Sync_Flag.Success;
                }
                else if (member == null)
                    return data.GameLog_Flag = Sync_Flag.UserNotExist;
            }
            return data.GameLog_Flag = Sync_Flag.Failed;
        }

        internal bool WriteGameLog(_Config.Item config, IEnumerator<T> next = null, Sync_Flag? setSyncFlag = null, bool createGameLog = false, _LogBase group = null)
        {
            T data = (T)this;
            if (createGameLog)
                CreateGameLog(config, group);
            if (data.GameLog_sqlstr == null) return false;
            if (data.GameLog_SqlCmd == null) return false;
            if (!data.GameLog_Flag.HasValue) return false;
            if (data.GameLog_Flag == Sync_Flag.Success)
            {
                foreach (Action commit2 in data.GameLog_SqlCmd.BeginTran())
                {
                    data.GameLog_SqlCmd.ExecuteNonQueryNoDuplicateKey(data.GameLog_sqlstr);
                    if (next?.MoveNext() == true)
                        next.Current.WriteGameLog(config, next, setSyncFlag);
                    data.SetSyncFlag(config, setSyncFlag ?? Sync_Flag.Success);
                    commit2();
                    return true;
                }
            }
            else if (data.GameLog_Flag.HasValue)
                data.SetSyncFlag(config, data.GameLog_Flag.Value);
            return false;
        }

        #endregion

        #region Archive

        #region IDbImport

        void IDbImport.Import(DbDataReader reader, int fieldIndex, string fieldName, object value) => OnDbImport(reader, fieldIndex, fieldName, value);
        void IDbImport.Missing(DbDataReader reader, int fieldIndex, string fieldName, object value) => OnDbImport(reader, fieldIndex, fieldName, value);
        void OnDbImport(DbDataReader reader, int fieldIndex, string fieldName, object value)
        {
            if (sql_Archive != null)
                this.OnDbImport(reader, fieldIndex, fieldName, value, sql_Archive);
        }
        protected virtual void OnDbImport(DbDataReader reader, int fieldIndex, string fieldName, object value, SqlBuilder sql)
        {
            if (value is DateTime)
                sql["", fieldName, SqlBuilder.DateTimeFormatX] = value;
            else if (value is byte[])
                sql["", fieldName] = (SqlBuilder.str)(((byte[])value).ToHexString());
            else
                sql["", fieldName] = value;
        }

        #endregion

        internal SqlBuilder sql_Archive;

        //static List<T> getSource(_Config.Item config, string op)
        //{
        //    StringBuilder sqlstr = new StringBuilder($"select top({util.TopN}) * from {TableName<T>.Value} nolock where datediff(ms,{util.CreateTime<T>()},getdate()) > {config.Reserved} and {util.CreateTime<T>()} {op} '{config.NewTimeStr}' order by {util.CreateTime<T>()} asc");
        //    if (!string.IsNullOrEmpty(util.Finished<T>()))
        //    {
        //        List<long> finished = null;
        //        string sql_finish1 = $"update {TableName<T>.Value} set _flag={(int)Sync_Flag.Finished_Timeout} where _flag is null and {util.Finished<T>()}= 0 and dateadd(ms,{config.SyncTimeout}, {util.CreateTime<T>()}) < getdate() select Id from {TableName<T>.Value} nolock where PlatformID={config.platform.ID} and _flag is null and {util.Finished<T>()}=0";
        //        foreach (Action commit in config.archiveDB.BeginTran())
        //        {
        //            foreach (SqlDataReader r in config.archiveDB.ExecuteReaderEach(sql_finish1))
        //                _null._new(ref finished).Add(r.GetInt64("Id"));
        //            commit();
        //        }
        //        if (finished != null)
        //        {
        //            string sql_finish2 = $"select Id from {TableName<T>.Value} nolock where Id in {finished.ToSqlString()} and {util.Finished<T>()}=1";
        //            finished.Clear();
        //            foreach (SqlDataReader r in config.gameDB.ExecuteReaderEach(sql_finish2))
        //                finished.Add(r.GetInt64("Id"));
        //            if (finished.Count > 0)
        //            {
        //                config.archiveDB.ExecuteNonQuery(true, $"delete {TableName<T>.Value} where PlatformID={config.platform.ID} and Id in {finished.ToSqlString()}");
        //                sqlstr.Append($" select * from {TableName<T>.Value} nolock where Id in {finished.ToSqlString()}");
        //            }
        //        }
        //    }
        //    return config.gameDB.ToList(config._new<T>, sqlstr.ToString());
        //    //List<T> data2 = null;
        //    //foreach (SqlDataReader r in config.gameDB.ExecuteReaderEach(sqlstr.ToString()))
        //    //{
        //    //    T data = new T() { sql_Archive = new SqlBuilder() };
        //    //    r.FillObject(data);
        //    //    data.sql_Archive["", "PlatformID"] = config.platform.ID;
        //    //    _null._new(ref data2).Add(data);
        //    //}
        //    //return data2 ?? _null<T>.list;
        //}

        static List<T> getSource(_Config.Item config, string op)
        {
            string sql = $@"select top({util.TopN}) * from {TableName<T>.Value} nolock where datediff(ms,{util.CreateTime<T>()},getdate()) > {config.Reserved} and {util.CreateTime<T>()} {op} '{config.NewTimeStr}' order by {util.CreateTime<T>()} asc";
            return config.gameDB.ToList(config._new<T>, sql);
        }
        internal static int ArchiveData(_Config.Item config)
        {
            int count = 0;
            config.NewTime = config.LastTime;
            grp_cache cache = new grp_cache();
            var data1 = _LogBase<T>.getSource(config, ">");
            foreach (T data in data1)
                if (data.ArchiveData(config, cache))
                    count++;

            if (config.NewTime != config.LastTime)
            {
                var data2 = _LogBase<T>.getSource(config, "=");
                foreach (T data in data2)
                    if (data.ArchiveData(config, cache))
                        count++;
            }

            //for (int i = 0; i < 2; i++)
            //{
            //    var data2 = getSource<T>();
            //    foreach (T data in data2)
            //        if (ArchiveData<T>(data, cache))
            //            count++;
            //}
            config.Update();
            return count;
        }

        internal bool ArchiveData(_Config.Item config, grp_cache cache = null, bool delete_exists = false)
        {
            T data = (T)this;
            if (data == null) return false;
            GameInfo gameInfo;
            if (data.GetGameInfo(config.platform, out gameInfo, grps: cache))
            {
                string sql_archive;
                if (delete_exists)
                    sql_archive = $"delete from {TableName<T>.Value} where PlatformID={config.platform.ID} and Id={data.Id} {data.sql_Archive._insert(TableName<T>.Value)}";
                else
                    sql_archive = $"if not exists (select Id from {TableName<T>.Value} nolock where PlatformID={config.platform.ID} and Id={data.Id}) {data.sql_Archive._insert(TableName<T>.Value)}";
                string sql_replay = null;
                IGameReplay _replay = data as IGameReplay;
                if (_replay != null)
                {
                    data.sql_Archive["GameLog"] = null;
                    //if (_finished?.IsFinished ?? true)
                    if (data.IsFinished)
                    {
                        SqlBuilder sql = new SqlBuilder();
                        sql["w", "PlatformID"] = config.platform.ID;
                        sql["w", "GameID"] = gameInfo.ID;
                        sql["w", "GroupID"] = data.GroupID;
                        sql[" ", "data"] = (SqlBuilder.str)"@data";
                        sql_replay = $@"delete from Replay {sql._where()} {sql._insert("Replay")}";
                    }
                }
                foreach (Action commit1 in config.archiveDB.BeginTran())
                {
                    int cnt = config.archiveDB.ExecuteNonQueryNoDuplicateKey(sql_archive);
                    if (!string.IsNullOrEmpty(sql_replay))
                    {
                        config.replayDB.Parameters.Clear();
                        config.replayDB.Parameters.Add("@data", SqlDbType.Text).Value = _replay.GameReplay ?? "";
                        config.replayDB.ExecuteNonQueryNoDuplicateKey(true, sql_replay);
                        config.replayDB.Parameters.Clear();
                    }
                    bool success;
                    if (delete_exists)
                        success = cnt > 1;
                    else
                        success = cnt == 1;
                    if (success)
                    {
                        commit1();
                        return true;
                    }
                }

                //if (data.proc_Archive(ref sqlcmd1))
                //    count++;
                if (config.NewTime < data.CreateTime)
                    config.NewTime = data.CreateTime;
            }
            return false;
        }

        internal T GetSourceData(_Config.Item config) => config.gameDB.ToObject(config._new<T>, $"select * from {TableName<T>.Value} nolock where Id={this.Id}");

        internal T ArchiveDataAgain(_Config.Item config, T row2 = null, Sync_Flag? setFlag = null)
        {
            T row1 = (T)this;
            if (row2?.sql_Archive == null)
                row2 = GetSourceData(config);
            if (row2 == null) return row1;
            if (setFlag.HasValue)
                row2.sql_Archive["", "_flag"] = setFlag;
            if (row2.ArchiveData(config, delete_exists: true))
                return row2;
            return row1;
        }

        #endregion

        internal static void Proc_Slot(_Config logItems)
        {
            foreach (_Config.Item config in _Config.Item.GetConfig<T>(logItems).Lock())
            {
                IG01PlatformInfo platform = logItems.platform;
                foreach (bool active in config.Tick())
                {
                    if (active)
                    {
                        int count = _LogBase<T>.ArchiveData(config);
                        string sqlstr = $"select top({util.TopN}) * from {TableName<T>.Value} nolock where PlatformID={platform.ID} and _flag is null {Null.gameIDs(platform)}";
                        var datas = config.archiveDB.ToList<T>(sqlstr);
                        foreach (T row1 in datas)
                        {
                            T row0 = row1.GetSourceData(config) ?? row1;
                            if (row0.IsFinished)
                                row1.ArchiveDataAgain(config, row0).WriteGameLog(config, createGameLog: true);
                            else if (row1._sync2 > config.SyncTimeout)
                                row1.ArchiveDataAgain(config, row0, Sync_Flag.Finished_Timeout);
                        }
                    }
                    else
                    {
                        config.Reload();
                    }
                }
            }
        }
    }

    public abstract class _LogBase_JP<T> : _LogBase<T> where T : _LogBase_JP<T>, new()
    {
        public abstract int PlayerId { get; set; }
        public abstract int GameId { get; set; }
        public abstract string JPType { get; set; }
        public abstract string GameType { get; set; }
        public abstract decimal BetAmount { get; set; }
        public abstract decimal WinAmount { get; set; }

        JackpotLog _jp1;
        List<JackpotUpdateLog> _jp2;
        public virtual decimal? JP_GRAND { get { return _jp2.GRAND()?.PushAmount; } }
        public virtual decimal? JP_MAJOR { get { return _jp2.MAJOR()?.PushAmount; } }
        public virtual decimal? JP_MINOR { get { return _jp2.MINOR()?.PushAmount; } }
        public virtual decimal? JP_MINI { get { return _jp2.MINI()?.PushAmount; } }
        public virtual decimal JP_Total { get { return (this.JP_GRAND ?? 0) + (this.JP_MAJOR ?? 0) + (this.JP_MINI ?? 0) + (this.JP_MINOR ?? 0); } }

        static string sql_getJackpot(_Config.Item config, int PlayerId, int GameId, string SerialNumber, bool? flag_is_null = null)
        {
            string sql = $"where PlatformID={config.platform.ID} and PlayerId={PlayerId} and GameId={GameId} and SerialNumber='{SerialNumber}'";
            if (flag_is_null.HasValue)
                return $"{sql} and _flag is{(flag_is_null.Value ? " " : " not ")}null";
            else
                return sql;
        }
        static JackpotLog getJackpotLog(_Config.Item config, int PlayerId, int GameId, string SerialNumber, bool? flag_is_null = null)
        {
            return config.archiveDB.ToObject<JackpotLog>($"select * from {TableName<JackpotLog>.Value} nolock {(sql_getJackpot(config, PlayerId, GameId, SerialNumber, flag_is_null))}");
        }
        static List<JackpotUpdateLog> getJackpotUpdateLog(_Config.Item config, int PlayerId, int GameId, string SerialNumber, bool? flag_is_null = null)
        {
            return config.archiveDB.ToList<JackpotUpdateLog>($"select * from {TableName<JackpotUpdateLog>.Value} nolock {(sql_getJackpot(config, PlayerId, GameId, SerialNumber, flag_is_null))} order by {util.CreateTime<JackpotUpdateLog>()}");
        }

        internal static void Proc_Jackpot(_Config logItems)
        {
            foreach (_Config.Item config_a in _Config.Item.GetConfig<JackpotLog>(logItems).Lock())
            {
                _Config.Item config_b = _Config.Item.GetConfig<JackpotUpdateLog>(logItems);
                foreach (_Config.Item config_c in _Config.Item.GetConfig<GameSpin>(logItems).Lock())
                {
                    foreach (_Config.Item config_d in _Config.Item.GetConfig<FivePK>(logItems).Lock())
                    {
                        foreach (bool active in config_a.Tick())
                        {
                            if (active)
                            {
                                int aa = 0, bb = 0, cc = 0, dd = 0;
                                for (;;)
                                {
                                    int _a = _LogBase<JackpotUpdateLog>.ArchiveData(config_b); aa += _a;
                                    int _b = _LogBase<JackpotLog>.ArchiveData(config_a); bb += _b;
                                    int _c = _LogBase<GameSpin>.ArchiveData(config_c); cc += _c;
                                    int _d = _LogBase<FivePK>.ArchiveData(config_d); dd += _d;
                                    if ((_a == 0) && (_b == 0) && (_c == 0) && (_d == 0)) break;
                                }
                                break;
                                GameSpin.proc3(config_c, config_a);
                                FivePK.proc3(config_d, config_a);
                                //continue;
                                //                        int aaa = 0, bbb = 0;

                                //                        var list1a = config.archiveDB.ToList<JackpotLog>($@"select * from {TableName<JackpotLog>.Value} nolock
                                //where PlatformID={config.platform.ID} and _flag is null and JackpotType<>'NONE' and SerialNumber<>'NONE'");
                                //                        var list1b = list1a; // (from _row in _jp1_list1 where _row.JackpotType != "NONE" && _row.SerialNumber != "NONE" select _row).ToList();
                                //                        foreach (var _jp1 in list1b)
                                //                        {
                                //                            var _jp2 = getJackpotUpdateLog(config, _jp1.PlayerId, _jp1.GameId, _jp1.SerialNumber);
                                //                            if (GameSpin.proc1a(config, _jp1, _jp2)) aaa++;
                                //                            else if (FivePK.proc1a(config, _jp1, _jp2)) aaa++;
                                //                        }

                                //                        var list2a = config.archiveDB.ToList<JackpotUpdateLog>($@"select top({util.TopN}) PlayerId, GameId, SerialNumber from {TableName<JackpotUpdateLog>.Value} nolock
                                //where PlatformID={config.platform.ID} and _flag is null
                                //group by PlayerId, GameId, SerialNumber");
                                //                        var list2b = list2a; // (from _row in _jp2_list1 where _row.SerialNumber == "" select _row).ToList();
                                //                        foreach (var _jp2_tmp in list2b)
                                //                        {
                                //                            if (getJackpotLog(config, _jp2_tmp.PlayerId, _jp2_tmp.GameId, _jp2_tmp.SerialNumber) != null) continue;
                                //                            var _jp2 = getJackpotUpdateLog(config, _jp2_tmp.PlayerId, _jp2_tmp.GameId, _jp2_tmp.SerialNumber);
                                //                            if (GameSpin.proc2a(config, null, _jp2_tmp.PlayerId, _jp2_tmp.GameId, _jp2_tmp.SerialNumber, _jp2)) bbb++;
                                //                            else if (FivePK.proc2a(config, null, _jp2_tmp.PlayerId, _jp2_tmp.GameId, _jp2_tmp.SerialNumber, _jp2)) bbb++;
                                //                        }
                            }
                            else
                            {
                                config_a.Reload();
                                config_b.Reload();
                            }
                        }
                    }
                }
            }
        }

        static bool proc3(_Config.Item config, _Config.Item config_a)
        {
            string sqlstr = $"select top({util.TopN}) PlayerId,GameId,SerialNumber from {TableName<T>.Value} nolock where PlatformID={config.platform.ID} and _flag is null {config.platform.GameIDs(true)} group by PlayerId,GameId,SerialNumber";
            List<T> list0 = config.archiveDB.ToList<T>(sqlstr);
            List<T> list1 = new List<T>(list0);
            while (list1.Count > 0)
            {
                T row0 = list1[0];
                list1.RemoveAt(0);
                proc3(config, config_a, list1, row0.PlayerId, row0.GameId, row0.SerialNumber);
            }
            return false;
        }

        static bool proc3(_Config.Item config, _Config.Item config_a, List<T> list1, int PlayerId, int GameId, string SerialNumber, JackpotLog _jp1 = null, List<JackpotUpdateLog> _jp2 = null)
        {
            List<T> rows = config.archiveDB.ToList<T>($"select * from {TableName<T>.Value} nolock where PlatformID={config.platform.ID} and PlayerId={PlayerId} and GameId={GameId} and SerialNumber='{SerialNumber}' and _flag is null");
            if (rows.Count == 0) return false;
            if (rows.Count > 1)
            {
                for (int i = rows.Count - 1; i >= 0; i--)
                {
                    T row1 = rows[i];
                    T row2 = row1.GetSourceData(config);
                    if (row2.BetAmount != 0) continue;
                    proc3(config, row1, row2);
                    rows.RemoveAt(i);
                }
                if (rows.Count == 1)
                    list1.Insert(0, rows[0]);
            }
            else
            {
                T row1 = rows[0];
                T row2 = row1.GetSourceData(config);
                row2._jp1 = _jp1 ?? getJackpotLog(config, row1.PlayerId, row1.GameId, row1.SerialNumber, true);
                row2._jp2 = _jp2 ?? getJackpotUpdateLog(config, row1.PlayerId, row1.GameId, row1.SerialNumber, true);
                if (row2._jp2.IsValidate())
                    return proc3(config, row1, row2);
                if (row2._jp2.Count == 8)
                {
                    while (row2._jp2.Count > 4)
                        row2._jp2.RemoveAt(0);
                    //List<JackpotUpdateLog> _jp2x = new List<JackpotUpdateLog>();
                    //while (row2._jp2.Count > 4)
                    //{
                    //    _jp2x.Add(row2._jp2[4]);
                    //    row2._jp2.RemoveAt(4);
                    //}
                    int sn2;
                    if (row1.sn2.ToInt32(out sn2))
                    {
                        SerialNumber = $"{row1.sn1}-{sn2 + 1}";
                        if (proc3(config, config_a, list1, PlayerId, GameId, SerialNumber, null, row2._jp2))
                        {
                            list1.Insert(0, row1);
                            list1.RemoveWhen((tmp) => tmp.SerialNumber == SerialNumber);
                            return true;
                        }
                    }
                    return false;
                }
                if (row1._sync2 > config.SyncTimeout)
                    return proc3(config, row1, row2, Sync_Flag.Jackpot_Error);
                return false;
            }
            return false;
        }

        static bool proc3(_Config.Item config, T row1, T row2, Sync_Flag? setflag = null)
        {
            Sync_Flag? flag;
            if (setflag.HasValue)
                flag = setflag;
            else
            {
                if (row2.IsFinished)
                    flag = null;
                else if (row1._sync2 > config.SyncTimeout)
                    flag = Sync_Flag.Finished_Timeout;
                else
                    return false;
            }

            row2.sql_Archive["", "_jp_GRAND "] = row2._jp2?.GRAND()?.Id;
            row2.sql_Archive["", "_jp_MAJOR "] = row2._jp2?.MAJOR()?.Id;
            row2.sql_Archive["", "_jp_MINOR "] = row2._jp2?.MINOR()?.Id;
            row2.sql_Archive["", "_jp_MINI  "] = row2._jp2?.MINI()?.Id;

            if (row2._jp1 == null)
            {
                row2.CreateGameLog(config);
                foreach (Action commit in config.archiveDB.BeginTran())
                {
                    row2._jp2?.SetSyncFlag(config, flag);
                    row1.ArchiveDataAgain(config, row2);
                    row2.WriteGameLog(config, null, flag);
                    commit();
                    return true;
                }
            }
            else
            {
                row2.sql_Archive["", "_jp"] = row2._jp1.Id;
                row2._jp1.GroupID = row2.GroupID;
                row2.WinAmount -= row2._jp1.WinAmount;
                row2._jp1.CreateGameLog(config);
                row2.CreateGameLog(config);
                foreach (Action commit1 in config.archiveDB.BeginTran())
                {
                    foreach (Action commit2 in row2._jp1.GameLog_SqlCmd.BeginTran())
                    {
                        row2._jp2?.SetSyncFlag(config, flag);
                        row1.ArchiveDataAgain(config, row2);
                        row2._jp1.WriteGameLog(config, setSyncFlag: flag);
                        row2.WriteGameLog(config, null, flag);
                        commit1();
                        commit2();
                        return true;
                    }
                }
            }
            return false;
        }

        //static bool proc2a(_Config.Item config, T row1, int PlayerId, int GameId, string SerialNumber, List<JackpotUpdateLog> _jp2)
        //{
        //    if (row1 != null)
        //        return proc2b(config, row1, PlayerId, GameId, SerialNumber, _jp2);
        //    string sqlstr = $"select * from {TableName<T>.Value} where PlatformID={config.platform.ID} and PlayerId={PlayerId} and GameId={GameId} and SerialNumber='{SerialNumber}' and _flag is null"; //{util.sql_Finished<T>()}" 
        //    List<T> _rows = config.archiveDB.ToList<T>(sqlstr);
        //    if (_rows.Count == 1)
        //        return proc2b(config, _rows[0], PlayerId, GameId, SerialNumber, _jp2);
        //    else if (_rows.Count == 0)
        //    {
        //        if (typeof(T) == typeof(GameSpin))
        //            return FivePK.proc2a(config, null, PlayerId, GameId, SerialNumber, _jp2);
        //        else
        //        {
        //            foreach (var tmp in _jp2)
        //                if (tmp._sync2 < config.SyncTimeout)
        //                    return false;
        //            _jp2.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //        }
        //    }
        //    else
        //    {
        //        foreach (T tmp1 in _rows)
        //        {
        //            if ((tmp1.BetAmount != 0) || !tmp1.IsFinished) continue;
        //            T tmp0 = tmp1.GetSourceData(config);
        //            if ((tmp0.BetAmount != 0) || !tmp0.IsFinished) continue;
        //            tmp0.ArchiveDataAgain(config, tmp0).WriteGameLog(config, createGameLog: true);
        //        }
        //    }
        //    return false;
        //}
        //static bool proc2b(_Config.Item config, T row1, int PlayerId, int GameId, string SerialNumber, List<JackpotUpdateLog> _jp2)
        //{
        //    T row2 = null;
        //    if (!row1.IsFinished)
        //    {
        //        row2 = row1.GetSourceData(config);
        //        if (row2.IsFinished)
        //            row1 = row2;
        //        else if (row1._sync2 < config.SyncTimeout)
        //            return false;
        //        else
        //        {
        //            foreach (Action commit in config.archiveDB.BeginTran())
        //            {
        //                row1.ArchiveDataAgain(config, row2, Sync_Flag.Finished_Timeout);
        //                _jp2.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //                commit();
        //            }
        //            return false;
        //        }
        //    }

        //    if (_jp2.IsValidate())
        //    {
        //        foreach (Action commit in config.archiveDB.BeginTran())
        //        {
        //            T row0 = row1.ArchiveDataAgain(config, row2);
        //            row0._jp2 = _jp2;
        //            row0.WriteGameLog(config, createGameLog: true);
        //            commit();
        //            return true;
        //        }
        //    }
        //    else if (_jp2.Count == 8)
        //    {
        //        while (_jp2.Count > 4)
        //            _jp2.RemoveAt(0);
        //        int sn2;
        //        if (row1.sn2.ToInt32(out sn2))
        //            return proc2a(config, null, PlayerId, GameId, $"{row1.sn1}-{sn2 + 1}", _jp2);
        //    }
        //    return false;
        //}

        //static bool proc1a(_Config.Item config, JackpotLog _jp1, List<JackpotUpdateLog> _jp2)
        //{
        //    string sqlstr = $"PlatformID={config.platform.ID} and PlayerId={_jp1.PlayerId} and GameId={_jp1.GameId} and SerialNumber='{_jp1.SerialNumber}'";
        //    sqlstr = $"select * from {TableName<T>.Value} nolock where _flag is null and {sqlstr}";
        //    List<T> _rows = config.archiveDB.ToList<T>(sqlstr);
        //    if (_rows.Count == 1)
        //        return proc1b(config, _rows[0], _jp1, _jp2);
        //    else if (_rows.Count == 0)
        //    {
        //        if (typeof(T) == typeof(GameSpin))
        //            return FivePK.proc1a(config, _jp1, _jp2);
        //        else if (_jp1._sync2 > config.SyncTimeout)
        //        {
        //            foreach (Action commit in config.archiveDB.BeginTran())
        //            {
        //                _jp1.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //                _jp2.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (T tmp1 in _rows)
        //        {
        //            if ((tmp1.BetAmount != 0) || (tmp1.WinAmount != 0) | !tmp1.IsFinished) continue;
        //            T tmp0 = tmp1.ArchiveDataAgain(config);
        //            if ((tmp0.BetAmount != 0) || (tmp0.WinAmount != 0) | !tmp0.IsFinished) continue;
        //            tmp0.WriteGameLog(config, createGameLog: true);
        //        }
        //    }
        //    return false;
        //}
        //static bool proc1b(_Config.Item config, T row1, JackpotLog _jp1, List<JackpotUpdateLog> _jp2)
        //{
        //    T row2 = null;
        //    if (!row1.IsFinished)
        //    {
        //        row2 = row1.GetSourceData(config);
        //        if (row2.IsFinished)
        //            row1 = row2;
        //        else if (row1._sync2 < config.SyncTimeout)
        //            return false;
        //        else
        //        {
        //            foreach (Action commit in config.archiveDB.BeginTran())
        //            {
        //                _jp1.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //                _jp2.SetSyncFlag(config, Sync_Flag.Finished_Timeout);
        //                row1.ArchiveDataAgain(config, row2, Sync_Flag.Finished_Timeout);
        //                commit();
        //            }
        //            return false;
        //        }
        //    }
        //    if (!_jp2.IsValidate())
        //    {
        //        if (row1._sync2 < config.SyncTimeout)
        //            return false;
        //    }
        //    T row0 = row1.ArchiveDataAgain(config, row2);
        //    row0._jp1 = _jp1;
        //    row0._jp2 = _jp2;
        //    _jp1.GroupID = row0.GroupID;
        //    row0.WinAmount -= _jp1.WinAmount;
        //    _jp1.CreateGameLog(config);
        //    row0.CreateGameLog(config);
        //    foreach (Action commit1 in config.archiveDB.BeginTran())
        //    {
        //        foreach (Action commit2 in _jp1.GameLog_SqlCmd.BeginTran())
        //        {
        //            _jp1.WriteGameLog(config);
        //            row0.WriteGameLog(config);
        //            commit1();
        //            commit2();
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //protected override string gameIDs(IG01PlatformInfo platform) => platform.GameIDs(false);

        //internal override void SetSyncFlag(_Config.Item config, Sync_Flag flag)
        //{
        //    foreach (Action commit in config.archiveDB.BeginTran())
        //    {
        //        string n0 = _jp1?.Id.ToString() ?? "null";
        //        string n1 = _jp2.GRAND()?.Id.ToString() ?? "null";
        //        string n2 = _jp2.MAJOR()?.Id.ToString() ?? "null";
        //        string n3 = _jp2.MINOR()?.Id.ToString() ?? "null";
        //        string n4 = _jp2.MINI()?.Id.ToString() ?? "null";
        //        config.archiveDB.ExecuteNonQuery($"update {TableName<T>.Value} set _flag={(int)flag}, _jp={n0}, _jp_GRAND={n1}, _jp_MAJOR={n2}, _jp_MINOR={n3}, _jp_MINI={n4} where PlatformID={config.platform.ID} and Id={Id}");
        //        _jp2.SetSyncFlag(config, flag);
        //        commit();
        //    }
        //}
    }

    public static class _LogBase<TGrp, TBet>
        where TGrp : _LogBase<TGrp, TBet>.Grp, new()
        where TBet : _LogBase<TGrp, TBet>.Bet, new()
    {
        internal static void Proc(_Config logItems)
        {
            //TGrp grp = _LogBase<TGrp>.Null;
            //TBet bet = _LogBase<TBet>.Null;
            foreach (_Config.Item config_g in _Config.Item.GetConfig<TGrp>(logItems).Lock())
            {
                _Config.Item config_b = _Config.Item.GetConfig<TBet>(logItems);// bet.config_init(logItems.platform);
                foreach (bool active in config_g.Tick())
                {
                    if (active)
                    {
                        int grp_count = _LogBase<TGrp>.ArchiveData(config_g);
                        int bet_count = _LogBase<TBet>.ArchiveData(config_b);

                        List<TGrp> grps = config_g.archiveDB.ToList<TGrp>($"select top({util.TopN}) * from {TableName<TGrp>.Value} nolock where PlatformID={config_g.platform.ID} and _flag is null");// {util.sql_Finished<TGrp>()}");
                        foreach (TGrp grp in grps)
                            proc1(config_g, grp);
                        //gamelog(config_g, grp, bet);
                    }
                    else
                    {
                        config_g.Reload();
                        config_b.Reload();
                    }
                }
            }
        }

        static bool proc1(_Config.Item config, TGrp grp)
        {
            //if (grp.Id == 11725)
            //    Debugger.Break();
            GameInfo gameInfo;
            if (!grp.GetGameInfo(config.platform, out gameInfo))
                return false;
            TGrp grp2 = null;
            if (!grp.IsFinished)
            {
                grp2 = grp.GetSourceData(config);
                if (grp2.IsFinished)
                    _null.noop();
                else if (grp._sync2 < config.SyncTimeout)
                    return false;
                else
                {
                    foreach (Action commit in config.archiveDB.BeginTran())
                    {
                        _LogBase<TBet>.SetSyncFlag(grp.Players, config, Sync_Flag.Finished_Timeout);
                        grp.ArchiveDataAgain(config, grp2, Sync_Flag.Finished_Timeout);
                        commit();
                    }
                    return false;
                }
            }

            grp.Players = config.archiveDB.ToList(() => new TBet() { Group = grp },
                $"select * from {TableName<TBet>.Value} nolock where {util.GroupID<TGrp>()}={grp.Id}");
            if ((grp.Players.Count > 0) && (grp.Players.Count == grp.TotalPlayerCount))
            {
                grp2 = grp.ArchiveDataAgain(config, grp2);
                grp2.Players = new List<TBet>();
                foreach (Action commit in config.archiveDB.BeginTran())
                {
                    foreach (TBet p in grp.Players)
                    {
                        TBet p2 = p.ArchiveDataAgain(config);
                        p2.Group = grp2;
                        grp2.Players.Add(p2);
                    }
                    commit();
                }
                bool success = true;
                foreach (TBet p in grp2.Players)
                    success &= Sync_Flag.Success == p.CreateGameLog(config, grp2);
                foreach (Action commit in config.archiveDB.BeginTran())
                {
                    if (success)
                    {
                        var players3 = grp2.Players.GetEnumerator();
                        if (players3.MoveNext())
                            players3.Current.WriteGameLog(config, players3);
                        grp2.SetSyncFlag(config, Sync_Flag.Success);
                    }
                    else
                    {
                        foreach (TBet p in grp2.Players)
                            p.SetSyncFlag(config, p.GameLog_Flag == Sync_Flag.Success ? Sync_Flag.PartialFailed : p.GameLog_Flag ?? Sync_Flag.PartialFailed);
                        grp2.SetSyncFlag(config, grp2.GameLog_Flag == Sync_Flag.Success ? Sync_Flag.PartialFailed : grp2.GameLog_Flag ?? Sync_Flag.PartialFailed);
                    }
                    commit();
                    return success;
                }
            }
            else if (grp._sync2 > config.SyncTimeout)
            {
                foreach (Action commit in config.archiveDB.BeginTran())
                {
                    foreach (TBet p in grp.Players)
                        p.ArchiveDataAgain(config, setFlag: Sync_Flag.Finished_Timeout);
                    grp.ArchiveDataAgain(config, grp2, Sync_Flag.Finished_Timeout);
                    commit();
                }
            }
            return false;
        }

        public abstract class Grp : _LogBase<TGrp>
        {
            public override long GroupID { get { return Id; } set { Id = value; } }
            public abstract int PlayerCount { get; }
            public abstract int TotalPlayerCount { get; }
            public List<TBet> Players;
        }

        public abstract class Bet : _LogBase<TBet>
        {
            internal TGrp Group;
            internal TGrp GetGroupRow(SqlCmd sqlcmd = null, grp_cache grps = null)
            {
                if (Group != null) return Group;
                if (grps != null)
                {
                    _LogBase tmp;
                    grps.TryGetValue(this.GroupID, out tmp);
                    Group = tmp as TGrp;
                    if (Group != null) return Group;
                }
                Group = (sqlcmd ?? util.GetSqlCmd(ams.DB.GeniusBullLogR)).ToObject<TGrp>($"select * from {TableName<TGrp>.Value} nolock where Id={this.GroupID}");
                if ((Group != null) && (grps != null))
                    grps[this.GroupID] = Group;
                return Group;
            }
            internal override int GetGameID(grp_cache grps = null) => GetGroupRow(null, grps)?.GetGameID() ?? 0;
            public override string SerialNumber { get { return GetGroupRow()?.SerialNumber; } set { GetGroupRow().SerialNumber = value; } }
        }
    }

    class grp_cache : Dictionary<long, _LogBase> { }

    partial class JackpotLog
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount; } }
        [GameLog(nameof(GameLog.BetAmount), ApplyExchangeRate = true)]
        public decimal BetAmount { get { return 0; } }
        [GameLog(nameof(GameLog.Balance), ApplyExchangeRate = true)]
        public decimal Balance { get { return 0; } }
    }
    //partial class JackpotUpdateLog
    //{
    //}
    partial class SlotGameLog
    {
        [DbImport("Amount")]
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get; set; }
    }
    partial class FivePK
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class RedDog
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class Oasis
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class GameSpin
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class DouDizhuBet
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class TexasBet
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount; } }
    }
    partial class TwMahjongBet
    {
        [GameLog(nameof(GameLog.Amount), ApplyExchangeRate = true)]
        public decimal Amount { get { return WinAmount - BetAmount + DcCompe - DcFine; } }
    }
}