using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    class _ActionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        static readonly _ActionFilterAttribute _instance = new _ActionFilterAttribute();

        public static void Init(HttpConfiguration config)
        {
            config.Filters.Add(_instance);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.ReturnType != null)
            {
                if ((actionExecutedContext.Response != null) && (actionExecutedContext.Response.Content is ObjectContent))
                {
                    ObjectContent c = (ObjectContent)actionExecutedContext.Response.Content;
                    if (c.Value == null)
                        throw new _Exception(Status.NoResult);
                }
            }
            base.OnActionExecuted(actionExecutedContext);
        }

        Task IExceptionFilter.ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            try
            {
                if (actionExecutedContext.Exception != null)
                {
                    HttpControllerDescriptor c = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor;
                    HttpActionDescriptor a = actionExecutedContext.ActionContext.ActionDescriptor;

                    log.message("Error", $@"{c.ControllerName}.{a.ActionName}
Url : {_HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath}
{actionExecutedContext.Exception}");
                    if (actionExecutedContext.Exception is _Exception)
                    {
                        _Exception ex = (_Exception)actionExecutedContext.Exception;
                        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(ex.HttpStatusCode, ex);
                    }
                    else if (actionExecutedContext.Exception is HttpResponseException)
                    {
                        HttpResponseException ex = (HttpResponseException)actionExecutedContext.Exception;
                        actionExecutedContext.Response = ex.Response; //actionExecutedContext.Request.CreateResponse(ex.Response.)
                    }
                    else
                    {
                        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new _ApiResult()
                        {
                            Status = Status.Unknown,
                            Message = actionExecutedContext.Exception.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return TaskHelpers.FromError(ex);
            }
            return TaskHelpers.Completed();
        }
    }
}
