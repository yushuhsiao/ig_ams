using ams;
using ams.Data;
using LogService;
using System;
using System.Data;
using System.Linq;

namespace GeniusBull
{
    [ams.TableName("DouDizhuGame"), FieldName(CreateTime = "InsertDate", GroupID = "DouDizhuGameId", Finished = nameof(IsResult))]
    public partial class DouDizhuGame : _LogBase<DouDizhuGame, DouDizhuBet>.Grp, IGameReplay//, IFinished
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

        /// <summary>
        /// 底分
        /// </summary>
        [DbImport("BaseValue")]
        [GameLog(nameof(GameLog.BaseValue), ApplyExchangeRate = true)]
        public decimal BaseValue { get; set; }

        [DbImport("SnatchLord")]
        [GameLog(nameof(GameLog.SnatchLord))]
        public bool SnatchLord { get; set; }

        [DbImport("Fine")]
        [GameLog(nameof(GameLog.Fine))]
        public bool Fine { get; set; }

        [DbImport("MissionMode")]
        [GameLog(nameof(GameLog.MissionMode))]
        public bool MissionMode { get; set; }

        [DbImport("LordPlayerId")]
        public int LordPlayerId { get; set; }

        [DbImport("MissionType")]
        [GameLog(nameof(GameLog.MissionType))]
        public string MissionType { get; set; }

        [DbImport("CallMultiplier")]
        [GameLog(nameof(GameLog.CallMultiplier))]
        public int CallMultiplier { get; set; }

        [DbImport("FinalMultiplier")]
        [GameLog(nameof(GameLog.FinalMultiplier))]
        public int FinalMultiplier { get; set; }

        [DbImport("NumOfSpring")]
        [GameLog(nameof(GameLog.NumOfSpring))]
        public int NumOfSpring { get; set; }

        [DbImport("NumOfAntiSpring")]
        [GameLog(nameof(GameLog.NumOfAntiSpring))]
        public int NumOfAntiSpring { get; set; }

        [DbImport("NumOfBomb")]
        [GameLog(nameof(GameLog.NumOfBomb))]
        public int NumOfBomb { get; set; }

        [DbImport("NumOfRocket")]
        [GameLog(nameof(GameLog.NumOfRocket))]
        public int NumOfRocket { get; set; }

        [DbImport("MissionAccomplished")]
        [GameLog(nameof(GameLog.MissionAccomplished))]
        public bool MissionAccomplished { get; set; }

        [DbImport("GameLog")]
        public string GameReplay { get; set; }

        [DbImport("IsResult")]
        public bool IsResult { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayStartTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        //internal override string GroupID_FieldName { get { return "DouDizhuGameId"; } }

        //string IFinished.FieldName { get { return nameof(IsResult); } }
        //bool IFinished.IsFinished { get { return IsResult; } }
        public override bool IsFinished { get { return this.IsResult; } }

        [GameLog(nameof(GameLog.PlayerCount))]
        public override int PlayerCount { get { return 3; } }

        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public override int TotalPlayerCount { get { return 3; } }

        [GameLog(nameof(GameLog.TotalFees), ApplyExchangeRate = true)]
        public decimal? TotalFees { get { return Players?.Sum((n) => n.Fee); } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;
    }
    [ams.TableName("DouDizhuBet"), FieldName(CreateTime = "InsertDate")]
    public partial class DouDizhuBet : _LogBase<DouDizhuGame, DouDizhuBet>.Bet
    {
        #region Fields

        [DbImport("Id")]
        [GameLog(nameof(GameLog.GroupBetID))]
        public override long Id { get; set; }

        [DbImport("DouDizhuGameId")]
        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("IsLord")]
        [GameLog(nameof(GameLog.IsBanker))]
        public bool IsLord { get; set; }

        [DbImport("Bets")]
        [GameLog(nameof(GameLog.Bets), ApplyExchangeRate = true)]
        public string Bets { get; set; }

        [DbImport("Results")]
        [GameLog(nameof(GameLog.Results))]
        public string Results { get; set; }

        [DbImport("Fee")]
        [GameLog(nameof(GameLog.Fee), ApplyExchangeRate = true)]
        public decimal Fee { get; set; }

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