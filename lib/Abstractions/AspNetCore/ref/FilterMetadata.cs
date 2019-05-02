using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Threading.Tasks;

namespace InnateGlory.AspNetCore
{
    class _IFilterMetadata : IFilterMetadata
    #region {}
    {
    }
    #endregion
    class _IActionFilter : IActionFilter
    #region {}
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IResponseCacheFilter : IActionFilter, IResponseCacheFilter
    #region {}
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncActionFilter : IAsyncActionFilter
    #region {}
    {
        Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAuthorizationFilter : IAuthorizationFilter
    #region {}
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncAuthorizationFilter : IAsyncAuthorizationFilter
    #region {}
    {
        Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _ICorsAuthorizationFilter : IAsyncAuthorizationFilter, IOrderedFilter, ICorsAuthorizationFilter
    #region {}
    {
        int IOrderedFilter.Order => throw new NotImplementedException();

        Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IExceptionFilter : IExceptionFilter
    #region {}
    {
        void IExceptionFilter.OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncExceptionFilter : IAsyncExceptionFilter
    #region {}
    {
        Task IAsyncExceptionFilter.OnExceptionAsync(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IResourceFilter : IResourceFilter
    #region {}
    {
        void IResourceFilter.OnResourceExecuted(ResourceExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IResourceFilter.OnResourceExecuting(ResourceExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncResourceFilter : IAsyncResourceFilter
    #region {}
    {
        Task IAsyncResourceFilter.OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IResultFilter : IResultFilter
    #region {}
    {
        void IResultFilter.OnResultExecuted(ResultExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncResultFilter : IAsyncResultFilter
    #region {}
    {
        Task IAsyncResultFilter.OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IPageFilter : IPageFilter
    #region {}
    {
        void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            throw new NotImplementedException();
        }

        void IPageFilter.OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            throw new NotImplementedException();
        }

        void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAsyncPageFilter : IAsyncPageFilter
    #region {}
    {
        Task IAsyncPageFilter.OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            throw new NotImplementedException();
        }

        Task IAsyncPageFilter.OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAllowAnonymousFilter : IAllowAnonymousFilter
    #region {}
    {
    }
    #endregion
    class _IFilterFactory : IFilterFactory
    #region {}
    {
        bool IFilterFactory.IsReusable => throw new NotImplementedException();

        IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IOrderedFilter : IOrderedFilter
    #region {}
    {
        int IOrderedFilter.Order => throw new NotImplementedException();
    }
    #endregion
    class _IApiRequestMetadataProvider : IApiRequestMetadataProvider
    #region {}
    {
        void IApiRequestMetadataProvider.SetContentTypes(MediaTypeCollection contentTypes)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IApiResponseMetadataProvider : IApiResponseMetadataProvider
    #region {}
    {
        Type IApiResponseMetadataProvider.Type => throw new NotImplementedException();

        int IApiResponseMetadataProvider.StatusCode => throw new NotImplementedException();

        void IApiResponseMetadataProvider.SetContentTypes(MediaTypeCollection contentTypes)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IFormatFilter : IFormatFilter
    #region {}
    {
        string IFormatFilter.GetFormat(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IRequestSizePolicy : IRequestSizePolicy
    #region {}
    {
    }
    #endregion
    class _ISaveTempDataCallback : ISaveTempDataCallback
    #region {}
    {
        void ISaveTempDataCallback.OnTempDataSaving(ITempDataDictionary tempData)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    class _IAntiforgeryPolicy : IAntiforgeryPolicy
    #region {}
    {
    }
    #endregion
}