using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InnateGlory
{
    [DebuggerDisplay("{Id}")]
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class amsUser : IUser
    {
        private DataService _dataService;
        public amsUser(DataService dataService)
        {
            _dataService = dataService;
        }

        //[JsonProperty]
        public UserId Id { get; set; }



        private Entity.UserData _userData;
        public Entity.UserData UserData => _userData = _userData ?? _dataService.Users.GetUser(this.Id);

        //public UserName CorpName => _corpName ?? default(UserName);
        //public UserName UserName => _userName ?? default(UserName);

        //[JsonProperty(nameof(CorpName))]
        //private UserName? _corpName => _dataService.Corps.Get(Id.CorpId)?.Name;

        //[JsonProperty(nameof(UserName))]
        //private UserName? _userName => UserData?.Name;

        //[JsonProperty]
        //public string DisplayName => UserData?.DisplayName;
    }
}
