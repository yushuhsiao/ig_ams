using ams;
using ams.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace RecogService
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TakePictureSession
    {
        [JsonProperty]
        public UserID? CorpID;
        [JsonProperty]
        public UserID? MemberID;
        [JsonProperty]
        public string TakePictureKey;

        public static void StringSet(string accessToken, TakePictureSession value)
        {
            foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
                redis.StringSet(
                    key: $"recog:{accessToken}",
                    value: json.SerializeObject(value),
                    expiry: TimeSpan.FromMinutes(10));
        }

        public static TakePictureSession StringGet(string accessToken)
        {
            foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
            {
                string s = redis.StringGet($"recog:{accessToken}");
                try { return json.DeserializeObject<TakePictureSession>(s); }
                catch { }
            }
            return null;
        }

        public void StringSet(string accessToken) => StringSet(accessToken, this);

        public MemberData GetMemberData(bool err = false)
        {
            if (this.CorpID.HasValue && this.MemberID.HasValue)
                return CorpInfo.GetCorpInfo(this.CorpID.Value, err: err)?.GetMemberData(id: this.MemberID, err: err);
            return null;
        }
    }
}
