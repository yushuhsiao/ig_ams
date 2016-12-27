using Newtonsoft.Json;
using System.Data;

namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserBalance
    {
        public static readonly UserBalance Null = new UserBalance();

        [JsonProperty, DbImport("ID")]
        public UserID ID;

        [DbImport("ver")]
        public SqlTimeStamp RowVersion;

        [JsonProperty]
        public UserName PlatformName;

        /// <summary>
        /// 已使用額度
        /// </summary>
        [JsonProperty, DbImport("b1")]
        public decimal Balance1; // n=輸贏值*占成比, b2+=n, b1-=輸贏值

        /// <summary>
        /// 現金額度
        /// </summary>
        [JsonProperty, DbImport("b2")]
        public decimal Balance2;

        /// <summary>
        /// 信用額度
        /// </summary>
        [JsonProperty, DbImport("b3")]
        public decimal Balance3;

        [JsonProperty]
        public decimal Balance
        {
            get { return Balance1 + Balance2 + Balance3; }
        }
    }

    //public class UserUpdateBalance : UserBalance
    //{
    //    internal UserData user;

    //    [DbImport("_ver")]
    //    public SqlTimeStamp PrevRowVersion;

    //    [DbImport("_b1")]
    //    public decimal PrevBalance1;

    //    [DbImport("_b2")]
    //    public decimal PrevBalance2;

    //    [DbImport("_b3")]
    //    public decimal PrevBalance3;

    //    public decimal PrevBalance
    //    {
    //        get { return PrevBalance1 + PrevBalance2 + PrevBalance3; }
    //    }

    //    public bool IsUpdated
    //    {
    //        get { return (this.PrevBalance1 != this.Balance1) || (this.PrevBalance2 != this.Balance2) || (this.PrevBalance3 != this.Balance3); }
    //    }
    //}
}