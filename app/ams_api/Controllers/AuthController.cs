﻿using System;
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
    [Api]
    [Route("/auth")]
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

        //public AuthController()
        //{
        //    //Microsoft.AspNetCore.Mvc.RazorPages.PageContext
        //}

        //[ActionContext]
        //public ActionContext ActionContext { get; set; }

        //static readonly Entity.UserState _null_GetState = new Entity.UserState() { UserId = UserId.Guest };

        //private Entity.UserState CreateState(Entity.CorpInfo corp, Entity.UserData userdata)
        //{
        //    return new Entity.UserState()
        //    {
        //        UserId = userdata.Id,
        //        CorpName = corp.Name,
        //        UserName = userdata.Name,
        //        DisplayName = userdata.DisplayName,
        //    };
        //}

        //[HttpPost("state"), AllowAnonymous]
        //private async Task<Entity.UserState> GetState([FromServices] DataService dataService, [FromServices] amsUser user)
        //{
        //    await Task.Delay(1);
        //    UserId userId = user.Id;
        //    if (dataService.Users.GetUser(userId, out var userdata))
        //    {
        //        Entity.CorpInfo corp = dataService.Corps.Get(userId.CorpId);
        //        return CreateState(corp, userdata);
        //    }
        //    else
        //    {
        //        if (!userId.IsGuest)
        //            await dataService.GetService<UserManager<amsUser>>().SignOutAsync();
        //        return _null_GetState;
        //    }
        //}

        [HttpPost("login"), AllowAnonymous]
        public Task<Models.LoginResult> Login([FromBody] Models.LoginModel model) => _UserLogin(model);

        [HttpPost("logout")]
        public async Task<IApiResult> Logout([FromServices] UserManager userManager/*, [FromServices] amsUser user*/)
        {
            var user = userManager.CurrentUser;

            await user.SignOutAsync(HttpContext);

            return ApiResult.OK;
        }

        [HttpPost("/user/agent/login"), AllowAnonymous]
        public async Task<Models.LoginResult> AgentLogin([FromBody] Models.LoginModel model) => await _UserLogin(model, UserType.Agent, LoginMode.UserToken);

        [HttpPost("/user/admin/login"), AllowAnonymous]
        public async Task<Models.LoginResult> AdminLogin([FromBody] Models.LoginModel model) => await _UserLogin(model, UserType.Admin, LoginMode.UserToken);

        [HttpPost("/user/member/login"), AllowAnonymous]
        public async Task<Models.LoginResult> MemberLogin([FromBody] Models.LoginModel model) => await _UserLogin(model, UserType.Member, LoginMode.UserToken);

        private async Task<Models.LoginResult> _UserLogin(Models.LoginModel model, UserType? loginType = null, LoginMode? mode = null)
        {
            if (model == null)
                throw new ApiException(Status.InvalidParameter);

            model.LoginType = loginType ?? model.LoginType;
            model.LoginMode = mode ?? model.LoginMode ?? LoginMode.Cookie;

            ModelState
                .Valid(model, nameof(model.UserName))
                .Valid(model, nameof(model.Password))
                .Valid(model, nameof(model.LoginType))
                .IsValid();

            DataService ds = HttpContext.RequestServices.GetService<DataService>();
            Status status = Status.Unknown;
            Entity.UserData userdata = null;
            try
            {
                status = ds.Users.UserLogin(model, out userdata);
                if (status != Status.Success)
                    throw new ApiException(status);

                if (model.LoginMode == LoginMode.AuthOnly)
                    return new Models.LoginResult { UserId = userdata.Id };

                var user = ds.CreateInstance<UserIdentity>(userdata);
                if (model.LoginMode == LoginMode.UserToken)
                {
                    //UserManager userManager = HttpContext.RequestServices.GetService<UserManager>();
                    //string sessionId = await userManager.SignInAsync(user, userdata.Id, HttpContext, _Consts.UserManager.AccessTokenScheme);
                    string sessionId = await user.SignInAsync(HttpContext, _Consts.UserManager.AccessTokenScheme);
                    return new Models.LoginResult { UserId = userdata.Id, AccessToken = sessionId };
                }
                else
                {
                    //UserManager userManager = HttpContext.RequestServices.GetService<UserManager>();
                    //await userManager.SignInAsync(user, userdata.Id, HttpContext);
                    await user.SignInAsync(HttpContext);
                    return new Models.LoginResult { UserId = userdata.Id };
                }
            }
            finally
            {
                ds.Users.WriteLoginLog(status, model, userdata?.CorpId, HttpContext);
            }
        }
    }
}