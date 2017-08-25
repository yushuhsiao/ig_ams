using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace ams
{
    using System.Web.Mvc;

    partial class AccessLimitFilter : IAuthorizationFilter
    {
        [DebuggerStepThrough]
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (IsAuthorized(filterContext)) return;
            filterContext.Result = new HttpNotFoundResult();
        }

        bool IsAuthorized(AuthorizationContext filterContext) => this.GetType().Assembly == filterContext.Controller.GetType().Assembly;
    }
}
namespace ams
{
    using System.Web.Http.Filters;

    partial class AccessLimitFilter : IAuthorizationFilter
    {
        bool IFilter.AllowMultiple
        {
            [DebuggerStepThrough]
            get { return this.GetType().GetCustomAttribute<AttributeUsageAttribute>(true).AllowMultiple; }
        }

        [DebuggerStepThrough]
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

        bool IsAuthorized(HttpActionDescriptor actionDescriptor) => this.GetType().Assembly == actionDescriptor.ControllerDescriptor.ControllerType.Assembly;
    }
}