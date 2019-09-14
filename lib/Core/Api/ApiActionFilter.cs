using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Api
{
    //public class ApiFilter : IActionFilter, IPageFilter
    //{
    //    void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context) { }
    //    void IPageFilter.OnPageHandlerExecuting(PageHandlerExecutingContext context) { }
    //    void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    //}

    internal class ApiActionFilter : IActionFilter
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
    }

    //public class ApiExceptionFilter : IExceptionFilter
    //{
    //}

}