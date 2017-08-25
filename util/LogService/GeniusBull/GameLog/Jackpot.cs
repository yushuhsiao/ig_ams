using ams;
using ams.Data;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("JackpotLog"), FieldName(CreateTime = "InsertDate")]
    public partial class JackpotLog : _LogBase<JackpotLog>
    {
        #region Fields

        [DbImport("Id")]
        [GameLog(nameof(GameLog.GroupBetID))]
        public override long Id { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

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

        [DbImport("JackpotType")]
        [GameLog(nameof(GameLog.JPType))]
        public string JackpotType { get; set; }

        [DbImport("Jackpot")]
        [GameLog(nameof(GameLog.Jackpot), ApplyExchangeRate = true)]
        public decimal Jackpot { get; set; }

        [DbImport("Base")]
        [GameLog(nameof(GameLog.Base))]
        public int Base { get; set; }

        [DbImport("Ratio")]
        [GameLog(nameof(GameLog.Ratio))]
        public decimal Ratio { get; set; }

        [DbImport("WinAmount")]
        [GameLog(nameof(GameLog.WinAmount), ApplyExchangeRate = true)]
        public decimal WinAmount { get; set; }

        [DbImport("BaseAmount")]
        [GameLog(nameof(GameLog.BaseAmount), ApplyExchangeRate = true)]
        public decimal BaseAmount { get; set; }

        [DbImport("FillAmount")]
        [GameLog(nameof(GameLog.FillAmount), ApplyExchangeRate = true)]
        public decimal FillAmount { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayEndTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get; set; }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;

        internal override int GetPlayerID() => this.PlayerId;

        [GameLog(nameof(GameLog.PlayerCount))]
        public int PlayerCount { get { return 1; } }

        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public int TotalPlayerCount { get { return 1; } }
    }

    [ams.TableName("JackpotUpdateLog"), FieldName(CreateTime = "InsertDate")]
    public partial class JackpotUpdateLog : _LogBase<JackpotUpdateLog>
    {
        #region Fields

        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("SerialNumber")]
        public override string SerialNumber { get; set; }

        [DbImport("JackpotType")]
        public string JackpotType { get; set; }

        [DbImport("PushAmount")]
        public decimal PushAmount { get; set; }

        [DbImport("InsertDate")]
        public override DateTime CreateTime { get; set; }

        #endregion

        public override long GroupID { get { return 0; } set { } }

        internal override int GetGameID(grp_cache grps = null) => GameId;
    }
}