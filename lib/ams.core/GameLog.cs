using Newtonsoft.Json;
using System;
using System.Data;

namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("GameLog", SortField = nameof(PlayEndTime), SortOrder = SortOrder.desc)]
    public class GameLog
    {
        internal PlatformInfo Platform;
        internal GameInfo GameInfo;

        public static GameLog[] GetGameLog(CorpInfo corp, PlatformInfo platform, GameInfo game, long groupID)
            => GetGameLog(corp, platform.ID, game.ID, groupID);
        public static GameLog[] GetGameLog(CorpInfo corp, int platformID, int gameID, long groupID)
            => corp.DB_Log01R().ToList<GameLog>($"select * from {TableName<GameLog>.Value} nolock where PlatformID={platformID} and GameID={gameID} and GroupID={groupID}")?.ToArray() ?? _null<GameLog>.array;

        [DbImport, JsonProperty]
        public long sn;

        [DbImport, JsonProperty, Sortable]
        public UserID CorpID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName CorpName;

        [DbImport, JsonProperty, Sortable]
        public UserID ParentID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName ParentName;

        [DbImport, JsonProperty, Sortable]
        public UserID UserID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName UserName;

        [DbImport, JsonProperty, Sortable]
        public int Depth;

        [DbImport, JsonProperty, Sortable, Filterable]
        public int PlatformID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public string PlatformName;// { get { return Platform?.PlatformName; } }

        [DbImport, JsonProperty, Sortable, Filterable]
        public PlatformType PlatformType;

        [DbImport, JsonProperty, Sortable, Filterable]
        public string GameName;// { get { return GameInfo?.Name; } }

        [DbImport, JsonProperty, Sortable, Filterable]
        public GameClass GameClass;

        [DbImport, JsonProperty, Sortable, Filterable]
        public int GameID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public long GroupID;

        [DbImport, JsonProperty, Sortable, Filterable]
        public long GroupBetID;

        /// <summary>
        /// 帳號幣別
        /// </summary>
        [DbImport, JsonProperty]
        public CurrencyCode CurrencyA;

        /// <summary>
        /// 遊戲幣別
        /// </summary>
        [DbImport, JsonProperty]
        public CurrencyCode CurrencyB;

        /// <summary>
        /// 匯率
        /// </summary>
        [DbImport, JsonProperty]
        public decimal CurrencyX;

        /// <summary>
        /// 輸贏值
        /// </summary>
        [DbImport, JsonProperty, Sortable]
        public decimal? Amount;

        /// <summary>
        /// 總公點
        /// </summary>
        [DbImport, JsonProperty]
        public decimal? TotalFees;

        /// <summary>
        /// 有效投注額
        /// </summary>
        [DbImport, JsonProperty, Sortable]
        public decimal? BetAmount;

        [DbImport, JsonProperty, Sortable]
        public decimal? WinAmount;

        [DbImport, JsonProperty, Sortable]
        public decimal? Balance;

        /// <summary>
        /// Jackpot Type
        /// </summary>
        [DbImport, JsonProperty]
        public string JPType; // FivePK, GameSpin

        /// <summary>
        /// 拉中彩金時, 彩池當時的數值 (Jackpot - WinAmount + FillAmount = BaseAmount )
        /// </summary>
        [DbImport, JsonProperty]
        public decimal Jackpot; // JackpotLog

        /// <summary>
        /// 彩池打底值
        /// </summary>
        [DbImport, JsonProperty]
        public decimal Base; // JackpotLog

        /// <summary>
        /// 拉走的彩金比例
        /// </summary>
        [DbImport, JsonProperty]
        public decimal Ratio; // JackpotLog

        /// <summary>
        /// 拉中彩金時, 彩池剩餘的數值
        /// </summary>
        [DbImport, JsonProperty]
        public decimal BaseAmount; // JackpotLog

        /// <summary>
        /// 剩餘的 Jackpot 小於打底時，補到打底值
        /// </summary>
        [DbImport, JsonProperty]
        public decimal FillAmount; // JackpotLog

        /// <summary>
        /// Jackpot Amount
        /// </summary>
        //[DbImport, JsonProperty]
        //public decimal JPAmount;

        /// <summary>
        /// Jackpot 提撥
        /// </summary>
        [DbImport, JsonProperty]
        public decimal JP_Total; // JackpotUpdateLog

        /// <summary>
        /// Jackpot 提撥 (GRAND)
        /// </summary>
        [DbImport, JsonProperty]
        public decimal JP_GRAND; // JackpotUpdateLog

        /// <summary>
        /// Jackpot 提撥 (MAJOR)
        /// </summary>
        [DbImport, JsonProperty]
        public decimal JP_MAJOR; // JackpotUpdateLog

        /// <summary>
        /// Jackpot 提撥 (MINOR)
        /// </summary>
        [DbImport, JsonProperty]
        public decimal JP_MINOR; // JackpotUpdateLog

        /// <summary>
        /// Jackpot 提撥 (MINI)
        /// </summary>
        [DbImport, JsonProperty]
        public decimal JP_MINI; // JackpotUpdateLog

        [DbImport, JsonProperty]
        public decimal BaseValue; // DouDizhuGame

        [DbImport, JsonProperty]
        public decimal SmallBaseValue;

        [DbImport, JsonProperty]
        public bool IsBanker;

        /// <summary>
        /// 有效玩家數量
        /// </summary>
        [DbImport, JsonProperty]
        public int PlayerCount;

        /// <summary>
        /// 總玩家數量
        /// </summary>
        [DbImport, JsonProperty]
        public int TotalPlayerCount;

        [DbImport, JsonProperty, Sortable, Filterable]
        public DateTime CreateTime;

        [DbImport, JsonProperty, Sortable, Filterable]
        public DateTime? PlayStartTime;

        [DbImport, JsonProperty, Sortable, Filterable]
        public DateTime? PlayEndTime;

        [DbImport, JsonProperty, Sortable, Filterable]
        public string SerialNumber;

        [DbImport, JsonProperty, Sortable, Filterable]
        public string sn1;

        [DbImport, JsonProperty, Sortable, Filterable]
        public string sn2;

        [DbImport, JsonProperty]
        public string Bets; // DouDizhuBet, GameSpin, Oasis, RedDog, TexasBet

        [DbImport, JsonProperty]
        public bool SnatchLord; // DouDizhuGame

        [DbImport, JsonProperty]
        public bool Fine; // DouDizhuGame

        [DbImport, JsonProperty]
        public bool MissionMode; // DouDizhuGame

        [DbImport, JsonProperty]
        public string MissionType; // DouDizhuGame

        [DbImport, JsonProperty]
        public int CallMultiplier; // DouDizhuGame

        [DbImport, JsonProperty]
        public int FinalMultiplier; // DouDizhuGame

        [DbImport, JsonProperty]
        public int NumOfSpring; // DouDizhuGame

        [DbImport, JsonProperty]
        public int NumOfAntiSpring; // DouDizhuGame

        [DbImport, JsonProperty]
        public int NumOfBomb; // DouDizhuGame

        [DbImport, JsonProperty]
        public int NumOfRocket; // DouDizhuGame

        [DbImport, JsonProperty]
        public bool MissionAccomplished; // DouDizhuGame

        [DbImport, JsonProperty]
        public string Results; // DouDizhuGame, Oasis, RedDog, TexasBet

        [DbImport, JsonProperty, Sortable]
        public decimal? Fee; // DouDizhuBet, TexasBet, TwMahjongBet

        //[DbImport, JsonProperty]
        //public int DealerSeat; // TexasGame

        //[DbImport, JsonProperty]
        //public int SmallBlind; // TexasGame

        //[DbImport, JsonProperty]
        //public int BigBlind; // TexasGame

        [DbImport, JsonProperty]
        public string Cards; // TexasGame

        //[DbImport, JsonProperty]
        //public int Seat; // TexasBet

        [DbImport, JsonProperty]
        public string FirstCard; // TexasBet

        [DbImport, JsonProperty]
        public string SecondCard; // TexasBet

        //[DbImport, JsonProperty, Sortable]
        //public decimal? Antes; // TwMahjongGame

        //[DbImport, JsonProperty, Sortable]
        //public decimal? Tai; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public sbyte? RoundType; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public int? ServiceCharge; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public int? TotalFanValue; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public int? ActiveFanValue; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public int? WindPosition; // TwMahjongGame

        [DbImport, JsonProperty, Sortable]
        public int? ExtraHand; // TwMahjongBet

        [DbImport, JsonProperty, Sortable]
        public int? SeatPosition; // TwMahjongBet

        [DbImport, JsonProperty, Sortable]
        public decimal DcFine { get; set; }

        [DbImport, JsonProperty, Sortable]
        public decimal DcCompe { get; set; }

        //[DbImport, JsonProperty, Sortable]
        //public string JackpotType; // JackpotLog

        //[DbImport, JsonProperty, Sortable]
        //public decimal? Jackpot; // JackpotLog

        //[DbImport, JsonProperty, Sortable]
        //public int? Base; // JackpotLog

        //[DbImport, JsonProperty, Sortable]
        //public decimal? Ratio; // JackpotLog

        //[DbImport, JsonProperty, Sortable]
        //public decimal? BaseAmount; // JackpotLog

        //[DbImport, JsonProperty, Sortable]
        //public decimal? FillAmount; // JackpotLog

        [DbImport, JsonProperty]
        public string ActionType; // FivePK, GameSpin

        [DbImport, JsonProperty]
        public string Odds; // GameSpin

        [DbImport, JsonProperty]
        public string Symbols; // GameSpin

        [DbImport, JsonProperty]
        public string GameType; // FivePK, GameSpin

        [DbImport, JsonProperty]
        public string Param_1; // GameSpin

        [DbImport, JsonProperty]
        public string Param_2; // GameSpin

        [DbImport, JsonProperty]
        public string Param_3; // GameSpin

        [DbImport, JsonProperty]
        public string Param_4; // GameSpin

        [DbImport, JsonProperty]
        public string Param_5; // GameSpin

        [DbImport, JsonProperty]
        public string Pays; // GameSpin

        [DbImport, JsonProperty]
        public string WinSpots; // GameSpin

        [DbImport, JsonProperty, Sortable]
        public string Deal_1; // FivePK

        [DbImport, JsonProperty, Sortable]
        public string Deal_2; // FivePK

        [DbImport, JsonProperty, Sortable]
        public string BackupCards; // FivePK

        [DbImport, JsonProperty, Sortable]
        public string WinType; // FivePK

        [DbImport, JsonProperty, Sortable]
        public string Card_1; // RedDog

        [DbImport, JsonProperty, Sortable]
        public string Card_2; // RedDog

        [DbImport, JsonProperty, Sortable]
        public string Card_3; // RedDog

        [DbImport, JsonProperty, Sortable]
        public int Spread; // RedDog

        [DbImport, JsonProperty, Sortable]
        public string BankerCards; // Oasis

        [DbImport, JsonProperty, Sortable]
        public string PlayerCardsBef; // Oasis

        [DbImport, JsonProperty, Sortable]
        public string PlayerCardsAft; // Oasis

        [DbImport, JsonProperty, Sortable]
        public decimal ExchangeCost; // Oasis
    }
}