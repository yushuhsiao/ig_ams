using Newtonsoft.Json;
using System;

namespace RecogService
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PictureInformation
    {
        [JsonProperty]
        public string UserName;
        [JsonProperty]
        public Guid? ImageID;
        [JsonProperty]
        public ImageType? ImageType;
        [JsonProperty]
        public bool? Success;
        [JsonProperty]
        public DateTime? CreateTime;
        [JsonProperty]
        public string Url;
        [JsonProperty]
        public float? Similarity;
        [JsonProperty]
        public double? TTL;
    }
}
