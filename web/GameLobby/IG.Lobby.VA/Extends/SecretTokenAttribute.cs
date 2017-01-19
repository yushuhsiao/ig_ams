using IG.Lobby.VA.Helpers;
using System;
using System.Web.Mvc;

namespace IG.Lobby.VA.Extends
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SecretTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var secretToken = filterContext.HttpContext.Request.Headers["X-Secret-Token"];

            if (secretToken != ConfigHelper.ApiSecretToken)
            {
                filterContext.Result = new HttpStatusCodeResult(404);
            }
        }
    }
}
