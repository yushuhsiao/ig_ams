using IG.Dal;
using System;
using System.Data;

namespace IG.Lobby.TG
{
    public partial class MemberRow
    {
        [DbImport]
        public int Id;
        [DbImport]
        public int ParentId;
        [DbImport]
        public int OwnerId;
        [DbImport]
        public string Account;          // varchar(50)
        [DbImport]
        public string Password;
        [DbImport]
        public string Nickname;         // nvarchar(50)
        [DbImport]
        public decimal Balance;
        [DbImport]
        public decimal CreditAmount;
        [DbImport]
        public int Stock;
        [DbImport]
        public MemberRole Role;
        [DbImport]
        public MemberStatus Status;
        [DbImport]
        public string Email;
        [DbImport]
        public string BankCode;
        [DbImport]
        public string BankAccount;
        [DbImport]
        public string BankUsername;
        [DbImport]
        public bool IsEmailValid;
        //[DbImport]
        //public DateTime RegisterTime;
        //[DbImport]
        //public string LastLoginIp;
        //[DbImport]
        //public DateTime? LastLoginTime;
        [DbImport]
        public string AccessToken;
    }

    public class MemberJoinTableRow
    {
        [DbImport]
        public int PlayerId;
        [DbImport]
        public int GameId;
        [DbImport]
        public int OwnerId;
        [DbImport]
        public int TableId;
        [DbImport]
        public int State;
        [DbImport]
        public DateTime JoinTime;
        [DbImport]
        public DateTime JoinExpire;
        [DbImport]
        public DateTime CurrentTime;
    }
}