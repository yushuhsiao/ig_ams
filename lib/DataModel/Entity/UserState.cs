using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UserName = System.String;

namespace InnateGlory.Entity
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserState
    {
        [JsonProperty]
        public UserId UserId { get; set; }

        [JsonProperty]
        public string CorpName { get; set; }

        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public string DisplayName { get; set; }
    }
}