using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Bridge;
#if wasm || jslib
using UserName = System.String;
#endif

namespace InnateGlory.Models
{
    //[NonScriptable]
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

        [JsonProperty]
        public bool? GetState { get; set; }
        //private int a;
    }
}
