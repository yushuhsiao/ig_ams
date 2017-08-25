using ams;
using ams.Data;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("Oasis"), FieldName(CreateTime = "InsertDate", Finished = nameof(GameFinished))]
    public partial class Oasis : _LogBase<Oasis>//, IFinished
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
        public int PlayerId { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("BankerCards")]
        [GameLog(nameof(GameLog.BankerCards))]
        public string BankerCards { get; set; }

        [DbImport("PlayerCardsBef")]
        [GameLog(nameof(GameLog.PlayerCardsBef))]
        public string PlayerCardsBef { get; set; }

        [DbImport("PlayerCardsAft")]
        [GameLog(nameof(GameLog.PlayerCardsAft))]
        public string PlayerCardsAft { get; set; }

        [DbImport("ExchangeCost")]
        [GameLog(nameof(GameLog.ExchangeCost), ApplyExchangeRate = true)]
        public decimal ExchangeCost { get; set; }

        [DbImport("GameFinished")]
        public bool GameFinished { get; set; }

        [DbImport("Bets")]
        [GameLog(nameof(GameLog.Bets))]
        public string Bets { get; set; }

        [DbImport("Results")]
        [GameLog(nameof(GameLog.Results))]
        public string Results { get; set; }

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