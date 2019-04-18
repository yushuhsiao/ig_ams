using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using _DebuggerStepThroughAttribute = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace Microsoft.AspNetCore.Http
{
    [_DebuggerStepThrough]
    public class HttpContextFactory<THttpContext> : IHttpContextFactory where THttpContext : HttpContext
    {
        //private static IHttpContextAccessor __httpContextAccessor;
        //public static IHttpContextAccessor HttpContextAccessor => Interlocked.CompareExchange(ref __httpContextAccessor, null, null);
        //public static THttpContext CurrentHttpContext => HttpContextAccessor?.HttpContext as THttpContext;

        private readonly IServiceProvider _services;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FormOptions _formOptions;

        public HttpContextFactory(IServiceProvider services, ObjectPoolProvider poolProvider, IOptions<FormOptions> formOptions, IHttpContextAccessor httpContextAccessor)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (poolProvider == null)
                throw new ArgumentNullException(nameof(poolProvider));
            if (formOptions == null)
                throw new ArgumentNullException(nameof(formOptions));
            _services = services;
            _formOptions = formOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            //Interlocked.Exchange(ref __httpContextAccessor, _httpContextAccessor = httpContextAccessor);
        }

        HttpContext IHttpContextFactory.Create(IFeatureCollection featureCollection)
        {
            if (featureCollection == null)
                throw new ArgumentNullException(nameof(featureCollection));

            var httpContext = _services.CreateInstance<THttpContext>(featureCollection);
            if (_httpContextAccessor != null)
                _httpContextAccessor.HttpContext = httpContext;

            var formFeature = new FormFeature(httpContext.Request, _formOptions);
            featureCollection.Set<IFormFeature>(formFeature);

            return httpContext;
        }

        void IHttpContextFactory.Dispose(HttpContext httpContext)
        {
            httpContext.RemoveItems<IHttpContextDispose>(x => x.Dispose(httpContext));
            //httpContext.Items.RemoveWhen(x =>
            //{
            //    if (x.Value is IHttpContextDispose)
            //    {
            //        try { ((IHttpContextDispose)x.Value).Dispose(httpContext); }
            //        catch { }
            //        return true;
            //    }
            //    return false;
            //});
            _httpContextAccessor.HttpContext = null;
        }
    }

    public static partial class HttpContextFactoryExtension
    {
        //public static IWebHostBuilder ReplaceHttpContextFactory(this IWebHostBuilder builder)
        //{
        //    return builder.ConfigureServices(services => services.ReplaceHttpContextFactory());
        //}

        public static IServiceCollection UseHttpContext<THttpContext>(this IServiceCollection services) where THttpContext : HttpContext
        {
            services.AddHttpContextAccessor();
            services.Replace(ServiceDescriptor.Transient<IHttpContextFactory, HttpContextFactory<THttpContext>>());
            return services;
        }


    }
}