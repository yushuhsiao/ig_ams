using ams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace RecogService
{
    [ams.TableName("RecogSession"), JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RecogSession
    {
        [DbImport]
        public Guid ID;
        [DbImport]
        public UserID CorpID;
        [DbImport]
        public UserID UserID;
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public DateTime? BeginTime;
        [DbImport]
        public DateTime? EndTime;


        [JsonProperty]
        public int RequestSimilarity;
        [JsonProperty]
        public Guid Session
        {
            get { return this.ID; }
        }
        [JsonProperty]
        public bool Finish
        {
            get { return this.EndTime.HasValue; }
        }
        [JsonProperty]
        public int? NumberOfMatch;
        [JsonProperty]
        public int? NumberOfItems;
        [JsonProperty]
        public List<string> MatchUsers;
        [JsonProperty]
        public List<PictureInformation> MatchUserDetails;
    }
}
