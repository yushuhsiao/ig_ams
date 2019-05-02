using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    //public interface IPageContextAccessor
    //{
    //    PageContext PageContext { get; set; }
    //}

    //public class PageContextAccessor : IPageContextAccessor
    //{
    //    private static readonly AsyncLocal<PageContext> _storage = new AsyncLocal<PageContext>();

    //    public PageContextAccessor()
    //    {
    //    }

    //    public PageContext PageContext
    //    {
    //        get { return _storage.Value; }
    //        set { _storage.Value = value; }
    //    }
    //}

    public static class PageContextExtension
    {
        public static IServiceCollection AddPageContextAccessor(this IServiceCollection services)
        {
            services.Replace<IPageHandlerMethodSelector>(inject);
            //services.TryAddSingleton<IPageContextAccessor, PageContextAccessor>();
            services.TryAddTransient(PageContext);
            return services;
        }

        public static PageContext PageContext(this IServiceProvider services)
        {
            return services.GetService<IActionContextAccessor>()?.ActionContext.HttpContext.Items[typeof(PageContext)] as PageContext;
        }

        private static ServiceDescriptor inject(ServiceDescriptor d)
        {
            if (d.ImplementationType == typeof(DefaultPageHandlerMethodSelector))
                return new ServiceDescriptor(d.ServiceType, typeof(_PageHandlerMethodSelector), d.Lifetime);
            return d;
        }
    }

    class _PageHandlerMethodSelector : IPageHandlerMethodSelector
    {
        private IPageHandlerMethodSelector _inner;

        public _PageHandlerMethodSelector(IServiceProvider services)
        {
            _inner = services.CreateInstance<DefaultPageHandlerMethodSelector>();
        }

        public HandlerMethodDescriptor Select(PageContext context)
        {
            //var accessor = context.HttpContext.RequestServices.GetService<IPageContextAccessor>();
            //if (accessor != null)
            //{
            //    if (accessor.PageContext == null)
            //        accessor.PageContext = context;
            //}
            context.HttpContext.Items[typeof(PageContext)] = context;
            return _inner.Select(context);
        }

    }

}