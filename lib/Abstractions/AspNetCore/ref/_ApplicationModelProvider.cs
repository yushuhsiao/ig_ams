using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Internal
{
    public class _ApplicationModelProvider : DefaultApplicationModelProvider
    {
        public _ApplicationModelProvider(IOptions<MvcOptions> mvcOptionsAccessor) : base(mvcOptionsAccessor) { }

        public override void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            base.OnProvidersExecuting(context);
        }

        //protected override ControllerModel CreateControllerModel(TypeInfo typeInfo)
        //{
        //    var result = base.CreateControllerModel(typeInfo);
        //    return result;
        //}

        //protected override PropertyModel CreatePropertyModel(PropertyInfo propertyInfo)
        //{
        //    var result = base.CreatePropertyModel(propertyInfo);
        //    return result;
        //}

        //protected override ActionModel CreateActionModel(TypeInfo typeInfo, MethodInfo methodInfo)
        //{
        //    var result = base.CreateActionModel(typeInfo, methodInfo);
        //    return result;
        //}

        protected override bool IsAction(TypeInfo typeInfo, MethodInfo methodInfo)
        {
            if (methodInfo.IsDefined(typeof(INonAction), true))
            {
                return false;
            }
            return base.IsAction(typeInfo, methodInfo);
        }

        //protected override ParameterModel CreateParameterModel(ParameterInfo parameterInfo)
        //{
        //    var result = base.CreateParameterModel(parameterInfo);
        //    return result;
        //}

        public override void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            base.OnProvidersExecuted(context);
        }
    }
}
