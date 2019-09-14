using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GetUserModel
    {
        [JsonProperty]
        public CorpId? CorpId { get; set; }

        [JsonProperty]
        public UserName CorpName { get; set; }

        [JsonProperty]
        public UserId? Id { get; set; }

        [JsonProperty]
        public UserName Name { get; set; }
    }
}
