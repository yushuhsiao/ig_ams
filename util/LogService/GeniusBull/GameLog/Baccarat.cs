using ams;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("BaccaratGame"), FieldName(CreateTime = "CreateTime", GroupID = "BaccaratGameId")]
    public partial class BaccaratGame : _LogBase<BaccaratGame, BaccaratBetLog>.Grp //_xxxxGame
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

        [DbImport("BankerFirstCard")]
        public string BankerFirstCard { get; set; }

        [DbImport("BankerSecondCard")]
        public string BankerSecondCard { get; set; }

        [DbImport("BankerThirdCard")]
        public string BankerThirdCard { get; set; }

        [DbImport("PlayerFirstCard")]
        public string PlayerFirstCard { get; set; }

        [DbImport("PlayerSecondCard")]
        public string PlayerSecondCard { get; set; }

        [DbImport("PlayerThirdCard")]
        public string PlayerThirdCard { get; set; }

        [DbImport("RoadTip")]
        public string RoadTip { get; set; }

        [DbImport("IsBankerWin")]
        public bool IsBankerWin { get; set; }

        [DbImport("IsPlayerWin")]
        public bool IsPlayerWin { get; set; }

        [DbImport("IsTieWin")]
        public bool IsTieWin { get; set; }

        [DbImport("IsBankerPairWin")]
        public bool IsBankerPairWin { get; set; }

        [DbImport("IsPlayerPairWin")]
        public bool IsPlayerPairWin { get; set; }

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

        //internal override string GroupID_FieldName { get { return "BaccaratGameId"; } }

        internal override int GetGameID(grp_cache grps = null) { throw new NotImplementedException(); }
    }
    [ams.TableName("BaccaratBetLog"), FieldName(CreateTime = "CreateTime")]
    public partial class BaccaratBetLog : _LogBase<BaccaratGame, BaccaratBetLog>.Bet //_xxxxBet<BaccaratGame>
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("BaccaratGameId")]
        public override long GroupID { get; set; }

        [DbImport("MemberId")]
        public int MemberId { get; set; }

        [DbImport("BankerBet")]
        public decimal BankerBet { get; set; }

        [DbImport("PlayerBet")]
        public decimal PlayerBet { get; set; }

        [DbImport("TieBet")]
        public decimal TieBet { get; set; }

        [DbImport("BankerPairBet")]
        public decimal BankerPairBet { get; set; }

        [DbImport("PlayerPairBet")]
        public decimal PlayerPairBet { get; set; }

        [DbImport("BankerWin")]
        public decimal BankerWin { get; set; }

        [DbImport("PlayerWin")]
        public decimal PlayerWin { get; set; }

        [DbImport("TieWin")]
        public decimal TieWin { get; set; }

        [DbImport("BankerPairWin")]
        public decimal BankerPairWin { get; set; }

        [DbImport("PlayerPairWin")]
        public decimal PlayerPairWin { get; set; }

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

        [DbImport("RakeType")]
        public sbyte RakeType { get; set; }

        [DbImport("RakeRatio")]
        public sbyte RakeRatio { get; set; }

        [DbImport("RakeAmount")]
        public decimal RakeAmount { get; set; }

        [DbImport("RakeResult")]
        public decimal RakeResult { get; set; }

        [DbImport("ResultAmount")]
        public decimal ResultAmount { get; set; }

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