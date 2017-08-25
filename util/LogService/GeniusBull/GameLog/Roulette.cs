using ams;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("RouletteGame"), FieldName(CreateTime = "CreateTime", GroupID = "RouletteGameId")]
    public partial class RouletteGame : _LogBase<RouletteGame, RouletteBetLog>.Grp//_xxxxGame
    {
        #region Fields

        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("TableId")]
        public int TableId { get; set; }

        [DbImport("ShoeNumber")]
        public int ShoeNumber { get; set; }

        [DbImport("GameNumber")]
        public int GameNumber { get; set; }

        [DbImport("Number")]
        public int Number { get; set; }

        [DbImport("RoadTip")]
        public string RoadTip { get; set; }

        [DbImport("IsBlackWin")]
        public bool IsBlackWin { get; set; }

        [DbImport("IsRedWin")]
        public bool IsRedWin { get; set; }

        [DbImport("IsOddWin")]
        public bool IsOddWin { get; set; }

        [DbImport("IsEvenWin")]
        public bool IsEvenWin { get; set; }

        [DbImport("IsBigWin")]
        public bool IsBigWin { get; set; }

        [DbImport("IsSmallWin")]
        public bool IsSmallWin { get; set; }

        [DbImport("IsZeroWin")]
        public bool IsZeroWin { get; set; }

        [DbImport("IsResult")]
        public bool IsResult { get; set; }

        [DbImport("CreateTime")]
        public override DateTime CreateTime { get; set; }

        #endregion

        public override string SerialNumber
        {
            get;
            set;
        }

        public override int PlayerCount { get { throw new NotImplementedException(); } }

        public override int TotalPlayerCount { get { throw new NotImplementedException(); } }
     
        //internal override string GroupID_FieldName { get { return "RouletteGameId"; } }

        internal override int GetGameID(grp_cache grps = null) => this.TableId;
    }
    [ams.TableName("RouletteBetLog"), FieldName(CreateTime = "CreateTime")]
    public partial class RouletteBetLog : _LogBase<RouletteGame, RouletteBetLog>.Bet//_xxxxBet<RouletteGame>
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("RouletteGameId")]
        public override long GroupID { get; set; }

        [DbImport("MemberId")]
        public int MemberId { get; set; }

        [DbImport("Bets")]
        public string Bets { get; set; }

        [DbImport("Wins")]
        public string Wins { get; set; }

        [DbImport("GoldCoinBet")]
        public decimal GoldCoinBet { get; set; }

        [DbImport("FreeCoinBet")]
        public decimal FreeCoinBet { get; set; }

        [DbImport("GoldCoinWin")]
        public decimal GoldCoinWin { get; set; }

        [DbImport("FreeCoinWin")]
        public decimal FreeCoinWin { get; set; }

        [DbImport("TotalBet")]
        public decimal TotalBet { get; set; }

        [DbImport("TotalWin")]
        public decimal TotalWin { get; set; }

        [DbImport("OutcomeAmount")]
        public decimal OutcomeAmount { get; set; }

        [DbImport("GoldCoinBudget")]
        public decimal GoldCoinBudget { get; set; }

        [DbImport("FreeCoinBudget")]
        public decimal FreeCoinBudget { get; set; }

        [DbImport("CreateTime")]
        public override DateTime CreateTime { get; set; }

        #endregion

        internal override int GetPlayerID() => this.MemberId;
    }
}