using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InnateGlory
{
    /// <summary>
    /// Action 將會使用 api 格式運作
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiAttribute : Attribute, IFilterMetadata
    {
        public string Acl { get; set; }

        public ApiAttribute() : base() { }
        //public ApiAttribute(string template) : base(template)
        //{
        //    this.Acl = template;
        //}
        //public ApiAttribute(string template, string acl) : base(template)
        //{
        //    this.Acl = acl;
        //}

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

        //    if (descriptor != null)
        //    {
        //        var parameters = descriptor.MethodInfo.GetParameters();

        //        foreach (var parameter in parameters)
        //        {
        //            var argument = context.ActionArguments[parameter.Name];

        //            EvaluateValidationAttributes(parameter, argument, context.ModelState);
        //        }
        //    }

        //    base.OnActionExecuting(context);
        //}

        //private void EvaluateValidationAttributes(ParameterInfo parameter, object argument, ModelStateDictionary modelState)
        //{
        //    var validationAttributes = parameter.CustomAttributes;

        //    foreach (var attributeData in validationAttributes)
        //    {
        //        var attributeInstance = CustomAttributeExtensions.GetCustomAttribute(parameter, attributeData.AttributeType);

        //        var validationAttribute = attributeInstance as ValidationAttribute;

        //        if (validationAttribute != null)
        //        {
        //            var isValid = validationAttribute.IsValid(argument);
        //            if (!isValid)
        //            {
        //                modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
        //            }
        //        }
        //    }
        //}
    }


    //public interface IApi
    //{
    //}
}