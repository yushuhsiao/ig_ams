using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using InnateGlory.Api;
using System.Data.SqlClient;
using System.Threading;

namespace InnateGlory.Controllers
{
    public class AuthController : Controller
    {
        //[Api("/auth/login2"), Acl]
        //public IActionResult Login(
        //    [ModelBinder(Name = "CorpName")] UserName corpName,
        //    [ModelBinder(Name = "UserName")] UserName userName,
        //    [ModelBinder(Name = "Password")] string password)
        //{
        //    return ApiResult.OK;
        //}

        public AuthController()
        {
            //Microsoft.AspNetCore.Mvc.RazorPages.PageContext
        }

        [ActionContext]
        public ActionContext ActionContext { get; set; }

        static readonly Entity.UserState _null_GetState = new Entity.UserState() { UserId = UserId.Guest };

        private Entity.UserState CreateState(Entity.CorpInfo corp, Entity.UserData userdata)
        {
            return new Entity.UserState()
            {
                UserId = userdata.Id,
                CorpName = corp.Name,
                UserName = userdata.Name,
                DisplayName = userdata.DisplayName,
            };
        }

        //[Route("/")]
        //public ViewResult Index([FromServices] amsUser user) => View(user.Id.IsGuest ? "~/Pages/Login.cshtml" : "~/Pages/Index.cshtml");

        [Api("/auth/state"), AllowAnonymous]
        private async Task<Entity.UserState> GetState([FromServices] DataService dataService, [FromServices] amsUser user)
        {
            await Task.Delay(1);
            UserId userId = user.Id;
            if (dataService.Users.GetUser(userId, out var userdata))
            {
                Entity.CorpInfo corp = dataService.Corps.Get(userId.CorpId);
                return CreateState(corp, userdata);
            }
            else
            {
                if (!userId.IsGuest)
                    await dataService.GetService<UserManager<amsUser>>().SignOutAsync();
                return _null_GetState;
            }
        }

        [Api("/auth/login"), AllowAnonymous]
        public Task<IApiResult> Login([FromBody] Models.LoginModel model) => _UserLogin(model);

        [Api("/user/agent/login"), AllowAnonymous]
        public Task<IApiResult> AgentLogin([FromBody] Models.LoginModel model) => _UserLogin(model, UserType.Agent, LoginMode.UserToken);

        [Api("/user/admin/login"), AllowAnonymous]
        public Task<IApiResult> AdminLogin([FromBody] Models.LoginModel model) => _UserLogin(model, UserType.Admin, LoginMode.UserToken);

        [Api("/user/member/login"), AllowAnonymous]
        public Task<IApiResult> MemberLogin([FromBody] Models.LoginModel model) => _UserLogin(model, UserType.Member, LoginMode.UserToken);

        private async Task<IApiResult> _UserLogin(Models.LoginModel model, UserType? loginType = null, LoginMode? mode = null)
        {
            //return ApiResult.Forbidden;
            if (model == null)
                throw new ApiException(Status.InvalidParameter);

            model.LoginType = loginType ?? model.LoginType;
            model.LoginMode = mode ?? model.LoginMode ?? LoginMode.Cookie;

            var validator = new ApiModelValidator(model)
                .Valid(nameof(model.UserName), model.UserName)
                .Valid(nameof(model.Password), model.Password)
                .Valid(nameof(model.LoginType), model.LoginType)
                .Validate();

            DataService dataService = HttpContext.RequestServices.GetService<DataService>();
            try
            {
                var s = dataService.Users.UserLogin(model, out var corp, out var userdata);
                dataService.Users.WriteLoginLog(s, model, corp?.Id, HttpContext);
                if (s == Status.Success)
                {
                    var user = dataService.CreateInstance<amsUser>();
                    user.Id = userdata.Id;
                    if (model.LoginMode == LoginMode.AuthOnly)
                    {
                    }
                    else if (model.LoginMode == LoginMode.UserToken)
                    {
                        UserManager<amsUser> userManager = HttpContext.RequestServices.GetService<UserManager<amsUser>>();
                        string sessionId = await userManager.SignInAsync(user, HttpContext, _Consts.UserManager.AccessTokenScheme);
                        return ApiResult.Success(new { AccessToken = sessionId });
                    }
                    else
                    {
                        UserManager<amsUser> userManager = HttpContext.RequestServices.GetService<UserManager<amsUser>>();
                        await userManager.SignInAsync(user, HttpContext);
                        if (model.GetState == true)
                        {
                            return ApiResult.Success(CreateState(corp, userdata));
                        }
                        return ApiResult.Success();
                    }
                }
                throw validator.SetStatus(s);
            }
            catch
            {
                dataService.Users.WriteLoginLog(Status.Unknown, model, null, HttpContext);
                throw;
            }
        }

        [Api("/auth/logout")]
        public async Task<IApiResult> Logout([FromServices] UserManager<amsUser> userManager, [FromServices] amsUser user)
        {
            //await Task.Delay(3000);
            await userManager.SignOutAsync();
            return ApiResult.OK;
        }
    }
}