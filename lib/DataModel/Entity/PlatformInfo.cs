using InnateGlory.Entity.Abstractions;
using Newtonsoft.Json;
using System.Data;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲平台
    /// </summary>
    [TableName("Platforms", Database = _Consts.db.CoreDB)]
    public class PlatformInfo : BaseData
    {
        public PlatformId Id { get; set; }
        public UserName Name { get; set; }
        public PlatformType PlatformType { get; set; }
        public CurrencyCode Currency { get; set; }
        public PlatformActiveState Active { get; set; }
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformInfoModel
    {
        [JsonProperty]
        public PlatformId? PlatformId { get; set; }

        [JsonProperty]
        public UserName PlatformName { get; set; }

        [JsonProperty]
        public PlatformType? PlatformType { get; set; }
    }
}
