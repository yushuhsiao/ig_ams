using ams;
using ams.Data;
using ams.Models;
using GeniusBull;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;

// 每組設定值可用的 Symbol, 定義在 ams_core.dbo.GeniusBull_EprobTableLimit, 
// 新增 Symbol 之後, 重新儲存一次就會自動新增到 EprobTable
// 刪除 Symbol 不處理

namespace GeniusBull
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GeniusBullSysApiController : _ApiController
    {
        [JsonProperty]
        Guid? ID;
        [JsonProperty]
        int? Eprob;
        [JsonProperty]
        int? SetActive;
        [JsonProperty]
        int? ParamID;

        PlatformGameInfo _p1;
        PlatformGameInfo p1 { get { return _p1 = _p1 ?? PlatformGameInfo.GetItem(this.ID.Value, err: true); } }

        IG01PlatformInfo _p2;
        IG01PlatformInfo p2 { get { return _p2 = _p2 ?? p1.GetPlatformInfo() as IG01PlatformInfo; } }

        Game _game;
        Game game { get { return _game = _game ?? Game.GetGame(p1, p2); } }

        SqlCmd _gamedb;
        SqlCmd gamedb { get { return _gamedb = _gamedb ?? p2.GameDB(); } }

        #region EprobTable

        [JsonProperty]
        Dictionary<string, EprobTableArguments> EprobTable;

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class EprobTableArguments
        {
            [JsonProperty]
            public int[] Reel;

            ////public long? Id;
            //public string GameId;
            //public int Eprob;
            ////public bool? Selected;
            //[JsonProperty]
            //public int? Symbol;
            //[JsonProperty]
            //public string SymbolName;
            //[JsonProperty]
            //public int? Reel_1;
            //[JsonProperty]
            //public int? Reel_2;
            //[JsonProperty]
            //public int? Reel_3;
            //[JsonProperty]
            //public int? Reel_4;
            //[JsonProperty]
            //public int? Reel_5;

            //public EprobTable Source;
            //public bool diff(SqlCmd gamedb, string gameId, int eprob)
            //{
            //    if (!Symbol.HasValue) return false;
            //    this.GameId = gameId;
            //    this.Eprob = eprob;
            //    Source = gamedb.ToObject<EprobTable>(sql_select());
            //    if (Source == null) return false;
            //    if (Reel_1.HasValue && (Reel_1.Value == Source.Reel_1)) Reel_1 = null;
            //    if (Reel_2.HasValue && (Reel_2.Value == Source.Reel_2)) Reel_2 = null;
            //    if (Reel_3.HasValue && (Reel_3.Value == Source.Reel_3)) Reel_3 = null;
            //    if (Reel_4.HasValue && (Reel_4.Value == Source.Reel_4)) Reel_4 = null;
            //    if (Reel_5.HasValue && (Reel_5.Value == Source.Reel_5)) Reel_5 = null;
            //    return true;
            //}

            //string sql_where() => $"where GameId={GameId} and Eprob={Eprob} and Symbol={Symbol}";
            //public string sql_select() => $"select * from {TableName<EprobTable>.Value} nolock {sql_where()}";
            //public string sql_update()
            //{
            //    List<string> _set = null;
            //    if (Reel_1.HasValue) _null._new(ref _set).Add($"Reel_1={Reel_1}");
            //    if (Reel_2.HasValue) _null._new(ref _set).Add($"Reel_2={Reel_2}");
            //    if (Reel_3.HasValue) _null._new(ref _set).Add($"Reel_3={Reel_3}");
            //    if (Reel_4.HasValue) _null._new(ref _set).Add($"Reel_4={Reel_4}");
            //    if (Reel_5.HasValue) _null._new(ref _set).Add($"Reel_5={Reel_5}");
            //    if (_set == null) return null;
            //    return $"update {TableName<EprobTable>.Value} set {string.Join(", ", _set)} {sql_where()}";
            //}
        }

        [HttpPost, Route("~/GeniusBull/EprobTable/GetGroup")]
        public List<EprobTable.Group> GetEprobGroup(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
            });
            string sql = $@"select GameId, Eprob, Selected from {TableName<EprobTable>.Value}
