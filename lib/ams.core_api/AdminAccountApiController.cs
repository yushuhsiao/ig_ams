using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminAccountApiController : UserApiController
    {
        [HttpPost, Route("~/Users/Admin/list")]
        public ListResponse<AdminData> list(ListRequest<AdminData> args)
        {
            return this.Null(args).Validate(this, true).GetResponse(create: () => new AdminData(args.CorpInfo));
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
        [JsonProperty("Active")]
        bool? AccountActive;
        public AdminActiveFlag? Active
        {
            get
            {
                if (AccountActive.HasValue)
                    return AccountActive.Value ? AdminActiveFlag.Active : AdminActiveFlag.Disabled;
                return null;
            }
        }
        #endregion

        [HttpPost, Route("~/Users/Admin/add")]
        public AdminData add(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(this.CorpID), this.CorpID, allow_null: true);
                ModelState.Validate(nameof(this.ParentName), this.ParentName);
                ModelState.Validate(nameof(this.Password), this.Password, allow_null: true);
                ModelState.Validate(nameof(this.UserName), this.UserName);
                ModelState.Validate(nameof(this.NickName), this.NickName, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["*n", "UserName     "] = this.UserName;
            sql1[" N", "NickName     "] = this.NickName.Trim(true) ?? this.UserName;
            sql1["  ", "Active       "] = this.Active ?? AdminActiveFlag.Active;

            CorpInfo corp = CorpInfo.GetCorpInfo(name: this.CorpName, err: true);
            SqlCmd userdb = corp.DB_User01W();
            AgentData parent = corp.GetAgentData(this.ParentName, userdb) ?? corp.GetAgentData(corp.ID, err: true);
            if (corp.GetAdminData(this.UserName, userdb) != null)
                throw new _Exception(Status.AdminAlreadyExist);
            if (parent.MaxAdminEnabled)
            {
                int agent_count = parent.GetAdminCount(userdb);
                if (agent_count >= parent.MaxAdmin)
                    throw new _Exception(Status.MaxAdminLimit);
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

            sql1["UserType"] = UserType.Admin;
            sql1[" ", "CorpID   "] = parent.CorpID;
            sql1[" ", "ParentID "] = parent.ID;
            sql1[" ", "Depth    "] = depth;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            UserID id;
            Guid uid;
            if (!corp.AllocUserID(out id, out uid, UserType.Admin, this.UserName))
                throw new _Exception(Status.UnableAllocateUserID);
            sql1[" ", "ID       "] = id;
            sql1[" ", "uid      "] = uid;

            string TableName = TableName<AdminData>._.TableName;
            string sql = sql1.Build($@"insert into {TableName}{sql1._insert()}
{pwd.Sql_Update(id, false)}
select * from {TableName} nolock where ID={id}");
            AdminData result = userdb.ToObject(() => new AdminData(corp), true, sql);
            AdminData.UserNames.Cache.UpdateVersion(corp.ID);
            return result;
        }

        [HttpPost, Route("~/Users/Admin/get")]
        public AdminData get(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(CorpID), this.CorpID, allow_null: true);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: this.CorpID, name: this.CorpName);
            if (this.ID.HasValue)
                return corp.GetAdminData(this.ID.Value);
            else if (this.UserName.IsValidEx)
                return corp.GetAdminData(this.UserName.Value);
            else ModelState.AddModelError("UserName", Status.InvalidParameter, throw_exception: true);
            return null;
        }

        [HttpPost, Route("~/Users/Admin/set")]
        public AdminData set(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate(nameof(this.CorpID), this.CorpID, allow_null: true);
                ModelState.Validate(nameof(this.ID), this.ID);
                ModelState.Validate(nameof(this.NickName), this.NickName, allow_null: true);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: this.CorpID, coredb: System.Web._HttpContext.GetSqlCmd(DB.Core01W), err: true);
            AdminData user = corp.GetAdminData(id: this.ID, name: this.UserName, userDB: corp.DB_User01W(), err: true);
            SqlBuilder sql1 = new SqlBuilder();
            sql1[" w", "ID          "] = this.ID;
            sql1["Nu", "NickName    "] = this.NickName.Trim(true);
            sql1[" u", "Active      "] = this.Active;
            return set(sql1, user, this.Password, () => new AdminData(corp));
        }
    }

    public class ApiAuthApiController : _ApiAuthApiController
    {
        [HttpPost, Route("~/Users/Admin/apiauth/get")]
        public ApiAuth get(_empty args) => base._get();

        [HttpPost, Route("~/Users/Admin/apiauth/set")]
        public ApiAuth set(_empty args) => base._set();
    }
}
namespace ams.Controllers
{
    using System.Web.Mvc;

    public class AdminAccountController : _Controller
    {
        [HttpGet, Route("~/Users/Admin"), Acl(typeof(AdminAccountApiController), nameof(AdminAccountApiController.get))]
        public ActionResult Admin() => View("~/Views/Users/Admin.cshtml");
        [HttpGet, Route("~/Users/Admin" + url_Details), Acl(typeof(AdminAccountApiController), nameof(AdminAccountApiController.get))]
        public ActionResult AdminDetails() => View_Details(Admin);
    }
}