using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RecogService
{
    class RestrictActionFilter : IActionFilter
    {
        bool IFilter.AllowMultiple
        {
            get { return true; }
        }

        Task<HttpResponseMessage> IActionFilter.ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ControllerContext.Controller is IRecogApiController)
                return continuation();
            return Task.FromResult(actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }
    }
}
