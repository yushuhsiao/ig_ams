using Newtonsoft.Json;
using System.Data;

namespace InnateGlory.Entity
{
    [TableName("UserBalance", Database = _Consts.db.UserDB, SortKey = nameof(Id))]
    public class UserBalance
    {
        [DbImport]
        public UserId Id { get; set; }

        [DbImport]
        public SqlTimeStamp ver { get; set; }
        
        /// <summary>
        /// 額度1,現金額度
        /// </summary>
        [DbImport]
        public decimal Balance1 { get; set; }
        
        /// <summary>
        /// 額度2,信用額度
        /// </summary>
        [DbImport]
        public decimal Balance2 { get; set; }
        
        /// <summary>
        /// 額度3
        /// </summary>
        [DbImport]
        public decimal Balance3 { get; set; }
        
        /// <summary>
        /// 總額度
        /// </summary>
        [DbImport]
        public decimal Balance { get; set; }
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class UserBalanceModel
    {
        [JsonProperty]
        public CorpId? CorpId { get; set; }

        [JsonProperty]
        public UserName CorpName { get; set; }

        /// <summary>
        /// 相對值
        /// </summary>
        [JsonProperty]
        public decimal? Amount1 { get; set; }

        /// <summary>
        /// 相對值
        /// </summary>
        [JsonProperty]
        public decimal? Amount2 { get; set; }

        /// <summary>
        /// 相對值
        /// </summary>
        [JsonProperty]
        public decimal? Amount3 { get; set; }

        /// <summary>
        /// 絕對值
        /// </summary>
        [JsonProperty]
        public decimal? Balance1 { get; set; }

        /// <summary>
        /// 絕對值
        /// </summary>
        [JsonProperty]
        public decimal? Balance2 { get; set; }

        /// <summary>
        /// 絕對值
        /// </summary>
        [JsonProperty]
        public decimal? Balance3 { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CorpBalanceModel : UserBalanceModel
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentBalanceModel : UserBalanceModel
    {
        [JsonProperty]
        public UserId AgentId { get; set; }

        [JsonProperty]
        public UserName AgentName { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberBalanceModel : UserBalanceModel
    {
        [JsonProperty]
        public UserId MemberId { get; set; }

        [JsonProperty]
        public UserName MemberName { get; set; }
    }
}