where GameId={p1.OriginalID}
group by GameId, Eprob, Selected";
            return gamedb.ToList<EprobTable.Group>(sql);
        }

        [HttpPost, Route("~/GeniusBull/EprobTable/SetGroup")]
        public List<EprobTable.Group> SetEprobGroup(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
                ModelState.Validate(nameof(SetActive), SetActive);
            });
            if (SetActive.HasValue)
            {
                foreach (Action commit in gamedb.BeginTran())
                {
                    int s = (int)p2.GameDB().ExecuteScalar($@"
update EprobTable set Selected=1 where GameId={p1.OriginalID} and Eprob =  {SetActive}
update EprobTable set Selected=0 where GameId={p1.OriginalID} and Eprob <> {SetActive}
select count(*) from (
    select GameId, Eprob, Selected from EprobTable
    where GameId={p1.OriginalID} and Selected = 1
    group by GameId, Eprob, Selected) a");
                    if (s == 1)
                        commit();
                }
            }
            return GetEprobGroup(args);
        }

        [HttpPost, Route("~/GeniusBull/EprobTable/GetValue")]
        public Dictionary<string, EprobTable> GetEprobTable(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
                ModelState.Validate(nameof(Eprob), Eprob);
            });
            string sql = $@"select a.*, b.Name as SymbolName from {TableName<EprobTable>.Value} a
