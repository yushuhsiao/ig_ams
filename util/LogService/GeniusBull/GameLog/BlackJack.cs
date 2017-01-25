using ams;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("BlackJackGame"), FieldName(CreateTime = "InsertDate", GroupID = "BlackJackGameId")]
    public partial class BlackJackGame : _LogBase<BlackJackGame, BlackJackBet>.Grp //_xxxxGame
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("SerialNumber")]
        public override string SerialNumber { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("Balance")]
        public decimal Balance { get; set; }

        [DbImport("InsertDate")]
        public override DateTime CreateTime { get; set; }

        #endregion

        public override int PlayerCount { get { throw new NotImplementedException(); } }

        public override int TotalPlayerCount { get { throw new NotImplementedException(); } }
    
        //internal override string GroupID_FieldName { get { return "BlackJackGameId"; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;
    }

    [ams.TableName("BlackJackBet"), FieldName(CreateTime = "InsertDate")]
    public partial class BlackJackBet : _LogBase<BlackJackGame, BlackJackBet>.Bet//_xxxxBet<BlackJackGame>
    {
        #region Fields

        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("BlackJackGameId")]
        public override long GroupID { get; set; }

        [DbImport("Seat")]
        public int Seat { get; set; }

        [DbImport("Level")]
        public int Level { get; set; }

        [DbImport("NormalBet")]
        public decimal NormalBet { get; set; }

        [DbImport("PairBet")]
        public decimal PairBet { get; set; }

        [DbImport("InsuranceBet")]
        public decimal InsuranceBet { get; set; }

        [DbImport("WinAmount")]
        public decimal WinAmount { get; set; }

        [DbImport("IsDouble")]
        public bool IsDouble { get; set; }

        [DbImport("IsSurrender")]
        public bool IsSurrender { get; set; }

        [DbImport("IsGameFinished")]
        public bool IsGameFinished { get; set; }

        [DbImport("IsBlackJack")]
        public bool IsBlackJack { get; set; }

        [DbImport("Cards")]
        public string Cards { get; set; }

        [DbImport("InsertDate")]
        public override DateTime CreateTime { get; set; }

        #endregion
    }
}