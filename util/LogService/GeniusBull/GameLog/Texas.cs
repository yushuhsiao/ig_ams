using ams;
using ams.Data;
using LogService;
using System;
using System.Data;
using System.Linq;

namespace GeniusBull
{
    [TableName("TexasGame"), FieldName(CreateTime = "InsertDate", GroupID = "TexasGameId", Finished = nameof(IsResult))]
    public partial class TexasGame : _LogBase<TexasGame, TexasBet>.Grp, IGameReplay//, IFinished
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

        [DbImport("DealerSeat")]
        //[GameLog(nameof(GameLog.DealerSeat))]
        public int DealerSeat { get; set; }

        [DbImport("SmallBlind")]
        [GameLog(nameof(GameLog.SmallBaseValue))]// [GameLog(nameof(GameLog.SmallBlind))]
        public int SmallBlind { get; set; }

        [DbImport("BigBlind")]
        [GameLog(nameof(GameLog.BaseValue))] //[GameLog(nameof(GameLog.BigBlind))]
        public int BigBlind { get; set; }

        [DbImport("Cards")]
        [GameLog(nameof(GameLog.Cards))]
        public string Cards { get; set; }

        [DbImport("TotalPlayer")]
        [GameLog(nameof(GameLog.TotalPlayerCount))]
        public int TotalPlayer { get; set; }

        [DbImport("ActivePlayer")]
        [GameLog(nameof(GameLog.PlayerCount))]
        public int ActivePlayer { get; set; }

        public override int PlayerCount { get { return ActivePlayer; } }
        public override int TotalPlayerCount { get { return TotalPlayer; } }

        [DbImport("GameLog")]
        public string GameReplay { get; set; }

        [DbImport("IsResult")]
        public bool IsResult { get; set; }

        [DbImport("InsertDate")]
        [GameLog(nameof(GameLog.PlayStartTime))]
        public override DateTime CreateTime { get; set; }

        #endregion

        //internal override string GroupID_FieldName { get { return "TexasGameId"; } }

        //string IFinished.FieldName { get { return nameof(IsResult); } }
        //bool IFinished.IsFinished { get { return IsResult; } }
        public override bool IsFinished { get { return IsResult; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;

        public decimal? TotalFees { get { return Players?.Sum((n) => n.Fee); } }
    }
    [TableName("TexasBet"), FieldName(CreateTime = "InsertDate")]
    public partial class TexasBet : _LogBase<TexasGame, TexasBet>.Bet
    {
        #region Fields

        [DbImport("Id")]
        [GameLog(nameof(GameLog.GroupBetID))]
        public override long Id { get; set; }

        [DbImport("TexasGameId")]
        [GameLog(nameof(GameLog.GroupID))]
        public override long GroupID { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("Seat")]
        [GameLog(nameof(GameLog.SeatPosition))]//[GameLog(nameof(GameLog.Seat))]
        public int Seat { get; set; }

        [DbImport("FirstCard")]
        [GameLog(nameof(GameLog.FirstCard))]
        public string FirstCard { get; set; }

        [DbImport("SecondCard")]
        [GameLog(nameof(GameLog.SecondCard))]
        public string SecondCard { get; set; }

        [DbImport("Bets")]
        [GameLog(nameof(GameLog.Bets))]
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

        [GameLog(nameof(GameLog.IsBanker))]
        public bool IsBanker
        {
            get { return this.GetGroupRow()?.DealerSeat == this.Seat; }
        }

        [GameLog(nameof(GameLog.TotalFees), ApplyExchangeRate = true)]
        public decimal? TotalFees
        {
            get
            {
                if (this.BetAmount == 0 && this.WinAmount == 0) return 0;
                else return this.GetGroupRow()?.TotalFees;
            }
        }
    }
}