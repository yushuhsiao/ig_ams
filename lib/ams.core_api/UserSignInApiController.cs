using ams.Data;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserSignInApiController : _ApiController
    {
        public const string login_url = "~/users/login";
        public const string logout_url = "~/users/logout";

        public const UserType DefaultUserType = UserType.Agent | UserType.Admin;

        #region arguments
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        [JsonProperty]
        public UserName CorpName;
        /// <summary>
        /// 登入帳號
        /// </summary>
        [JsonProperty]
        public UserName UserName;
        /// <summary>
        /// 登入密碼
        /// </summary>
        [JsonProperty]
        public string Password;
        [JsonProperty]
        public UserType? LoginType;
        #endregion

        [AllowAnonymous, HttpPost, Route(login_url)]
        public IHttpActionResult login(_empty req) => Ok(_login(req));
        [NonAction]
        public _ApiResult _login(_empty req)
        {
            _HttpContext context = _HttpContext.Current;
            this.Validate(true, req, () => { });

            if (this.UserName.IsNullOrEmpty)
                throw new _Exception(Status.MissingParameter, "UserName is empty");

            if (string.IsNullOrEmpty(this.Password))
                throw new _Exception(Status.MissingParameter, "Password is empty");

            if (!this.CorpName.IsValid)
                throw new _Exception(Status.InvalidParameter, "CorpName contains invalid char");

            if (!this.UserName.IsValid)
                throw new _Exception(Status.InvalidParameter, "UserName contains invalid char");

            if (this.LoginType.HasValue && !this.LoginType.Value.In(UserType.Agent, UserType.Admin, UserType.Member))
                throw new _Exception(Status.InvalidParameter);

            CorpInfo corpInfo;
            UserType allow_type;
            UserType login_type;
            // Get CorpID from request url or CorpName
            LoginUrl.GetCorp(context.SiteRootUrl, out corpInfo, out allow_type);
            if (corpInfo == null)
            {
                if (this.CorpName.IsNullOrEmpty)
                    corpInfo = CorpInfo.GetCorpInfo(UserID.root);
                else
                    corpInfo = CorpInfo.GetCorpInfo(this.CorpName);
                allow_type = UserSignInApiController.DefaultUserType;
            }
            if (corpInfo == null)
                throw new _Exception(Status.CorpNotExist);
            if (corpInfo.Active != ActiveState.Active)
                throw new _Exception(Status.CorpDisabled, "Corp {UserName} Is Disabled".formatWith(corpInfo));

            // 決定登入類型
            if (this.LoginType.HasValue)
                login_type = this.LoginType.Value;
            else if (allow_type.In(UserType.Agent, UserType.Admin, UserType.Member))
                login_type = allow_type;
            else
                throw new _Exception(Status.UnableDecideUserType);
            login_type &= allow_type;

            if (!login_type.In(UserType.Agent, UserType.Admin, UserType.Member))
                throw new _Exception(Status.UserTypeNotAllow);

            UserData user = corpInfo.GetUserData(login_type, this.UserName);
            if (user == null)
                throw new _Exception(Status.UserNotExist);

            bool active = false;
            AgentData _agent = user as AgentData;
            AdminData _admin = user as AdminData;
            MemberData _member = user as MemberData;
            if (_agent != null)
                active = _agent.AccountsActive;
            else if (_admin != null)
                active = _admin.AccountActive;
            else if (_member != null)
                active = _member.AccountActive;

            if (!active)
                throw new _Exception(Status.AccountDisabled);

            //if (!user.Active.HasFlag(Active3.Accounts))
            //    throw new _Exception(Status.AccountDisabled);

            foreach (AgentData _parent in user.GetAllParent(false))
            {
                if (_parent.AccountsActive)
                    continue;
                throw new _Exception(Status.ParentDisabled, "Parent {UserName} Is Disabled".formatWith(_parent));
            }

            PasswordEncryptor p = user.GetPassword();
            //if (p == null && user.ID.IsRoot)
            //{
            //    SqlPassword pp = new SqlPassword() { UserID = UserID.root };
            //    pp.Encrypt("root");
            //    p = user.CreatePassword(pp);
            //}
            if (p == null)
                throw new _Exception(Status.PasswordNotFound);

            if (!p.IsActive)
                throw new _Exception(Status.PasswordDisabled);

            if (p.IsExpired)
                throw new _Exception(Status.PasswordExpired);

            if (!p.Compare(this.Password))
                throw new _Exception(Status.PasswordNotMatch);

            _User.Manager.SetCurrentUser(user);

            return new _ApiResult() { Status = Status.Success };
        }

        [AllowAnonymous, HttpPost, Route(logout_url)]
        public void logout()
        {
            _User.Manager.SetCurrentUser(null);
            //return Ok(new _ApiResult() { Status = Status.Success });
            //return _login_result(Status.Success);
        }

        IHttpActionResult user_login<T>() where T : UserData<T>
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("CorpName", this.CorpName, allow_null: true);
                ModelState.Validate("UserName", this.UserName);
                ModelState.Validate("Password", this.Password);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: this.CorpName, err: true);
            //if (args.CorpName.IsNullOrEmpty)
            //{
            //    UserType allow_type;
            //    LoginUrl.GetCorp(_HttpContext.Current.SiteRootUrl, out corp, out allow_type);
            //    corp = corp ?? CorpInfo.GetCorpInfo(UserID.root);
            //}
            //else
            //corp = ModelState.ValidateCorpName("CorpName", this.CorpName);
            //ModelState.Validate("UserName", this.UserName);
            //ModelState.Validate("Password", this.Password);
            //ModelState.IsValid();

            if (corp.Active != ActiveState.Active)
                throw new _Exception(Status.CorpDisabled);

            UserData<T> user = corp.GetUserData<T>(this.UserName);
            if (user == null)
                throw new _Exception(Status.UserNotExist);
            bool active = false;
            AgentData _agent = user as AgentData;
            AdminData _admin = user as AdminData;
            MemberData _member = user as MemberData;
            if (_agent != null)
                active = _agent.AccountsActive;
            else if (_admin != null)
                active = _admin.AccountActive;
            else if (_member != null)
                active = _member.AccountActive;

            if (!active)
                throw new _Exception(Status.AccountDisabled);

            for (AgentData parent = user.GetParent(); parent != null; parent = parent.GetParent())
                if (!parent.AccountsActive)
                    throw new _Exception(Status.ParentDisabled);

            PasswordEncryptor p = user.GetPassword();
            if (p == null)
                throw new _Exception(Status.PasswordNotFound);
            if (!p.IsActive)
                throw new _Exception(Status.PasswordDisabled);
            if (!p.Compare(this.Password))
                throw new _Exception(Status.PasswordNotMatch);

            _User _user = _User.Current;
            if (!(_user is _ApiUser))
                _User.Manager.SetCurrentUser(user);
            return Ok();
        }

        [HttpPost, Route("~/Users/Agent/login"), AllowAnonymous]
        public IHttpActionResult agent_login(_empty args) => user_login<AgentData>();

        [HttpPost, Route("~/Users/Admin/login"), AllowAnonymous]
        public IHttpActionResult admin_login(_empty args) => user_login<AdminData>();

        [HttpPost, Route("~/Users/Member/login"), AllowAnonymous]
        public IHttpActionResult member_login(_empty args) => user_login<MemberData>();

    }
}
