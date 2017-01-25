using ams;
using ams.Data;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("FivePK"), FieldName(CreateTime = "InsertDate", Finished = nameof(GameFinished))]
    public partial class FivePK : _LogBase_JP<FivePK>//, IFinished
    {
        #region Fields

        [DbImport("Id")]
        [GameLog(nameof(GameLog.GroupBetID))]
        public override long Id { get; set; }

        [DbImport("SerialNumber")]
        [GameLog(nameof(GameLog.SerialNumber))]
        public override string SerialNumber { get; set; }

        [GameLog(nameof(GameLog.sn1))]
        public override string sn1 { get { return base.sn1; } }
        //public string sn1 { get { return util.sn1(this.SerialNumber); } }
        [GameLog(nameof(GameLog.sn2))]
        public override string sn2 { get { return base.sn2; } }
        //public string sn2 { get { return util.sn2(this.SerialNumber); } }

        [DbImport("PlayerId")]
        public override int PlayerId { get; set; }

        [DbImport("GameId")]
        public override int GameId { get; set; }

        [DbImport("ActionType")]
        [GameLog(nameof(GameLog.ActionType))]
        public string ActionType { get; set; }

        [DbImport("GameType")]
        [GameLog(nameof(GameLog.GameType))]
        public override string GameType { get; set; }

        [DbImport("GameFinished")]
        public bool GameFinished { get; set; }

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
        public override string JPType { get; set; }

        [DbImport("BetAmount")]
        [GameLog(nameof(GameLog.BetAmount), ApplyExchangeRate = true)]
        public override decimal BetAmount { get; set; }

        [DbImport("WinAmount")]
        [GameLog(nameof(GameLog.WinAmount), ApplyExchangeRate = true)]
        public override decimal WinAmount { get; set; }

        [DbImport("Balance")]
        [GameLog(nameof(GameLog.Balance), ApplyExchangeRate = true)]
        public decimal Balance { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayEndTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        //[GameLog(nameof(GameLog.JPAmount), ApplyExchangeRate = true)]
        //public override decimal? JPAmount
        //{
        //    get { return base.JPAmount; }
        //}

        [GameLog(nameof(GameLog.JP_Total), ApplyExchangeRate = true)]
        public override decimal JP_Total
        {
            get { return base.JP_Total; }
        }

        [GameLog(nameof(GameLog.JP_GRAND), ApplyExchangeRate = true)]
        public override decimal? JP_GRAND
        {
            get { return base.JP_GRAND; }
        }

        [GameLog(nameof(GameLog.JP_MAJOR), ApplyExchangeRate = true)]
        public override decimal? JP_MAJOR
        {
            get { return base.JP_MAJOR; }
        }

        [GameLog(nameof(GameLog.JP_MINI), ApplyExchangeRate = true)]
        public override decimal? JP_MINI
        {
            get { return base.JP_MINI; }
        }

        [GameLog(nameof(GameLog.JP_MINOR), ApplyExchangeRate = true)]
        public override decimal? JP_MINOR
        {
            get { return base.JP_MINOR; }
        }

        //string IFinished.FieldName { get { return nameof(GameFinished); } }
        //bool IFinished.IsFinished { get { return this.GameFinished; } }
        public override bool IsFinished { get { return this.GameFinished; } }

        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get { return this.Id; } set { this.Id = value; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;

        internal override int GetPlayerID() => this.PlayerId;

        [GameLog(nameof(GameLog.PlayerCount))]
        public int PlayerCount { get { return 1; } }

        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public int TotalPlayerCount { get { return 1; } }
    }
}