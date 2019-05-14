using Newtonsoft.Json;
using System.Data;

namespace InnateGlory.Entity
{
    [TableName("Members", Database = _Consts.db.UserDB, SortKey = nameof(Id))]
    public class Member : UserData
    {
        public override UserType UserType => UserType.Member;
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberModel
    {
        [JsonProperty]
        public CorpId? CorpId { get; set; }

        [JsonProperty]
        public UserName CorpName { get; set; }

        [JsonProperty]
        public UserId? ParentId { get; set; }

        [JsonProperty]
        public UserName ParentName { get; set; }

        [JsonProperty]
        public UserId? Id { get; set; }

        [JsonProperty]
        public UserName Name { get; set; }

        [JsonProperty]
        public string DisplayName { get; set; }

        [JsonProperty]
        public ActiveState? Active { get; set; }
    }
}
