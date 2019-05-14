using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LoginModel
    {
        public DateTime Time = DateTime.Now;
        public UserId? UserId;
        public CorpId? CorpId;

        [JsonProperty/*, Required(ErrorMessage = "LoginType is null")*/]
        public UserType? LoginType { get; set; }

        [JsonProperty]
        public LoginMode? LoginMode { get; set; }

        [JsonProperty]
        public UserName CorpName { get; set; }

        [JsonProperty, Required]
        public UserName UserName { get; set; }

        [JsonProperty, Required]
        public string Password { get; set; }

        //[JsonProperty]
        //public bool? GetState { get; set; }
        //private int a;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LoginResult
    {
        [JsonProperty]
        public UserId? UserId { get; set; }

        [JsonProperty]
        public string AccessToken { get; set; }
    }
}
