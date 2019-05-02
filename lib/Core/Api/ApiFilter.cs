using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Api
{
    public class ApiFilter : IActionFilter, IPageFilter,  IExceptionFilter, IResultFilter
    {
        private void ExecuteValidation(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                foreach (var parameter in descriptor.Parameters.OfType<ControllerParameterDescriptor>())
                {
                    foreach (var validationAttribute in parameter.ParameterInfo.GetCustomAttributes<ValidationAttribute>())
                    {
                        var argument = context.ActionArguments[parameter.Name];

                        if (false == validationAttribute.IsValid(argument))
                        {
                            context.ModelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
                        }
                    }
                }
            }
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            //this.ExecuteValidation(context);
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context) { }

        void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context) { }

        void IPageFilter.OnPageHandlerExecuting(PageHandlerExecutingContext context) { }

        void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context) { }

        void IExceptionFilter.OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            if (ex is IApiResult)
                context.Result = (IApiResult)ex;
            else if (context.ActionDescriptor.IsApi())
                context.Result = new ApiException(Status.Unknown, ex.Message, ex);
        }

        // 具有 ApiAttribute 的 Action, 會強制使用 IApiResult 的格式輸出
        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor.IsApi())
            {
                context.Result = ApiResult.FromActionResult(context.Result);
            }
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext context) { }
    }
}