using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class PasswordBase
    {
        internal int Encrypt;
        internal string a;
        internal string b;
        internal string c;

        public UserId UserId { get; set; }

        /// <summary>
        /// Encrypt Type
        /// </summary>
        public int Type
        {
            get => Encrypt;
            set => Encrypt = value;
        }

        public string Ciphertext
        {
            get => a;
            set => a = value;
        }

        public string Password
        {
            get => b;
            set => b = value;
        }

        public string Salt
        {
            get => c;
            set => c = value;
        }

        /// <summary>
        /// 密碼過期時間
        /// </summary>
        public int? Expiry { get; set; }

        /// <summary>
        /// 密碼到期日
        /// </summary>
        public DateTime? ExpireTime
        {
            get
            {
                if (Expiry.HasValue)
                    return CreateTime.AddMinutes(Expiry.Value);
                else
                    return null;
            }
        }

        public bool IsExpire
        {
            get
            {
                DateTime? t = this.ExpireTime;
                if (t.HasValue)
                    return DateTime.Now >= t.Value;
                return false;
            }
        }

        public DateTime CreateTime { get; set; }

        public UserId CreateUser { get; set; }
    }
}
namespace InnateGlory.Entity
{
    [TableName("Password", Database = _Consts.db.UserDB)]
    public class Password : Abstractions.PasswordBase
    {
        internal byte[] ver;

        public SqlTimeStamp Version => (SqlTimeStamp)ver;
    }

    [TableName("PasswordHist", Database = _Consts.db.UserDB)]
    public class PasswordHist : Abstractions.PasswordBase
    {
        internal long ver;

        public long Version => ver;
    }
}
