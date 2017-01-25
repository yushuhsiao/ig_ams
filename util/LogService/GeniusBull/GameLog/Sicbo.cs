using ams;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("SicboGame"), FieldName(CreateTime = "CreateTime", GroupID = "SicboGameId")]
    public partial class SicboGame : _LogBase<SicboGame, SicboBetLog>.Grp//_xxxxGame
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

        [DbImport("Dice_1")]
        public int Dice_1 { get; set; }

        [DbImport("Dice_2")]
        public int Dice_2 { get; set; }

        [DbImport("Dice_3")]
        public int Dice_3 { get; set; }

        [DbImport("RoadTip")]
        public string RoadTip { get; set; }

        [DbImport("IsBigWin")]
        public bool IsBigWin { get; set; }

        [DbImport("IsSmallWin")]
        public bool IsSmallWin { get; set; }

        [DbImport("IsOddWin")]
        public bool IsOddWin { get; set; }

        [DbImport("IsEvenWin")]
        public bool IsEvenWin { get; set; }

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
        //internal override string GroupID_FieldName { get { return "SicboGameId"; } }

        internal override int GetGameID(grp_cache grps = null) { throw new NotImplementedException(); }
    }
    [ams.TableName("SicboBetLog"), FieldName(CreateTime = "CreateTime")]
    public partial class SicboBetLog : _LogBase<SicboGame, SicboBetLog>.Bet//_xxxxBet<SicboGame>
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("SicboGameId")]
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