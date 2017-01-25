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

// ToDo : 根據目前使用者身分做限制
namespace ams.Data
{
    [Flags]
    public enum AgentActiveFlag : byte
    {
        Disabled = ActiveState.Disabled,
        Accounts = 1 << 0,
        Game = 1 << 1,
        MaxDepthEnabled = 1 << 4,
        MaxAgentEnabled = 1 << 5,
        MaxAdminEnabled = 1 << 6,
        MaxMemberEnabled = 1 << 7,
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [UserData(UserType = UserType.Agent, Balance = "AgentBalance"), ams.TableName("Agents", SortField = nameof(CreateTime))]
    public class AgentData : UserData<AgentData>
    {
        public AgentData(CorpInfo corpInfo) : base(corpInfo) { }

        #region Properties

        [DbImport("MaxDepth")]
        int maxDepth;
        [JsonProperty]
        public int MaxDepth
        {
            get { if (this.ID.IsRoot) return 0; return Math.Max(0, this.maxDepth); }
        }
        [JsonProperty]
        public bool MaxDepthEnabled
        {
            get { if (this.ID.IsRoot) return false; return this.Active.HasFlag(AgentActiveFlag.MaxDepthEnabled); }
        }

        [DbImport("MaxAgent")]
        int maxAgent;
        [JsonProperty]
        public int MaxAgent
        {
            get { if (this.ID.IsRoot) return 0; return Math.Max(0, this.maxAgent); }
        }
        [JsonProperty]
        public bool MaxAgentEnabled
        {
            get { if (this.ID.IsRoot) return true; return this.Active.HasFlag(AgentActiveFlag.MaxAgentEnabled); }
        }

        [DbImport("MaxAdmin")]
        int maxAdmin;
        [JsonProperty]
        public int MaxAdmin
        {
            get { if (this.ID.IsRoot) return 0; return Math.Max(0, this.maxAdmin); }
        }
        [JsonProperty]
        public bool MaxAdminEnabled
        {
            get { if (this.ID.IsRoot) return false; return this.Active.HasFlag(AgentActiveFlag.MaxAdminEnabled); }
        }

        [DbImport("MaxMember")]
        int maxMember;
        [JsonProperty]
        public int MaxMember
        {
            get { if (this.ID.IsRoot) return 0; return Math.Max(0, this.maxMember); }
        }
        [JsonProperty]
        public bool MaxMemberEnabled
        {
            get { if (this.ID.IsRoot) return true; return this.Active.HasFlag(AgentActiveFlag.MaxMemberEnabled); }
        }

        #endregion

        [DbImport("Active")]
        AgentActiveFlag Active;

        [JsonProperty("Active1")]
        public bool AccountsActive
        {
            get { if (this.ID.IsRoot) return true; return this.Active.HasFlag(AgentActiveFlag.Accounts); }
        }

        [JsonProperty("Active2")]
        public bool GameDisabled
        {
            get { if (this.ID.IsRoot) return false; return !this.Active.HasFlag(AgentActiveFlag.Game); }
        }
    }
}