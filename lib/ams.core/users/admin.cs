using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using ams.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web;
using ams.Controllers;
using ams.Data;

namespace ams.Data
{
    [Flags]
    public enum AdminActiveFlag : byte
    {
        Disabled = ActiveState.Disabled,
        Active = ActiveState.Active,
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [UserData(UserType = UserType.Admin), ams.TableName("Admins", SortField = nameof(CreateTime))]
    public class AdminData : UserData<AdminData>
    {
        public AdminData(CorpInfo corpInfo) : base(corpInfo) { }

        [DbImport("Active")]
        AdminActiveFlag Active;

        [JsonProperty("Active")]
        public bool AccountActive
        {
            get { return this.Active.HasFlag(AdminActiveFlag.Active); }
        }
    }
}