using ams;
using ams.Data;
using LogService;
using System;
using System.Data;
using System.Data.Common;

namespace GeniusBull
{
    [TableName("IG_GameLog"), FieldName(CreateTime = "InsertDate")]
    public partial class SlotGameLog : _LogBase<SlotGameLog>
    {
        #region Fields

        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("SerialNumber")]
        [GameLog(nameof(GameLog.SerialNumber))]
        public override string SerialNumber { get; set; }

        [GameLog(nameof(GameLog.sn1))]
        public override string sn1 { get { return base.sn1; } }

        [GameLog(nameof(GameLog.sn2))]
        public override string sn2 { get { return base.sn2; } }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("ActionType")]
        [GameLog(nameof(GameLog.ActionType))]
        public string ActionType { get; set; }

        [DbImport("Bets")]
        [GameLog(nameof(GameLog.Bets))]
        public string Bets { get; set; }

        [DbImport("Odds")]
        [GameLog(nameof(GameLog.Odds))]
        public string Odds { get; set; }

        [DbImport("Symbols")]
        [GameLog(nameof(GameLog.Symbols))]
        public string Symbols { get; set; }

        [DbImport("GameType")]
        [GameLog(nameof(GameLog.GameType))]
        public string GameType { get; set; }

        [DbImport("Param_1")]
        [GameLog(nameof(GameLog.Param_1))]
        public string Param_1 { get; set; }

        [DbImport("Param_2")]
        [GameLog(nameof(GameLog.Param_2))]
        public string Param_2 { get; set; }

        [DbImport("Param_3")]
        [GameLog(nameof(GameLog.Param_3))]
        public string Param_3 { get; set; }

        [DbImport("Param_4")]
        [GameLog(nameof(GameLog.Param_4))]
        public string Param_4 { get; set; }

        [DbImport("Param_5")]
        [GameLog(nameof(GameLog.Param_5))]
        public string Param_5 { get; set; }

        [DbImport("Pays")]
        [GameLog(nameof(GameLog.Pays))]
        public string Pays { get; set; }

        [DbImport("WinSpots")]
        [GameLog(nameof(GameLog.WinSpots))]
        public string WinSpots { get; set; }

        //[DbImport("GameFinished")]
        //public bool GameFinished { get; set; }

        [DbImport("Deal_1")]
        [GameLog(nameof(GameLog.Deal_1))]
        public string Deal_1 { get; set; }

        [DbImport("Deal_2")]
        [GameLog(nameof(GameLog.Deal_2))]
        public string Deal_2 { get; set; }

        [DbImport("BackupCards")]
        [GameLog(nameof(GameLog.BackupCards))]
        public string BackupCards { get; set; }

        [DbImport("WinType")]
        [GameLog(nameof(GameLog.WinType))]
        public string WinType { get; set; }

        [DbImport("JPType")]
        [GameLog(nameof(GameLog.JPType))]
        public string JPType { get; set; }

        [DbImport("BetAmount")]
        [GameLog(nameof(GameLog.BetAmount), ApplyExchangeRate = true)]
        public decimal BetAmount { get; set; }

        [DbImport("WinAmount")]
        [GameLog(nameof(GameLog.WinAmount), ApplyExchangeRate = true)]
        public decimal WinAmount { get; set; }

        [DbImport("Balance")]
        [GameLog(nameof(GameLog.Balance), ApplyExchangeRate = true)]
        public decimal Balance { get; set; }

        [DbImport("JP_Balance")]
        [GameLog(nameof(GameLog.Jackpot), ApplyExchangeRate = true)]
        public decimal JP_Balance { get; set; }

        [DbImport("JP_Base")]
        [GameLog(nameof(GameLog.Base))]
        public int JP_Base { get; set; }

        [DbImport("JP_Ratio")]
        [GameLog(nameof(GameLog.Ratio))]
        public decimal JP_Ratio { get; set; }

        [DbImport("JP_BaseAmount")]
        [GameLog(nameof(GameLog.BaseAmount), ApplyExchangeRate = true)]
        public decimal JP_BaseAmount { get; set; }

        [DbImport("JP_FillAmount")]
        [GameLog(nameof(GameLog.FillAmount), ApplyExchangeRate = true)]
        public decimal JP_FillAmount { get; set; }

        [DbImport("JP_Total")]
        [GameLog(nameof(GameLog.JP_Total), ApplyExchangeRate = true)]
        public decimal JP_Total { get; set; }

        [DbImport("JP_GRAND")]
        [GameLog(nameof(GameLog.JP_GRAND), ApplyExchangeRate = true)]
        public decimal? JP_GRAND { get; set; }

        [DbImport("JP_MAJOR")]
        [GameLog(nameof(GameLog.JP_MAJOR), ApplyExchangeRate = true)]
        public decimal? JP_MAJOR { get; set; }

        [DbImport("JP_MINOR")]
        [GameLog(nameof(GameLog.JP_MINOR), ApplyExchangeRate = true)]
        public decimal? JP_MINOR { get; set; }

        [DbImport("JP_MINI")]
        [GameLog(nameof(GameLog.JP_MINI), ApplyExchangeRate = true)]
        public decimal? JP_MINI { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayEndTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        protected override void OnDbImport(DbDataReader reader, int fieldIndex, string fieldName, object value, SqlBuilder sql)
        {
            if (fieldName == nameof(JP_Total)) return;
            base.OnDbImport(reader, fieldIndex, fieldName, value, sql);
        }

        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get { return this.Id; } set { this.Id = value; } }

        [GameLog(nameof(GameLog.GroupBetID))]
        long GroupBetID { get { return 0; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;

        internal override int GetPlayerID() => this.PlayerId;

        [GameLog(nameof(GameLog.PlayerCount))]
        public int PlayerCount { get { return 1; } }

        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public int TotalPlayerCount { get { return 1; } }
    }
}