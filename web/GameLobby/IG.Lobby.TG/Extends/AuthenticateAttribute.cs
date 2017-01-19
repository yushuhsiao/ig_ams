using IG.Lobby.TG.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IG.Lobby.TG.Extends
{
    enum UnauthorizedType
    {
        Authenticated,
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthenticateAttribute : AuthorizeAttribute
    {
        private UnauthorizedType unauthorizedType;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var user = httpContext.User;

            // 檢查有無登入
            if (!user.Identity.IsAuthenticated)
            {
                unauthorizedType = UnauthorizedType.Authenticated;

                return false;
            }

            // 檢查 AccessToken
            if (!AuthManager.CheckAccessToken(user.TakeAccessToken()))
            {
                unauthorizedType = UnauthorizedType.Singleton;
                AuthManager.Logout();

                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            switch (unauthorizedType)
            {
                case UnauthorizedType.Singleton:
                    filterContext.Controller.TempData["NOTY_ERROR"] = "Repeat login.";
                    break;
                default:
                    filterContext.Controller.TempData["NOTY_ERROR"] = "Please login to use this feature.";
                    break;
            }

            filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
        }
    }
}
