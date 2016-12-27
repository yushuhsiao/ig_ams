using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ams.Controllers
{
    public abstract class _UserSignInApiController : _ApiController
    {
        public const string login_url = "~/users/login";
        public const string logout_url = "~/users/logout";

        public const UserType DefaultUserType = UserType.Agent | UserType.Admin;

        internal IHttpActionResult login(LoginRequest req)
        {
            _HttpContext context = _HttpContext.Current;

            if (req == null)
                throw new _Exception(Status.MissingParameter);

            if (req.UserName.IsNullOrEmpty)
                throw new _Exception(Status.MissingParameter, "UserName is empty");

            if (string.IsNullOrEmpty(req.Password))
                throw new _Exception(Status.MissingParameter, "Password is empty");

            if (!req.CorpName.IsValid)
                throw new _Exception(Status.InvalidParameter, "CorpName contains invalid char");

            if (!req.UserName.IsValid)
                throw new _Exception(Status.InvalidParameter, "UserName contains invalid char");

            if (req.LoginType.HasValue && !req.LoginType.Value.In(UserType.Agent, UserType.Admin, UserType.Member))
                throw new _Exception(Status.InvalidParameter);

            CorpInfo corpInfo;
            UserType allow_type;
            UserType login_type;
            // Get CorpID from request url or CorpName
            LoginUrl.GetCorp(context.SiteRootUrl, out corpInfo, out allow_type);
            if (corpInfo == null)
            {
                if (req.CorpName.IsNullOrEmpty)
                    corpInfo = CorpInfo.GetCorpInfo(UserID.root);
                else
                    corpInfo = CorpInfo.GetCorpInfo(req.CorpName);
                allow_type = _UserSignInApiController.DefaultUserType;
            }
            if (corpInfo == null)
                throw new _Exception(Status.CorpNotExist);
            if (corpInfo.Active != ActiveState.Active)
                throw new _Exception(Status.CorpDisabled, "Corp {UserName} Is Disabled".formatWith(corpInfo));

            // 決定登入類型
            if (req.LoginType.HasValue)
                login_type = req.LoginType.Value;
            else if (allow_type.In(UserType.Agent, UserType.Admin, UserType.Member))
                login_type = allow_type;
            else
                throw new _Exception(Status.UnableDecideUserType);
            login_type &= allow_type;

            if (!login_type.In(UserType.Agent, UserType.Admin, UserType.Member))
                throw new _Exception(Status.UserTypeNotAllow);

            UserData user = corpInfo.GetUserData(login_type, req.UserName);
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

            if (!p.Compare(req.Password))
                throw new _Exception(Status.PasswordNotMatch);

            _User.Manager.SetCurrentUser(user);

            return Ok(new _ApiResult() { Status = Status.Success });
            //return _login_result(Status.Success);
        }

        internal void logout()
        {
            _User.Manager.SetCurrentUser(null);
            //return Ok(new _ApiResult() { Status = Status.Success });
            //return _login_result(Status.Success);
        }

        //static LoginResponse _login_result(Status status, string format, params object[] args)
        //{
        //    return _login_result(status, string.Format(format, args));
        //}

        //static LoginResponse _login_result(Status status, string message = null)
        //{
        //    return new LoginResponse()
        //    {
        //        Status = status,
        //        Message = message ?? status.ToString()
        //    };
        //}
    }
}
namespace ams.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LoginRequest
    {
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
    }
}