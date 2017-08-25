using ams;
using ams.Data;
using LogService;
using System;
using System.Data;
using System.Linq;

namespace GeniusBull
{
    [ams.TableName("TwMahjongGame"), FieldName(CreateTime = "InsertDate", GroupID = "MahjongGameId")]
    public partial class TwMahjongGame : _LogBase<TwMahjongGame, TwMahjongBet>.Grp
    {
        #region Fields

        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("SerialNumber")]
        [GameLog(nameof(GameLog.SerialNumber))]
        public override string SerialNumber { get; set; }

        [GameLog(nameof(GameLog.sn1))]
        public override string sn1 { get { return base.sn1; } }
        //public string sn1 { get { return util.sn1(this.SerialNumber); } }
        [GameLog(nameof(GameLog.sn2))]
        public override string sn2 { get { return base.sn2; } }
        //public string sn2 { get { return util.sn2(this.SerialNumber); } }

        [DbImport("Antes")]
        [GameLog(nameof(GameLog.BaseValue))] //[GameLog(nameof(GameLog.Antes))]
        public int Antes { get; set; }

        [DbImport("Tai")]
        [GameLog(nameof(GameLog.SmallBaseValue))] //[GameLog(nameof(GameLog.Tai))]
        public int Tai { get; set; }

        [DbImport("RoundType")]
        [GameLog(nameof(GameLog.RoundType))]
        public sbyte RoundType { get; set; }

        [DbImport("ServiceCharge")]
        [GameLog(nameof(GameLog.ServiceCharge))]
        public int ServiceCharge { get; set; }

        /// <summary>
        /// 實際胡牌台數
        /// </summary>
        [DbImport("TotalFanValue")]
        [GameLog(nameof(GameLog.TotalFanValue))]
        public int? TotalFanValue { get; set; }

        /// <summary>
        /// 受限制的胡牌台數
        /// </summary>
        [DbImport("ActiveFanValue")]
        [GameLog(nameof(GameLog.ActiveFanValue))]
        public int? ActiveFanValue { get; set; }

        /// <summary>
        /// 風圈風位
        /// </summary>
        [DbImport("WindPosition")]
        [GameLog(nameof(GameLog.WindPosition))]
        public MahjongWindPosition? WindPosition { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayStartTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        //internal override string GroupID_FieldName { get { return "MahjongGameId"; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;

        [GameLog(nameof(GameLog.PlayerCount))]
        public override int PlayerCount { get { return 4; } }

        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public override int TotalPlayerCount { get { return 4; } }

        [GameLog(nameof(GameLog.TotalFees), ApplyExchangeRate = true)]
        public decimal? TotalFees { get { return Players?.Sum((n) => n.Fee); } }
    }
    [ams.TableName("TwMahjongBet"), FieldName(CreateTime = "InsertDate")]
    public partial class TwMahjongBet : _LogBase<TwMahjongGame, TwMahjongBet>.Bet
    {
        #region Fields

        [DbImport("Id")]
        [GameLog(nameof(GameLog.GroupBetID))]
        public override long Id { get; set; }

        [DbImport("MahjongGameId")]
        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("IsDealer")]
        [GameLog(nameof(GameLog.IsBanker))]
        public bool? IsDealer { get; set; }

        /// <summary>
        /// 目前莊家是第幾次連莊
        /// </summary>
        [DbImport("ExtraHand")]
        [GameLog(nameof(GameLog.ExtraHand))]
        public int? ExtraHand { get; set; }

        [DbImport("SeatPosition")]
        [GameLog(nameof(GameLog.SeatPosition))]
        public int? SeatPosition { get; set; }

        [DbImport("Fee")]
        [GameLog(nameof(GameLog.Fee), ApplyExchangeRate = true)]
        public decimal Fee { get; set; }

        [DbImport("DcFine")]
        public decimal DcFine { get; set; }

        [GameLog(nameof(GameLog.DcFine), ApplyExchangeRate = true)]
        public decimal DcFindInv { get { return -DcFine; } }

        [DbImport("DcCompe")]
        [GameLog(nameof(GameLog.DcCompe), ApplyExchangeRate = true)]
        public decimal DcCompe { get; set; }

        [DbImport("BetAmount")]
        [GameLog(nameof(GameLog.BetAmount), ApplyExchangeRate = true)]
        public decimal BetAmount { get; set; }

        [DbImport("WinAmount")]
        [GameLog(nameof(GameLog.WinAmount), ApplyExchangeRate = true)]
        public decimal WinAmount { get; set; }

        [DbImport("Balance")]
        [GameLog(nameof(GameLog.Balance), ApplyExchangeRate = true)]
        public decimal Balance { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayEndTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        internal override int GetPlayerID() => this.PlayerId;
    }
}