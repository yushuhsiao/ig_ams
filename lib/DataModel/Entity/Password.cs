using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class PasswordBase
    {
        [DbImport]
        public UserId UserId { get; set; }

        /// <summary>
        /// Encrypt Type
        /// </summary>
        [DbImport("Encrypt")]
        public int Type { get; set; }

        [DbImport("a")]
        public string Ciphertext { get; set; }

        [DbImport("b")]
        public string Password { get; set; }

        [DbImport("c")]
        public string Salt { get; set; }

        /// <summary>
        /// 密碼過期時間
        /// </summary>
        [DbImport("Expiry")]
        public int? Expiry { get; set; }

        /// <summary>
        /// 密碼到期日
        /// </summary>
        public DateTime? ExipreTime
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
                DateTime? t = this.ExipreTime;
                if (t.HasValue)
                    return DateTime.Now >= t.Value;
                return false;
            }
        }

        [DbImport]
        public DateTime CreateTime { get; set; }

        [DbImport]
        public UserId CreateUser { get; set; }
    }
}
namespace InnateGlory.Entity
{
    [TableName("Password", Database = _Consts.db.UserDB)]
    public class Password : Abstractions.PasswordBase
    {
        [DbImport("ver")]
        public SqlTimeStamp Version { get; set; }
    }

    [TableName("PasswordHist", Database = _Consts.db.UserDB)]
    public class PasswordHist : Abstractions.PasswordBase
    {
        [DbImport("ver")]
        public long Version { get; set; }
    }
}
