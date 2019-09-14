using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace InnateGlory.Api
{
    // 具有 ApiAttribute 的 Action, 會強制使用 IApiResult 的格式輸出
    internal class ApiResultFilter : IResultFilter, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext context)
        {
            if (false == context.ActionDescriptor.IsApi())
                return;
            var ex = context.Exception;
            if (ex is ApiException)
                context.Result = (ApiException)ex;
            else
                context.Result = new ApiException(Status.Unknown, ex.Message, ex);
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            if (false == context.ActionDescriptor.IsApi())
                return;
            context.Result = ApiResultFilter.FromActionResult(context.Result);
        }

        private static IActionResult FromActionResult(IActionResult src)
        {
            if (src is IApiResult)
                return src;
            if (src is JsonResult)
            {
                JsonResult obj = (JsonResult)src;
                var ret = ApiResult.Success(obj.Value);
                ret.ContentType = obj.ContentType;
                ret.HttpStatusCode = (HttpStatusCode?)obj.StatusCode;
                return ret;
            }
            else if (src is ObjectResult)
            {
                ObjectResult obj = (ObjectResult)src;
                var ret = ApiResult.Success(obj.Value);
                ret.HttpStatusCode = (HttpStatusCode?)obj.StatusCode;
                return ret;
            }
            else if (src is EmptyResult)
            {
                return ApiResult.Success(null);
            }
            return ApiResult.Success(src);
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext context) { }
    }
}
