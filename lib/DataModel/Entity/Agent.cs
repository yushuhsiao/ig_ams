using Newtonsoft.Json;
using System.Data;

namespace InnateGlory.Entity
{
    [TableName("Agents", Database = _Consts.db.UserDB, SortKey = nameof(Id))]
    public class Agent : UserData
    {
        public override UserType UserType => UserType.Agent;

        #region Properties

        private int _MaxDepth;
        private int? _MaxAgents;
        private int? _MaxAdmins;
        private int? _MaxMembers;

        /// <summary>
        /// 最大層數
        /// </summary>
        [DbImport]
        public virtual int MaxDepth
        {
            get => this.CorpId.IsRoot ? 0 : _MaxDepth;
            set => _MaxDepth = value;
        }

        /// <summary>
        /// 子代理帳號數量限制, null=不限
        /// </summary>
        [DbImport]
        public virtual int? MaxAgents
        {
            get => this.CorpId.IsRoot ? 0 : _MaxAgents;
            set => _MaxAgents = value;
        }

        /// <summary>
        /// 附屬管理帳號數量限制, null=不限
        /// </summary>
        [DbImport]
        public virtual int? MaxAdmins
        {
            get => this.CorpId.IsRoot ? null : _MaxAdmins;
            set => _MaxAdmins = value;
        }

        /// <summary>
        /// 子會員帳號數量限制, null=不限
        /// </summary>
        [DbImport]
        public virtual int? MaxMembers
        {
            get => this.CorpId.IsRoot ? 0 : _MaxMembers;
            set => _MaxMembers = value;
        }

        #endregion
    }
}
namespace InnateGlory.Models
{
    [TableName(typeof(Entity.Agent))]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public struct AgentModel
    {
        [JsonProperty]
        public CorpId? CorpId { get; set; }

        [JsonProperty]
        public UserName CorpName { get; set; }

        [JsonProperty]
        public UserId? ParentId { get; set; }

        [JsonProperty]
        public UserName ParentName { get; set; }

        [JsonProperty]
        public UserId? Id { get; set; }

        [JsonProperty]
        public UserName Name { get; set; }

        [JsonProperty]
        public string DisplayName { get; set; }

        [JsonProperty]
        public ActiveState? Active { get; set; }

        [JsonProperty]
        public int? MaxDepth { get; set; }

        [JsonProperty]
        public int? MaxAgents { get; set; }

        [JsonProperty]
        public int? MaxAdmins { get; set; }

        [JsonProperty]
        public int? MaxMembers { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentListModel
    {
        [JsonProperty]
        public UserId? ParentId { get; set; }

        [JsonProperty]
        public bool? All { get; set; }

        [JsonProperty]
        public PagingModel<Entity.Agent> Paging
        {
            get => paging;//?? PagingModel<Entity.Agent>.Instance;
            set => paging = value;
        }
        PagingModel<Entity.Agent> paging;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ChildAgentModel
    {
        [JsonProperty]
        public UserId? agentId { get; set; }

        [JsonProperty]
        public bool include_root { get; set; }
    }
}
