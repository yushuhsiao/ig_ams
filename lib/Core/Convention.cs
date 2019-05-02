using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace InnateGlory
{
    class _ApplicationModelProvider : IApplicationModelProvider, IPageApplicationModelProvider
    {
        public int Order => 10000;

        void IApplicationModelProvider.OnProvidersExecuting(ApplicationModelProviderContext context)
        {
        }

        void IApplicationModelProvider.OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        void IPageApplicationModelProvider.OnProvidersExecuting(PageApplicationModelProviderContext context)
        {
        }

        void IPageApplicationModelProvider.OnProvidersExecuted(PageApplicationModelProviderContext context)
        {
        }
    }

    class _ApplicationModelConvention : IApplicationModelConvention, IPageApplicationModelConvention
    {
        void IApplicationModelConvention.Apply(ApplicationModel application)
        {
            for (int n1 = 0; n1 < application.Controllers.Count; n1++)
            {
                var controller = application.Controllers[n1];
                Apply(controller);
                for (int n2 = 0; n2 < controller.Actions.Count; n2++)
                {
                    var action = controller.Actions[n2];
                    Apply(action);
                    if (action.Attributes.OfType<INonAction>().Count() > 0)
                    {
                        controller.Actions.RemoveAt(n2);
                        n2--;
                        continue;
                    }
                    for (int n3 = 0; n3 < action.Parameters.Count; n3++)
                    {
                        var parameter = action.Parameters[n3];
                        Apply(parameter);

                    }
                }
            }
        }

        private void Apply(ControllerModel controller)
        {
        }

        private void Apply(ActionModel action)
        {
        }

        private void Apply(ParameterModel parameter)
        {
            //if (parameter.ParameterInfo.ParameterType.IsDefined(typeof(JsonObjectAttribute), true))
            //{
            //    ;
            //}
            //if (parameter.ParameterInfo.ParameterType == typeof(UserId) ||
            //    parameter.ParameterInfo.ParameterType == typeof(UserId?))
            //    parameter.BindingInfo = new BindingInfo()
            //    {
            //        BinderType = typeof(UserIdModelBinder)
            //    };
        }

        void IPageApplicationModelConvention.Apply(PageApplicationModel model)
        {
            //model.Filters.Add(new GenericFilter());
        }

        //public class _ControllerModelConvention : IControllerModelConvention
        //{
        //    void IControllerModelConvention.Apply(ControllerModel controller)
        //    {
        //    }
        //}
        //public class _ActionModelConvention : IActionModelConvention
        //{
        //    void IActionModelConvention.Apply(ActionModel action)
        //    {
        //    }
        //}
        //public class _ParameterModelConvention : IParameterModelConvention
        //{
        //    void IParameterModelConvention.Apply(ParameterModel parameter)
        //    {
        //    }
        //}
    }

    //class _PageRouteModelConvention : IPageRouteModelConvention
    //{
    //    void IPageRouteModelConvention.Apply(PageRouteModel model)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    partial class GenericFilter : IFilterMetadata { }
    //partial class GenericFilter : IActionFilter
    //{
    //    void IActionFilter.OnActionExecuted(ActionExecutedContext context)
    //    {
    //    }

    //    void IActionFilter.OnActionExecuting(ActionExecutingContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IResponseCacheFilter { }
    //partial class GenericFilter : IAsyncActionFilter
    //{
    //    Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        return next();
    //    }
    //}
    //partial class GenericFilter : IPageFilter
    //{
    //    void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context)
    //    {
    //    }

    //    void IPageFilter.OnPageHandlerExecuting(PageHandlerExecutingContext context)
    //    {
    //    }

    //    void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAsyncPageFilter
    //{
    //    Task IAsyncPageFilter.OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    //    {
    //        return next();
    //    }

    //    Task IAsyncPageFilter.OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    //    {
    //        return Task.CompletedTask;
    //    }
    //}
    //partial class GenericFilter : IAuthorizationFilter
    //{
    //    void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAsyncAuthorizationFilter
    //{
    //    Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        return Task.CompletedTask;
    //    }
    //}
    //partial class GenericFilter : IOrderedFilter
    //{
    //    int IOrderedFilter.Order => 0;
    //}
    //partial class GenericFilter : ICorsAuthorizationFilter { }
    //partial class GenericFilter : IExceptionFilter
    //{
    //    void IExceptionFilter.OnException(ExceptionContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAsyncExceptionFilter
    //{
    //    Task IAsyncExceptionFilter.OnExceptionAsync(ExceptionContext context)
    //    {
    //        return Task.CompletedTask;
    //    }
    //}
    //partial class GenericFilter : IResourceFilter
    //{
    //    void IResourceFilter.OnResourceExecuted(ResourceExecutedContext context)
    //    {
    //    }

    //    void IResourceFilter.OnResourceExecuting(ResourceExecutingContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAsyncResourceFilter
    //{
    //    Task IAsyncResourceFilter.OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    //    {
    //        return next();
    //    }
    //}
    //partial class GenericFilter : IResultFilter
    //{
    //    void IResultFilter.OnResultExecuted(ResultExecutedContext context)
    //    {
    //    }

    //    void IResultFilter.OnResultExecuting(ResultExecutingContext context)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAsyncResultFilter
    //{
    //    Task IAsyncResultFilter.OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    //    {
    //        return next();
    //    }
    //}
    //partial class GenericFilter : IAllowAnonymousFilter
    //{
    //}
    //partial class GenericFilter : IFilterFactory
    //{
    //    bool IFilterFactory.IsReusable => true;

    //    IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //partial class GenericFilter : IApiRequestMetadataProvider
    //{
    //    void IApiRequestMetadataProvider.SetContentTypes(MediaTypeCollection contentTypes)
    //    {
    //    }
    //}
    //partial class GenericFilter : IApiResponseMetadataProvider
    //{
    //    Type IApiResponseMetadataProvider.Type => throw new NotImplementedException();

    //    int IApiResponseMetadataProvider.StatusCode => 200;

    //    void IApiResponseMetadataProvider.SetContentTypes(MediaTypeCollection contentTypes)
    //    {
    //    }
    //}
    //partial class GenericFilter : IFormatFilter
    //{
    //    string IFormatFilter.GetFormat(ActionContext context)
    //    {
    //        return null;
    //    }
    //}
    //partial class GenericFilter : IRequestSizePolicy
    //{
    //}
    //partial class GenericFilter : ISaveTempDataCallback
    //{
    //    void ISaveTempDataCallback.OnTempDataSaving(ITempDataDictionary tempData)
    //    {
    //    }
    //}
    //partial class GenericFilter : IAntiforgeryPolicy
    //{
    //}
}