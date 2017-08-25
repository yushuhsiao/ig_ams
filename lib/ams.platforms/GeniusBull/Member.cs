using ams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeniusBull
{
    [TableName("Member")]
    public partial class Member
    {
        public static readonly SqlBuilder.str TableName = "Member";
        public Member Parent;
        [DbImport]
        public int Id;
        [DbImport]
        public int ParentId;
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
        public MemberType Type;
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
        [DbImport]
        public DateTime RegisterTime;
        [DbImport]
        public string LastLoginIp;
        [DbImport]
        public DateTime? LastLoginTime;
        [DbImport]
        public string AccessToken;
    }
}
