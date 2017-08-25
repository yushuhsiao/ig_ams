using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Mvc
{
    [DebuggerStepThrough]
    public abstract class AuthorizationFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (IsAuthorized(filterContext)) return;
            filterContext.Result = new HttpNotFoundResult();
        }
        protected abstract bool IsAuthorized(AuthorizationContext filterContext);
    }
}
namespace System.Web.Http.Filters
{
    using System.Web.Http.Controllers;

    [DebuggerStepThrough]
    public abstract class AuthorizationFilter : IAuthorizationFilter
    {
        bool IFilter.AllowMultiple
        {
            get { return this.GetType().GetCustomAttribute<AttributeUsageAttribute>(true).AllowMultiple; }
        }

        Task<HttpResponseMessage> IAuthorizationFilter.ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            // OnAuthorization
            if (!IsAuthorized(actionContext.ActionDescriptor))
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found!");

            if (actionContext.Response != null)
                return Task<HttpResponseMessage>.FromResult(actionContext.Response);
            else
                return continuation();
        }

        protected abstract bool IsAuthorized(HttpActionDescriptor actionDescriptor);
    }
}