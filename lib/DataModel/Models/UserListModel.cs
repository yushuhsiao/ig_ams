using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserListModel
    {
        [JsonProperty]
        public UserId ParentId { get; set; }

        [Required]
        [JsonProperty]
        public bool? All { get; set; }

        [JsonProperty]
        public PagingModel Paging { get; set; }
    }
}
