using ams.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ams;
using GeniusBull;
using System.Web;

namespace GeniusBull
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn), ams.TableName("Game")]
    public class Game
    {
        [DbImport, JsonProperty]
        public int Id;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public string Route;
        [DbImport, JsonProperty]
        public string FileToken;
        [DbImport, JsonProperty]
        public int Width;
        [DbImport, JsonProperty]
        public int Height;
        [DbImport, JsonProperty]
        public string ServerUrl;
        [DbImport, JsonProperty]
        public int ServerPort;
        [DbImport, JsonProperty]
        public string ServerRest;
        [DbImport, JsonProperty]
        public string ServerUrlForFun;
        [DbImport, JsonProperty]
        public int ServerPortForFun;
        [DbImport, JsonProperty]
        public string ServerRestForFun;
        [DbImport, JsonProperty]
        public bool Jackpot;
        [DbImport, JsonProperty]
        public int Click;
        [DbImport, JsonProperty]
        public int Sort;
        [DbImport, JsonProperty]
        public int Category;
        [DbImport, JsonProperty]
        public int Status;
        [DbImport, JsonProperty]
        public DateTime InsertDate;
        [DbImport, JsonProperty]
        public DateTime? ModifyDate;

        public IG01PlatformInfo Platform { get; private set; }

        public string[] ConfigKeys
        {
            get; private set;
        }

        public Type TableConfigType;

        Dictionary<string, GameConfig> configs;
        public Dictionary<string, GameConfig> GetConfigs()
        {
            if (this.configs == null)
            {
                if (this.ConfigKeys.Length > 0)
                {
                    StringBuilder sql1 = new StringBuilder($"select * from {ams.TableName<GameConfig>.Value} nolock where Name in ");
                    this.ConfigKeys.ToSqlString(sql1);
                    List<GameConfig> tmp = this.Platform.GameDB().ToList<GameConfig>($"{sql1}");
                    if (tmp.Count > 0)
                        this.configs = tmp.ToDictionary<GameConfig, string>((c) => c.Name);
                }
                this.configs = this.configs ?? _null<Dictionary<string, GameConfig>>.value;
            }
            return this.configs;
        }

        readonly object _game_sync = new object();
        RedisVer<List<TableConfig>> _tableSettings;
        public List<TableConfig> GetTableSettings(SqlCmd gamedb = null)
        {
            if (gamedb != null) return ReadTableSettings(gamedb, this.Id);
            RedisVer<List<TableConfig>> _tableSettings;
            lock (_game_sync)
            {
                if (this._tableSettings == null)
                    this._tableSettings = new RedisVer<List<TableConfig>>("GeniusBull_TableSettings", index: this.Id) { ReadData = ReadTableSettings };
                _tableSettings = this._tableSettings;
            }
            return _tableSettings.Value;
        }
        public void TableSettings_Updated()
        {
            lock (_game_sync) this._tableSettings?.UpdateVersion();
        }

        List<TableConfig> ReadTableSettings(SqlCmd sqlcmd, int index)
        {
            SqlCmd gamedb = this.Platform?.GameDB();
            using (_HttpContext.GetSqlCmd(out gamedb, null, this.Platform.ApiUrl))
                return gamedb.ToList(
                    () => (TableConfig)Activator.CreateInstance(this.TableConfigType),
                    $"select * from {ams.TableNameAttribute.GetTableName(this.TableConfigType)} nolock");
        }


        Dictionary<string, EprobTableLimit> _EprobTableLimit;
        public Dictionary<string, EprobTableLimit> EprobTableLimit
        {
            get
            {
                return _EprobTableLimit = _EprobTableLimit ??
                    (from x1 in GeniusBull.EprobTableLimit.Cache[this.Platform?.ID ?? 0].Value
                     where x1.GameId == this.Id
                     orderby x1.SymbolCode
                     select x1).ToDictionary((x2) => x2.Symbol);
            }
        }

        public bool HasEprobTable() => this.EprobTableLimit.Count > 0;
        public bool HasGameConfig() => this.ConfigKeys.Length > 0;
        public bool HasTableSettings() => TableConfigType != null;

        public static readonly Game Null = new Game(null, 0);

        public Game(IG01PlatformInfo p, int gameId)
        {
            this.Platform = p;
            switch (this.Id = gameId)
            {
                case 06: /* WINTERWONDERS       聖誕奇蹟        */ break;
                case 25: /* WATCHTHEBIRDIE      探索鳥任務      */ break;
                case 69: /* BRUCELEE            功夫           */ ConfigKeys = new[] { "BRUCELEE_BG_SETTING" }; break;
                case 56: /* BEAUTYANDTHENERD    正妹愛阿宅      */ ConfigKeys = new[] { "BEAUTYANDTHENERD_DOUBLEODDS_LIST" }; break;
                case 28: /* ROLLOUTTHEBARRELS   啤酒狂歡節      */ break;
                case 64: /* PREHISTORICPARK     史前公園        */ break;
                case 14: /* OCEANTREASURE       海底寶藏        */ break;
                case 12: /* SECRETGARDEN        秘密花園        */ break;
                case 23: /* REELOFFORTUNE       時來運轉        */ break;
                case 19: /* ATOMICTGE           原子時代        */ break;
                case 20: /* GUSHERSGOLD         油田大富翁      */ break;
                case 57: /* TRUEILLUSIONS       大魔法師        */ break;
                case 22: /* OPERANIGHT          歌劇夜晚        */ break;
                case 65: /* COLLECTOR           收藏家           */ break;
                case 05: /* FLYINGCOLORS        彩色大機戰      */ break;
                case 26: /* ZOMBIEZEEMONEY      錢進屍樂園      */ break;
                case 11: /* SUMMEREASE          夏日派對        */ break;
                case 04: /* BESTOFLUCK          吉星高照        */ break;
                case 24: /* SCARYRICH           萬聖夜驚魂      */ break;
                case 13: /* CLEOPATRASCOINS     埃及豔后的錢幣   */ break;
                case 21: /* ICEPICKS            突破冰原        */ break;
                case 03: /* CANDYCOTTAGE        糖果屋           */ break;
                case 09: /* DIAMONDTEMPLE       鑽石神廟        */ break;
                case 27: /* NUCLEARFISHIN       瘋狂輻射魚      */ break;
                case 01: /* MONSTER             怪物村          */ break;
                case 02: /* ASTRALLUCK          幸運星座        */ break;
                case 68: /* LOUNGEBAR           LoungeBar水果盤 */ break;
                case 49: /* ROCKET              Rocket水果盤    */ break;
                case 45: /* CARS                Cars小瑪莉      */ ConfigKeys = new[] { "CARS_DOUBLEODDS_LIST", "CARS_SEND_ODDS", "CARS_SENDNUM_RANGE", "CARS_RANDPRICE" }; break;
                case 40: /* MARIOSISTERS        Mario小瑪莉     */ ConfigKeys = new[] { "MARIOSISTER_SEND_ODDS", "MARIOSISTER_SENDNUM_RANGE", "MARIOSISTER_RANDPRICE", "MARIOSISTER_DOUBLEODDS_LIST" }; break;
                case 48: /* STARTEAMS5PK        星際戰隊5PK     */ ConfigKeys = new[] { "STARTEAMS5PK_LOSE", "STARTEAMS5PK_JOKER_RAISE", "STARTEAMS5PK_DOUBLEODDS_LIST", "STARTEAMS5PK_SEND_ODDS", "STARTEAMS5PK_SENDRATE_ODDS" }; break;
                case 66: /* AXEGANG5PK          斧頭幫5PK       */ ConfigKeys = new[] { "AXEGANG5PK_LOSE", "AXEGANG5PK_JOKER_RAISE", "AXEGANG5PK_DOUBLEODDS_LIST", "AXEGANG5PK_SEND_ODDS", "AXEGANG5PK_SENDRATE_ODDS", "AXEGANG5PK_BONUS_ODDS" }; break;
                case 29: /* KING5PK             撲克王5PK       */ ConfigKeys = new[] { "KING5PK_LOSE", "KING5PK_JOKER_RAISE", "KING5PK_DOUBLEODDS_LIST", "KING5PK_SEND_ODDS", "KING5PK_SENDRATE_ODDS" }; break;
                case 67: /* NIGHTOFSHANGHAI     夜上海7PK       */ ConfigKeys = new[] { "NIGHTOFSHANGHAI7PK_LOSE", "NIGHTOFSHANGHAI7PK_JOKER_RAISE", "NIGHTOFSHANGHAI7PK_SEND_ODDS", "NIGHTOFSHANGHAI7PK_SENDRATE_ODDS", "NIGHTOFSHANGHAI7PK_DOUBLEODDS_LIST", "NIGHTOFSHANGHAI7PK_BONUS_ODDS" }; break;
                case 43: /* NINJA7PK            忍者7PK        */ ConfigKeys = new[] { "NINJA7PK_JOKER_RAISE", "NINJA7PK_LOSE", "NINJA7PK_SEND_ODDS", "NINJA7PK_SENDRATE_ODDS", "NINJA7PK_DOUBLEODDS_LIST" }; break;
                case 42: /* GOBLIN7PK           小妖精7PK       */ ConfigKeys = new[] { "GOBLIN7PK_LOSE", "GOBLIN7PK_JOKER_RAISE", "GOBLIN7PK_SEND_ODDS", "GOBLIN7PK_SENDRATE_ODDS", "GOBLIN7PK_DOUBLEODDS_LIST" }; break;
                case 1092: /* DOUDIZHU          鬥地主          */ TableConfigType = typeof(DouDizhuConfig); ConfigKeys = new[] { "FIGHTTHELANDLORD_PREVENT_CHEATING", "FIGHTTHELANDLORD_SERVICE_CHARGE" }; break;
                case 1091: /* TEXASHOLDEM       德州撲克        */ TableConfigType = typeof(TexasConfig); ConfigKeys = new[] { "TEXASHOLDEM_SERVICECHARGE", "TEXASHOLDEM_PREVENT_CHEATING", "TEXASHOLDEM_SERVICECHARGE_LIMIT" }; break;
                case 1093: /* TWMAHJONG         台灣麻將        */ TableConfigType = typeof(TwMahjongConfig); ConfigKeys = new string[0]; break;
            }
            ConfigKeys = ConfigKeys ?? new string[0];
        }

        //public static RedisVer<List<Game>>.Dict Cache = new RedisVer<List<Game>>.Dict("GeniusBull_Game")
        //{
        //    ReadData = (sqlcmd, index) =>
        //    {
        //        IG01PlatformInfo p = IG01PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(index);
        //        SqlCmd gamedb;
        //        using (DB.GetSqlCmd(out gamedb, null, p.ApiUrl))
        //            return gamedb.ToList(
        //                () => new Game(p, gamedb.DataReader.GetInt32(nameof(Game.Id))),
        //                $"select * from {TableName<Game>.Value} nolock")
        //                ?? new List<Game>();
        //    },
        //    CheckUpdate = (value, index) =>
        //    {
        //        IG01PlatformInfo p = IG01PlatformInfo.GetPlatformInfo<IG01PlatformInfo>(index);
        //        foreach (var n in value)
        //            if (n.Platform != p)
        //                return true;
        //        return false;
        //    }
        //};

        //public static Game GetGame(IG01PlatformInfo p, int gameId)
        //{
        //    return Cache[p.ID].Value.Find((n) => n.Id == gameId);
        //}

        public static Game GetGame(PlatformGameInfo p1, IG01PlatformInfo p2 = null, SqlCmd gamedb = null)
        {
            if (p1 == null) return null;
            p2 = p2 ?? p1.GetPlatformInfo() as IG01PlatformInfo;
            if (p2 == null) return null;
            gamedb = gamedb ?? p2.GameDB();
            return gamedb.ToObject(() => new Game(p2, gamedb.DataReader.GetInt32("Id")), $"select * from {ams.TableName<Game>.Value} nolock where Id={p1.OriginalID}");
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn), ams.TableName("JackpotConfig")]
    public class JackpotConfig
    {
        [DbImport, JsonProperty]
        public string JackpotType;
        [DbImport, JsonProperty]
        public decimal Ratio;
        [DbImport, JsonProperty]
        public int Goal;
        [DbImport, JsonProperty]
        public int Base;
    }
}
namespace ams.Data
{
    partial class IG01PlatformInfo
    {
        public override void ExtraInfo(List<PlatformGameInfo> rows)
        {
            SqlCmd gamedb = GameDB();
            List<Game> games = gamedb.ToList(() => new Game(this, gamedb.DataReader.GetInt32("Id")), $"select * from {TableName<Game>.Value} nolock");
            //if (games == null) games = new List<Game>();
            List<EprobTable> eprobs = GameDB().ToList<EprobTable>($@"select GameId, Eprob from {TableName<EprobTable>.Value} where Selected=1 group by GameId, Eprob");
            foreach (var row in rows)
            {
                if (row.GetPlatformInfo() is IG01PlatformInfo)
                {
                    Game game = games.Find((g) => g.Id.ToString() == row.OriginalID);
                    if (game != null)
                    {
                        row.EprobGroup = eprobs.Find((_eprob) => _eprob.GameId == game.Id)?.Eprob;
                        //row.GameParams =
                        //row.HasEprobTable = game.HasEprobTable();
                        if (game.ConfigKeys.Length > 0)
                            row.ConfigKeys = game.ConfigKeys.Length;
                    }
                }
            }
            //lock (games)
            //{
            //    if (games.Count == 0)
            //    {
            //        //games = this.GameDB().ToObject(() => new Game(p1, p2), $"select * from {TableName<Game>.Value} nolock where Id={p1.OriginalID}");
            //    }
            //}
            //GameInfo game = rows.GetGameInfo();
        }
    }
}