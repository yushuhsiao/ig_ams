using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentAccountApiController : UserApiController
    {
        [HttpPost, Route("~/Users/Agent/list")]
        public ListResponse<AgentData> list(ListRequest<AgentData> args)
        {
            this.Null(args).Validate(this, true);
            args.sql_where_add("ID <> CorpID");
            var ret = args.GetResponse(create: () => new AgentData(args.CorpInfo));
            foreach (AgentData data in ret.Rows)
                data.Balance = data.GetBalance();
            return ret;

            //
            //ListRequest_2<AgentData>.Valid(ModelState, args);
            //SqlCmd userDB = args.CorpInfo.DB_User01R();
            //args.sql_build_op.Add("and ID <> CorpID");
            //return args.GetResponse(userDB, create: () => new AgentData(args.CorpInfo));

            //if (args == null)
            //    throw new _Exception(Status.InvalidParameter);
            //args.Valid(ModelState);
            //string sql1, sql2;
            //args.GetSqlStr(TableName<AgentData>._.TableName, args.OnBuild, out sql1, out sql2);
            //SqlCmd userDB = args.CorpInfo.DB_User01R();
            //var ret = new ListResponse<AgentData>()
            //{
            //    Rows = userDB.ToList(() => new AgentData(args.CorpInfo), sql1),
            //    RowsCount = userDB.ExecuteScalar(sql2) as int?,
            //};
            //foreach (var n in ret.Rows)
            //    n.Balance = n.GetBalance(userDB);
            //return ret;
        }

        #region arguments
        [JsonProperty]
        public UserID? CorpID;
        [JsonProperty]
        public UserName CorpName;
        [JsonProperty]
        public UserName ParentName;
        [JsonProperty]
        public UserID? ID;
        [JsonProperty]
        public UserName UserName;
        [JsonProperty]
        public string NickName;
        [JsonProperty]
        public string Password;
        [JsonProperty("Active1")]
        bool? AccountActive;
        [JsonProperty("Active2")]
        bool? GameDisabled;
        public AgentActiveFlag? Active
        {
            get
            {
                AgentActiveFlag? result = null;
                result = setFlag(result, AccountActive, AgentActiveFlag.Accounts);
                result = setFlag(result, !GameDisabled, AgentActiveFlag.Game);
                result = setFlag(result, MaxDepthEnabled, AgentActiveFlag.MaxDepthEnabled);
                result = setFlag(result, MaxAgentEnabled, AgentActiveFlag.MaxAgentEnabled);
                result = setFlag(result, MaxAdminEnabled, AgentActiveFlag.MaxAdminEnabled);
                result = setFlag(result, MaxMemberEnabled, AgentActiveFlag.MaxMemberEnabled);
                return result;
            }
        }

        AgentActiveFlag? setFlag(AgentActiveFlag? src, bool? enabled, AgentActiveFlag value)
        {
            if (enabled.HasValue)
            {
                if (enabled.Value)
                    return (src ?? 0) | value;
                else
                    return (src ?? 0) & ~value;
            }
            else return src;
        }

        int? nn(int? n1, bool? n2)
        {
            if (n1.HasValue && n1.Value >= 0 && n2.HasValue)
                return n2.Value ? n1.Value : -n1.Value;
            return null;
        }

        [JsonProperty]
        public int? MaxDepth;
        [JsonProperty]
        bool? MaxDepthEnabled;

        [JsonProperty]
        public int? MaxAgent;
        [JsonProperty]
        bool? MaxAgentEnabled;

        [JsonProperty]
        public int? MaxAdmin;
        [JsonProperty]
        bool? MaxAdminEnabled;

        [JsonProperty]
        public int? MaxMember;
        [JsonProperty]
        bool? MaxMemberEnabled;
        #endregion

        [HttpPost, Route("~/Users/Agent/add")]
        public AgentData add(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(this.CorpID), this.CorpID, allow_null: true);
                ModelState.Validate(nameof(this.ParentName), this.ParentName, allow_null: true);
                ModelState.Validate(nameof(this.Password), this.Password, allow_null: true);
                ModelState.Validate(nameof(this.UserName), this.UserName);
                ModelState.Validate(nameof(this.NickName), this.NickName, allow_null: true);
                ModelState.Validate(nameof(this.MaxDepth), this.MaxDepth, min: (n) => n >= 0, allow_null: true);
                ModelState.Validate(nameof(this.MaxAgent), this.MaxAgent, min: (n) => n >= 0, allow_null: true);
                ModelState.Validate(nameof(this.MaxAdmin), this.MaxAdmin, min: (n) => n >= 0, allow_null: true);
                ModelState.Validate(nameof(this.MaxMember), this.MaxMember, min: (n) => n >= 0, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["*n", "UserName    "] = this.UserName;
            sql1[" N", "NickName    "] = this.NickName.Trim(true) ?? this.UserName;
            sql1["  ", "Active      "] = this.Active ?? this.Active ?? AgentActiveFlag.Accounts | AgentActiveFlag.Game | AgentActiveFlag.MaxDepthEnabled | AgentActiveFlag.MaxAdminEnabled | AgentActiveFlag.MaxAgentEnabled;
            sql1["  ", "MaxDepth    "] = this.MaxDepth ?? 0;
            sql1["  ", "MaxAgent    "] = this.MaxAgent ?? 0;
            sql1["  ", "MaxAdmin    "] = this.MaxAdmin ?? 0;
            sql1["  ", "MaxMember   "] = this.MaxMember ?? 0;

            CorpInfo corp = CorpInfo.GetCorpInfo(name: this.CorpName, err: true);
            SqlCmd userdb = corp.DB_User01W();
            AgentData parent = corp.GetAgentData(this.ParentName, userdb) ?? corp.GetAgentData(corp.ID, err: true);
            if (corp.GetAgentData(this.UserName, userdb) != null)
                throw new _Exception(Status.AgentAlreadyExist);
            if (parent.MaxAgentEnabled)
            {
                int agent_count = parent.GetAgentCount(userdb);
                if (agent_count >= parent.MaxAgent)
                    throw new _Exception(Status.MaxAgentLimit);
            }
            int depth = parent.Depth + 1;
            int maxDepth = int.MaxValue;
            for (AgentData p = parent; p != null; p = p.GetParent(userdb))
            {
                if (p.MaxDepthEnabled)
                {
                    maxDepth = Math.Min(maxDepth, p.Depth + p.MaxDepth);
                    if (depth > maxDepth)
                        throw new _Exception(Status.MaxDepthLimit, "MaxDepth Limit on '{0}'".format(p.UserName));
                }
            }

            PasswordEncryptor pwd = new PasswordEncryptor(this.Password);

            sql1["TableName"] = (SqlBuilder.str)TableName<AgentData>._.TableName;
            sql1["UserType"] = UserType.Agent;
            sql1[" ", "CorpID   "] = parent.CorpID;
            sql1[" ", "ParentID "] = parent.ID;
            sql1[" ", "Depth    "] = depth;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            UserID id;
            Guid uid;
            if (!corp.AllocUserID(out id, out uid, UserType.Agent, this.UserName))
                throw new _Exception(Status.UnableAllocateUserID);
            sql1[" ", "ID       "] = id;
            sql1[" ", "uid      "] = uid;

            string sql = sql1.Build("insert into {TableName} ", SqlBuilder.op.insert, @"
", pwd.Sql_Update(id, false), @"
select * from {TableName} nolock where ID={ID}");
            AgentData result = userdb.ToObject(() => new AgentData(corp), true, sql);
            AgentData.UserNames.Cache.UpdateVersion(corp.ID);
            return result;
        }

        [HttpPost, Route("~/Users/Agent/get")]
        public AgentData get(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(this.CorpID), this.CorpID, allow_null: true);
                ModelState.Validate(nameof(this.ID), this.ID, allow_null: true);
                ModelState.Validate(nameof(this.UserName), this.UserName, allow_null: true);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: this.CorpID, err: true);
            AgentData data = corp.GetAgentData(id: this.ID, name: this.UserName.Value, err: true);
            if (data != null)
                data.Balance = data.GetBalance();
            return data;

            //CorpInfo corp = ModelState.ValidateCorpID("CorpID", args?.CorpID);
            ////CorpInfo corp = ModelState.ValidateCorpName("CorpName", args.CorpName);
            //ModelState.IsValid();
            //if (args.ID.HasValue)
            //    return corp.GetAgentData(args.ID.Value);
            //else if (args.UserName.IsValidEx)
            //    return corp.GetAgentData(args.UserName.Value);
            //else ModelState.AddModelError("UserName", Status.InvalidParameter, throw_exception: true);
            //return null;
        }

        [HttpPost, Route("~/Users/Agent/set")]
        public AgentData set(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(this.CorpID), this.CorpID, allow_null: true);
                ModelState.Validate(nameof(this.ID), this.ID);
                ModelState.Validate(nameof(this.NickName), this.NickName, allow_null: true);
                ModelState.Validate(nameof(this.MaxDepth), this.MaxDepth, min: (n) => n >= 0);
                ModelState.Validate(nameof(this.MaxAgent), this.MaxAgent, min: (n) => n >= 0);
                ModelState.Validate(nameof(this.MaxAdmin), this.MaxAdmin, min: (n) => n >= 0);
                ModelState.Validate(nameof(this.MaxMember), this.MaxMember, min: (n) => n >= 0);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: this.CorpID, coredb: System.Web._HttpContext.GetSqlCmd(DB.Core01W), err: true);
            AgentData user = corp.GetAgentData(id: this.ID, name: this.UserName, userDB: corp.DB_User01W(), err: true);
            SqlBuilder sql1 = new SqlBuilder();
            sql1[" w", "ID          "] = this.ID;
            sql1["Nu", "NickName    "] = this.NickName.Trim(true);
            sql1[" u", "Active      "] = this.Active;
            sql1[" u", "MaxDepth    "] = this.MaxDepth;
            sql1[" u", "MaxAgent    "] = this.MaxAgent;
            sql1[" u", "MaxAgent    "] = this.MaxAgent;
            sql1[" u", "MaxMember   "] = this.MaxMember;
            return set(sql1, user, this.Password, () => new AgentData(corp));
        }
    }
}
namespace ams.Controllers
{
    using System.Web.Mvc;
    public class AgentAccountController : _Controller
    {
        [HttpGet, Route("~/Users/Agent"), Acl(typeof(AgentAccountApiController), nameof(AgentAccountApiController.get))]
        public ActionResult Agent() => View("~/Views/Users/Agent.cshtml");

        [HttpGet, Route("~/Users/Agent" + url_Details), Acl(typeof(AgentAccountApiController), nameof(AgentAccountApiController.get))]
        public ActionResult AgentDetails() => View_Details(Agent);
    }
}