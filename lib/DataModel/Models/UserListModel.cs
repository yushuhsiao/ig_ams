using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserListModel<T> : ListModel<T>
    {
        [JsonProperty]
        public UserId ParentId { get; set; }

        [JsonProperty]
        public bool? All { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ListModel<T>
    {
        [JsonProperty]
        public PagingModel<T> Paging { get; set; }
    }
}