left join {TableName<EprobSymbol>.Value} b
on a.Symbol = b.Symbol
where a.GameId={game.Id} and a.Eprob={Eprob}
order by a.Symbol";
            Dictionary<string, EprobTable> dict = new Dictionary<string, EprobTable>();
            List<EprobTable> list = gamedb.ToList<EprobTable>(sql);
            foreach (string symbol in game.EprobTableLimit.Keys)
            {
                GeniusBull.EprobTableLimit r1 = game.EprobTableLimit[symbol];
                GeniusBull.EprobTable r2 = list.Find((n) => n.SymbolName == symbol);
                r2 = r2 ?? new EprobTable() { };
                r2.PayRate["X2"] = r1.X2_Ratio;
                r2.PayRate["X3"] = r1.X3_Ratio;
                r2.PayRate["X4"] = r1.X4_Ratio;
                r2.PayRate["X5"] = r1.X5_Ratio;
                dict[symbol] = r2;
            }
            return dict;
        }

        [HttpPost, Route("~/GeniusBull/EprobTable/SetValue")]
        public Dictionary<string, EprobTable> SetEprobTable(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
                ModelState.Validate(nameof(Eprob), Eprob);
            });
            if (EprobTable != null)
            {
                //List<EprobTableArguments> rows = new List<EprobTableArguments>();
                try
                {
                    StringBuilder sql1 = new StringBuilder();
                    foreach (string symbol in game.EprobTableLimit.Keys)
                    {
                        EprobTableLimit n1 = game.EprobTableLimit[symbol];
                        string _where = $" where GameId={game.Id} and Eprob={Eprob} and Symbol={n1.SymbolCode}";
                        EprobTableArguments n2;
                        if (!EprobTable.TryGetValue(symbol, out n2))
                        {
                            _where += " -- *";
                            n2 = new EprobTableArguments() { Reel = new int[5] };
                        }
                        SqlBuilder sql = new SqlBuilder();
                        sql["w", nameof(GeniusBull.EprobTable.GameId)] = game.Id;
                        sql["w", nameof(GeniusBull.EprobTable.Eprob)] = Eprob.Value;
                        sql["w", nameof(GeniusBull.EprobTable.Symbol)] = n1.SymbolCode;
                        sql[" ", nameof(GeniusBull.EprobTable.Selected)] = 0;
                        for (int i = 0, j = Math.Min(5, n2.Reel.Length); i < j; i++)
                        {
                            if (n2.Reel[i] < n1.Min[i]) n2.Reel[i] = n1.Min[i];
                            if (n2.Reel[i] > n1.Max[i]) n2.Reel[i] = n1.Max[i];
                            sql["u", $"Reel_{i + 1}"] = n2.Reel[i];
                        }
                        sql1.AppendLine($@"if exists (select Id from {TableName<EprobTable>.Value} nolock {sql._where()})
     update {TableName<EprobTable>.Value}{sql._update_set()}{sql._where()}
else insert into {TableName<EprobTable>.Value}{sql._insert()}");
                    }
                    if (sql1.Length > 0)
                    {
                        sql1.Append($"select sum(Reel_1) as Reel_1, sum(Reel_2) as Reel_2, sum(Reel_3) as Reel_3, sum(Reel_4) as Reel_4, sum(Reel_5) as Reel_5 from {TableName<EprobTable>.Value} nolock where GameId={game.Id} and Eprob={Eprob}");
                        string sql2 = sql1.ToString();
                        foreach (Action commit in gamedb.BeginTran())
                        {
                            EprobTable chk = gamedb.ToObject<EprobTable>(sql2);
                            int sum = chk.Reel.Sum();
                            if (sum > 3)
                                commit();
                        }
                    }
                }
                catch { }
            }
            return GetEprobTable(args);
        }

        #endregion

        #region ConfigValue

        [JsonProperty]
        Dictionary<string, GameConfig> ConfigValue;

        [HttpPost, Route("~/GeniusBull/GameConfig/GetValue")]
        public Dictionary<string, GameConfig> GetConfigValues(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
            });
            Dictionary<string, GameConfig> ret = null;
            if (game.ConfigKeys.Length > 0)
            {
                StringBuilder sql1 = new StringBuilder($"select * from {TableName<GameConfig>.Value} nolock where Name in ");
                game.ConfigKeys.ToSqlString(sql1);
                List<GameConfig> tmp = gamedb.ToList<GameConfig>($"{sql1}");
                if (tmp.Count > 0)
                {
                    ret = new Dictionary<string, GameConfig>();
                    foreach (var n in tmp)
                    {
                        ret[n.Name] = n;
                        n.Name = null;
                    }
                }
            }
            return ret ?? _null<Dictionary<string, GameConfig>>.value;
        }

        [HttpPost, Route("~/GeniusBull/GameConfig/SetValue")]
        public Dictionary<string, GameConfig> SetConfigValues(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
            });
            if (ConfigValue != null)
            {
                try
                {
                    StringBuilder sql1 = new StringBuilder();
                    foreach (string name in ConfigValue.Keys)
                    {
                        GameConfig n1 = ConfigValue[name];
                        if (!game.ConfigKeys.Contains(name)) continue;
                        if (string.IsNullOrEmpty(n1.Value)) continue;
                        sql1.AppendLine($@"update {TableName<GameConfig>.Value} set Value=N'{SqlCmd.magic_quote(n1.Value)}' where Name='{name}'");
                    }
                    int cnt = gamedb.ExecuteNonQuery(true, sql1.ToString());
                }
                catch { }
            }
            return GetConfigValues(args);
        }

        #endregion

        #region TableSetting

        [JsonProperty]
        TableSettingArguments TableSetting;

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class TableSettingArguments
        #region { }
        {
            [JsonProperty]
            public int? Id;
            [JsonProperty]
            public string TableName_EN;
            [JsonProperty]
            public string TableName_CHS;
            [JsonProperty]
            public string TableName_CHT;
            [JsonProperty]
            public int? BaseValue;
            [JsonProperty]
            public int? SecondsToCountdown;
            [JsonProperty]
            public bool SnatchLord;
            [JsonProperty]
            public bool Fine;
            [JsonProperty]
            public bool MissionMode;
            [JsonProperty]
            public bool Ai;
            [JsonProperty]
            public int? LuckyHand;
            [JsonProperty]
            public int? FakePlayerNum;
            [JsonProperty]
            public int? SmallBlind;
            [JsonProperty]
            public int? BigBlind;
            [JsonProperty]
            public int? SeatMax;
            [JsonProperty]
            public int? TableMax;
            [JsonProperty]
            public int? Antes;
            [JsonProperty]
            public int? Tai;
            [JsonProperty]
            public int? RoundType;
            [JsonProperty]
            public int? ThinkTime;
            [JsonProperty]
            public int? ServiceCharge;
            [JsonProperty]
            public int? MoneyLimit;
        }
        #endregion

        [HttpPost, Route("~/GeniusBull/TableSettings/GetGroup")]
        public List<TableConfig> GetTableSettingList(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
            });
            if (game.TableConfigType == null)
                throw new _Exception(Status.ParameterNotAllow);
            var ret = game.GetTableSettings(gamedb);
            //gamedb.ToList(() => (TableConfig)Activator.CreateInstance(game.TableConfigType),
            //    $"select * from {TableNameAttribute.GetTableName(game.TableConfigType)} nolock");
            return ret;
        }

        [HttpPost, Route("~/GeniusBull/TableSettings/SetGroup")]
        public TableConfig SetTableSettingList(_empty args) { return null; }

        [HttpPost, Route("~/GeniusBull/TableSettings/GetValue")]
        public TableConfig GetTableSetting(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
                ModelState.Validate(nameof(ParamID), ParamID);
            });
            return gamedb.ToObject(() => (TableConfig)Activator.CreateInstance(game.TableConfigType),
                $"select * from {TableNameAttribute.GetTableName(game.TableConfigType)} nolock where Id={ParamID}");
        }

        [HttpPost, Route("~/GeniusBull/TableSettings/SetValue")]
        public TableConfig SetTableSetting(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(ID), ID);
                ModelState.Validate(nameof(ParamID), ParamID);
            });
            if (this.TableSetting == null)
                throw new _Exception(Status.ParameterNotAllow);
            SqlBuilder sql = new SqlBuilder();
            sql["w", "Id"] = TableSetting.Id;
            if (game.TableConfigType == typeof(DouDizhuConfig))
            {
                sql["Nu", nameof(DouDizhuConfig.TableName_EN), "      "] = TableSetting.TableName_EN;
                sql["Nu", nameof(DouDizhuConfig.TableName_CHS), "     "] = TableSetting.TableName_CHS;
                sql["Nu", nameof(DouDizhuConfig.TableName_CHT), "     "] = TableSetting.TableName_CHT;
                sql[" u", nameof(DouDizhuConfig.BaseValue), "         "] = TableSetting.BaseValue;
                sql[" u", nameof(DouDizhuConfig.SecondsToCountdown), ""] = TableSetting.SecondsToCountdown;
                sql[" u", nameof(DouDizhuConfig.SnatchLord), "        "] = TableSetting.SnatchLord;
                sql[" u", nameof(DouDizhuConfig.Fine), "              "] = TableSetting.Fine;
                sql[" u", nameof(DouDizhuConfig.MissionMode), "       "] = TableSetting.MissionMode;
                sql[" u", nameof(DouDizhuConfig.LuckyHand), "         "] = TableSetting.LuckyHand;
                sql[" u", nameof(DouDizhuConfig.FakePlayerNum), "     "] = TableSetting.FakePlayerNum;
                sql[" u", nameof(DouDizhuConfig.ModifyDate), "        "] = SqlBuilder.str.getdate;
            }
            else if (game.TableConfigType == typeof(TexasConfig))
            {
                sql["Nu", nameof(TexasConfig.TableName_EN), "      "] = TableSetting.TableName_EN;
                sql["Nu", nameof(TexasConfig.TableName_CHS), "     "] = TableSetting.TableName_CHS;
                sql["Nu", nameof(TexasConfig.TableName_CHT), "     "] = TableSetting.TableName_CHT;
                sql[" u", nameof(TexasConfig.SmallBlind), "        "] = TableSetting.SmallBlind;
                sql[" u", nameof(TexasConfig.BigBlind), "          "] = TableSetting.BigBlind;
                sql[" u", nameof(TexasConfig.SecondsToCountdown), ""] = TableSetting.SecondsToCountdown;
                sql[" u", nameof(TexasConfig.SeatMax), "           "] = TableSetting.SeatMax;
                sql[" u", nameof(TexasConfig.TableMax), "          "] = TableSetting.TableMax;
                sql[" u", nameof(TexasConfig.ModifyDate), "        "] = SqlBuilder.str.getdate;
            }
            else if (game.TableConfigType == typeof(TwMahjongConfig))
            {   
                sql[" u", nameof(TwMahjongConfig.Antes), "        "] = TableSetting.Antes;
                sql[" u", nameof(TwMahjongConfig.Tai), "          "] = TableSetting.Tai;
                sql[" u", nameof(TwMahjongConfig.RoundType), "    "] = TableSetting.RoundType;
                sql[" u", nameof(TwMahjongConfig.ThinkTime), "    "] = TableSetting.ThinkTime;
                sql[" u", nameof(TwMahjongConfig.ServiceCharge), ""] = TableSetting.ServiceCharge;
                sql[" u", nameof(TwMahjongConfig.MoneyLimit), "   "] = TableSetting.MoneyLimit;
                sql[" u", nameof(TwMahjongConfig.ModifyDate), "   "] = SqlBuilder.str.getdate;
            }
            string s = $"update {TableNameAttribute.GetTableName(game.TableConfigType)}{sql._update_set()}{sql._where()}";
            gamedb.ExecuteNonQuery(true, s);
            game.TableSettings_Updated();
            return GetTableSetting(args);
        }

        #endregion

        #region JackpotLog

        IG01PlatformInfo jackpot_p
        {
            get { return _p2 = _p2 ?? PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(this.PlatformName, err: true); }
        }

        [JsonProperty]
        UserName PlatformName;
        [JsonProperty]
        string JackpotType;
        [JsonProperty]
        JackpotConfigArguments JackpotRow;

        [JsonObject(MemberSerialization = MemberSerialization.OptIn), TableName("JackpotConfig")]
        public class JackpotConfigArguments
        {
            [JsonProperty]
            public string JackpotType;
            [JsonProperty]
            public decimal? Ratio;
            [JsonProperty]
            public int? Goal;
            [JsonProperty]
            public int? Base;
        }

        [HttpPost, Route("~/GeniusBull/JackpotConfig/list")]
        public List<JackpotConfig> GetJackpotConfigList(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(PlatformName), PlatformName);
            });
            return jackpot_p.GameDB().ToList<JackpotConfig>($"select * from {TableName<JackpotConfig>.Value} nolock");
        }

        [HttpPost, Route("~/GeniusBull/JackpotConfig/get")]
        public JackpotConfig GetJackpotConfig(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(PlatformName), PlatformName);
                ModelState.Validate(nameof(JackpotType), JackpotType);
            });
            return jackpot_p.GameDB().ToObject<JackpotConfig>($"select * from {TableName<JackpotConfig>.Value} nolock where JackpotType='{SqlCmd.magic_quote(JackpotType)}'");
        }

        [HttpPost, Route("~/GeniusBull/JackpotConfig/set")]
        public JackpotConfig SetJackpotConfig(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(PlatformName), PlatformName);
                if (JackpotRow == null)
                    ModelState.AddModelError("JackpotRow", Status.InvalidParameter);
                else
                {
                    ModelState.Validate(nameof(JackpotConfigArguments.JackpotType), JackpotRow.JackpotType);
                    ModelState.Validate(nameof(JackpotConfigArguments.Ratio), JackpotRow.Ratio, allow_null: true);
                    ModelState.Validate(nameof(JackpotConfigArguments.Goal), JackpotRow.Goal, allow_null: true);
                    ModelState.Validate(nameof(JackpotConfigArguments.Base), JackpotRow.Base, allow_null: true);
                }
            });
            SqlBuilder sql = new SqlBuilder();
            sql["w", nameof(JackpotConfigArguments.JackpotType)] = JackpotRow.JackpotType;
            sql["u", nameof(JackpotConfigArguments.Ratio)] = JackpotRow.Ratio;
            sql["u", nameof(JackpotConfigArguments.Goal)] = JackpotRow.Goal;
            sql["u", nameof(JackpotConfigArguments.Base)] = JackpotRow.Base;
            if (sql.UpdateCount > 0)
                jackpot_p.GameDB().ExecuteNonQuery(true, $"update {TableName<JackpotConfig>.Value}{sql._update_set()}{sql._where()}");
            return GetJackpotConfig(args);
        }

        #endregion

        //[HttpPost, Route("~/GeniusBull/GetWaitUserCount")]
        //public object GetWaitUserCount()
        //{
        //    IG01PlatformInfo p = IG01PlatformInfo.PokerInstance;
        //    return p.rest_MJ_waitingPlayers();
        //}
    }

    [TableName("MemberBlacklist"), JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberBlacklist
    {
        [DbImport, JsonProperty]
        public long Id;
        [DbImport, JsonProperty]
        public int MemberId;
        [DbImport, JsonProperty]
        public int BlacklistId;
        [JsonProperty]
        public string BlacklistName;
        [DbImport, JsonProperty]
        public DateTime BlacklistTime;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GeniusBullUserApiController : _ApiController
    {
        #region arguments

        [JsonProperty]
        UserName CorpName;

        [JsonProperty]
        UserName UserName;

        [JsonProperty]
        UserName PlatformName;

        [JsonProperty]
        UserName BlacklistName;

        #endregion

        [HttpPost, Route("~/Users/Member/BlackList/List")]
        public List<MemberBlacklist> GetBlackList(_empty args)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
                ModelState.Validate(nameof(PlatformName), PlatformName);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: CorpName, err: true);
            MemberData m1 = corp.GetMemberData(name: UserName, err: true);
            IG01PlatformInfo platform = PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(this.PlatformName, err: true);
            IG01MemberPlatformData m2 = platform.GetMember(m1, false);
            List<MemberBlacklist> ret = platform.GameDB().ToList<MemberBlacklist>($"select * from {TableName<MemberBlacklist>.Value} nolock where MemberId={m2.destID}");
            for (int i = ret.Count - 1; i >= 0; i--)
            {
                var n = ret[i];
                IG01MemberPlatformData m3 = platform.GetMemberByDestID(corp.DB_User01R(), n.BlacklistId);
                MemberData m4 = corp.GetMemberData(m3.MemberID);
                if (m3 == null || m4 == null)
                {
                    ret.RemoveAt(i);
                }
                else
                {
                    n.MemberId = m1.ID;
                    n.BlacklistId = m4.ID;
                    n.BlacklistName = m4.UserName;
                }
            }
            return ret;
        }

        bool ModifyBlackList(_empty args, Func<IG01MemberPlatformData, IG01MemberPlatformData, string> cb)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
                ModelState.Validate(nameof(PlatformName), PlatformName);
                ModelState.Validate(nameof(BlacklistName), BlacklistName);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: CorpName, err: true);
            IG01PlatformInfo platform = PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(this.PlatformName, err: true);
            MemberData m1 = corp.GetMemberData(name: UserName, err: true);
            MemberData m2 = corp.GetMemberData(name: BlacklistName, err: true);
            IG01MemberPlatformData m3 = platform.GetMember(m1, true);
            IG01MemberPlatformData m4 = platform.GetMember(m2, true);
            if ((m3 == null) || (m4 == null)) return false;
            SqlCmd gamedb = platform.GameDB();
            foreach (Action commit in gamedb.BeginTran())
            {
                int n = gamedb.ExecuteNonQuery(cb(m3, m4));
                if (n > 0)
                {
                    commit();
                    return true;
                }
                return n == 1;
            }
            return false;
        }

        [HttpPost, Route("~/Users/Member/BlackList/Add")]
        public bool AddBlackList(_empty args)
            => ModifyBlackList(args, (m3, m4) => $@"if not exists (select Id from {TableName<MemberBlacklist>.Value} where MemberId={m3.destID} and BlacklistId={m4.destID})
insert into {TableName<MemberBlacklist>.Value} (MemberId,BlacklistId,BlacklistTime) values ({m3.destID},{m4.destID},getdate())");

        [HttpPost, Route("~/Users/Member/BlackList/Remove")]
        public bool RemoveBlackList(_empty args)
            => ModifyBlackList(args, (m3, m4) => $"delete from {TableName<MemberBlacklist>.Value} where MemberId={m3.destID} and BlacklistId={m4.destID}");
        //public bool RemoveBlackList(_empty args)
        //{
        //    this.Validate(true, args, () =>
        //    {
        //        ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
        //        ModelState.Validate(nameof(UserName), UserName);
        //        ModelState.Validate(nameof(PlatformName), PlatformName);
        //        ModelState.Validate(nameof(BlacklistName), BlacklistName);
        //    });
        //    CorpInfo corp = CorpInfo.GetCorpInfo(name: CorpName, err: true);
        //    IG01PlatformInfo platform = PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(this.PlatformName, err: true);
        //    MemberData m1 = corp.GetMemberData(name: UserName, err: true);
        //    MemberData m2 = corp.GetMemberData(name: BlacklistName, err: true);
        //    IG01MemberPlatformData m3 = platform.GetMember(m1, false);
        //    IG01MemberPlatformData m4 = platform.GetMember(m2, false);
        //    if ((m3 == null) || (m4 == null)) return false;
        //    SqlCmd gamedb = platform.GameDB();
        //    foreach (Action commit in gamedb.BeginTran())
        //    {
        //        int n = gamedb.ExecuteNonQuery($"delete from {TableName<MemberBlacklist>.Value} where MemberId={m3.destID} and BlacklistId={m4.destID}");
        //        if (n == 1)
        //        {
        //            commit();
        //            return true;
        //        }
        //        return n == 1;
        //    }
        //    return false;
        //}
    }
}
