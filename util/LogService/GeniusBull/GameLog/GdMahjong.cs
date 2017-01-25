using ams;
using LogService;
using System;
using System.Data;

namespace GeniusBull
{
    [ams.TableName("GdMahjongGame"), FieldName(CreateTime = "InsertDate", GroupID = "MahjongGameId")]
    public partial class GdMahjongGame : _LogBase<GdMahjongGame, GdMahjongBet>.Grp //_xxxxGame
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("GameId")]
        public int GameId { get; set; }

        [DbImport("SerialNumber")]
        public override string SerialNumber { get; set; }

        [DbImport("Antes")]
        public int Antes { get; set; }

        [DbImport("Tai")]
        public int Tai { get; set; }

        [DbImport("RoundType")]
        public sbyte RoundType { get; set; }

        [DbImport("ServiceCharge")]
        public int ServiceCharge { get; set; }

        [DbImport("InsertDate")]
        public override DateTime CreateTime { get; set; }

        #endregion

        public override int PlayerCount { get { throw new NotImplementedException(); } }

        public override int TotalPlayerCount { get { throw new NotImplementedException(); } }

        //internal override string GroupID_FieldName { get { return "MahjongGameId"; } }

        internal override int GetGameID(grp_cache grps = null) => this.GameId;
    }
    [ams.TableName("GdMahjongBet"), FieldName(CreateTime = "InsertDate")]
    public partial class GdMahjongBet : _LogBase<GdMahjongGame, GdMahjongBet>.Bet //_xxxxBet<GdMahjongGame>
    {
        #region Fields
        [DbImport("Id")]
        public override long Id { get; set; }

        [DbImport("MahjongGameId")]
        public override long GroupID { get; set; }

        [DbImport("PlayerId")]
        public int PlayerId { get; set; }

        [DbImport("Fee")]
        public decimal Fee { get; set; }

        [DbImport("BetAmount")]
        public decimal BetAmount { get; set; }

        [DbImport("WinAmount")]
        public decimal WinAmount { get; set; }

        [DbImport("Balance")]
        public decimal Balance { get; set; }

        [DbImport("InsertDate")]
        public override DateTime CreateTime { get; set; }

        #endregion

        internal override int GetPlayerID() => this.PlayerId;
    }
}