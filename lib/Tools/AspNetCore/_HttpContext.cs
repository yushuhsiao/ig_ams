#if NETSTANDARD1_6
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Http
{
    public class _HttpContext : DefaultHttpContext
    {
        public static _HttpContext Current
        {
            [DebuggerStepThrough]
            get { return _HttpContextExtensions.GetHttpContext(); }
        }

        public _HttpContext() : base() { }
        public _HttpContext(IFeatureCollection features) : base(features) { }

        public override void Initialize(IFeatureCollection features)
        {
            base.Initialize(features);
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
        }
    }

    //public class _HttpContext : HttpContext
    //{
    //    // Lambdas hoisted to static readonly fields to improve inlining https://github.com/dotnet/roslyn/issues/13624
    //    private readonly static Func<IFeatureCollection, IItemsFeature> _newItemsFeature 
    //        = f => new ItemsFeature();
    //    private readonly static Func<IFeatureCollection, IServiceProvidersFeature> _newServiceProvidersFeature 
    //        = f => new ServiceProvidersFeature();
    //    private readonly static Func<IFeatureCollection, IHttpAuthenticationFeature> _newHttpAuthenticationFeature
    //        = f => new HttpAuthenticationFeature();
    //    private readonly static Func<IFeatureCollection, IHttpRequestLifetimeFeature> _newHttpRequestLifetimeFeature
    //        = f => new HttpRequestLifetimeFeature();
    //    private readonly static Func<IFeatureCollection, ISessionFeature> _newSessionFeature 
    //        = f => new DefaultSessionFeature();
    //    private readonly static Func<IFeatureCollection, ISessionFeature> _nullSessionFeature
    //        = f => null;
    //    private readonly static Func<IFeatureCollection, IHttpRequestIdentifierFeature> _newHttpRequestIdentifierFeature
    //        = f => new HttpRequestIdentifierFeature();

    //    private FeatureReferences<FeatureInterfaces> _features;

    //    private HttpRequest _request;
    //    private HttpResponse _response;
    //    private AuthenticationManager _authenticationManager;
    //    private ConnectionInfo _connection;
    //    private WebSocketManager _websockets;

    //    public _HttpContext()
    //        : this(new FeatureCollection())
    //    {
    //        Features.Set<IHttpRequestFeature>(new HttpRequestFeature());
    //        Features.Set<IHttpResponseFeature>(new HttpResponseFeature());
    //    }

    //    public _HttpContext(IFeatureCollection features)
    //    {
    //        Initialize(features);
    //    }

    //    public virtual void Initialize(IFeatureCollection features)
    //    {
    //        _features = new FeatureReferences<FeatureInterfaces>(features);
    //        _request = InitializeHttpRequest();
    //        _response = InitializeHttpResponse();
    //    }

    //    public virtual void Uninitialize()
    //    {
    //        _features = default(FeatureReferences<FeatureInterfaces>);
    //        if (_request != null)
    //        {
    //            UninitializeHttpRequest(_request);
    //            _request = null;
    //        }
    //        if (_response != null)
    //        {
    //            UninitializeHttpResponse(_response);
    //            _response = null;
    //        }
    //        if (_authenticationManager != null)
    //        {
    //            UninitializeAuthenticationManager(_authenticationManager);
    //            _authenticationManager = null;
    //        }
    //        if (_connection != null)
    //        {
    //            UninitializeConnectionInfo(_connection);
    //            _connection = null;
    //        }
    //        if (_websockets != null)
    //        {
    //            UninitializeWebSocketManager(_websockets);
    //            _websockets = null;
    //        }
    //    }

    //    private IItemsFeature ItemsFeature =>
    //        _features.Fetch(ref _features.Cache.Items, _newItemsFeature);

    //    private IServiceProvidersFeature ServiceProvidersFeature =>
    //        _features.Fetch(ref _features.Cache.ServiceProviders, _newServiceProvidersFeature);

    //    private IHttpAuthenticationFeature HttpAuthenticationFeature =>
    //        _features.Fetch(ref _features.Cache.Authentication, _newHttpAuthenticationFeature);

    //    private IHttpRequestLifetimeFeature LifetimeFeature =>
    //        _features.Fetch(ref _features.Cache.Lifetime, _newHttpRequestLifetimeFeature);

    //    private ISessionFeature SessionFeature =>
    //        _features.Fetch(ref _features.Cache.Session, _newSessionFeature);

    //    private ISessionFeature SessionFeatureOrNull =>
    //        _features.Fetch(ref _features.Cache.Session, _nullSessionFeature);

    //    private IHttpRequestIdentifierFeature RequestIdentifierFeature =>
    //        _features.Fetch(ref _features.Cache.RequestIdentifier, _newHttpRequestIdentifierFeature);

    //    public override IFeatureCollection Features => _features.Collection;

    //    public override HttpRequest Request => _request;

    //    public override HttpResponse Response => _response;

    //    public override ConnectionInfo Connection => _connection ?? (_connection = InitializeConnectionInfo());

    //    public override AuthenticationManager Authentication => _authenticationManager ?? (_authenticationManager = InitializeAuthenticationManager());

    //    public override WebSocketManager WebSockets => _websockets ?? (_websockets = InitializeWebSocketManager());

    //    public override ClaimsPrincipal User
    //    {
    //        get
    //        {
    //            var user = HttpAuthenticationFeature.User;
    //            if (user == null)
    //            {
    //                user = new ClaimsPrincipal(new ClaimsIdentity());
    //                HttpAuthenticationFeature.User = user;
    //            }
    //            return user;
    //        }
    //        set { HttpAuthenticationFeature.User = value; }
    //    }

    //    public override IDictionary<object, object> Items
    //    {
    //        get { return ItemsFeature.Items; }
    //        set { ItemsFeature.Items = value; }
    //    }

    //    public override IServiceProvider RequestServices
    //    {
    //        get { return ServiceProvidersFeature.RequestServices; }
    //        set { ServiceProvidersFeature.RequestServices = value; }
    //    }

    //    public override CancellationToken RequestAborted
    //    {
    //        get { return LifetimeFeature.RequestAborted; }
    //        set { LifetimeFeature.RequestAborted = value; }
    //    }

    //    public override string TraceIdentifier
    //    {
    //        get { return RequestIdentifierFeature.TraceIdentifier; }
    //        set { RequestIdentifierFeature.TraceIdentifier = value; }
    //    }

    //    public override ISession Session
    //    {
    //        get
    //        {
    //            var feature = SessionFeatureOrNull;
    //            if (feature == null)
    //            {
    //                throw new InvalidOperationException("Session has not been configured for this application " +
    //                    "or request.");
    //            }
    //            return feature.Session;
    //        }
    //        set
    //        {
    //            SessionFeature.Session = value;
    //        }
    //    }



    //    public override void Abort()
    //    {
    //        LifetimeFeature.Abort();
    //    }


    //    protected virtual HttpRequest InitializeHttpRequest() => new DefaultHttpRequest(this);
    //    protected virtual void UninitializeHttpRequest(HttpRequest instance) { }

    //    protected virtual HttpResponse InitializeHttpResponse() => new DefaultHttpResponse(this);
    //    protected virtual void UninitializeHttpResponse(HttpResponse instance) { }

    //    protected virtual ConnectionInfo InitializeConnectionInfo() => new DefaultConnectionInfo(Features);
    //    protected virtual void UninitializeConnectionInfo(ConnectionInfo instance) { }

    //    protected virtual AuthenticationManager InitializeAuthenticationManager() => new DefaultAuthenticationManager(this);
    //    protected virtual void UninitializeAuthenticationManager(AuthenticationManager instance) { }

    //    protected virtual WebSocketManager InitializeWebSocketManager() => new DefaultWebSocketManager(Features);
    //    protected virtual void UninitializeWebSocketManager(WebSocketManager instance) { }

    //    struct FeatureInterfaces
    //    {
    //        public IItemsFeature Items;
    //        public IServiceProvidersFeature ServiceProviders;
    //        public IHttpAuthenticationFeature Authentication;
    //        public IHttpRequestLifetimeFeature Lifetime;
    //        public ISessionFeature Session;
    //        public IHttpRequestIdentifierFeature RequestIdentifier;
    //    }
    //}

    class _HttpContextFactory : IHttpContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FormOptions _formOptions;

        public _HttpContextFactory(ObjectPoolProvider poolProvider, IOptions<FormOptions> formOptions, IHttpContextAccessor httpContextAccessor)
        {
            if (poolProvider == null)
                throw new ArgumentNullException(nameof(poolProvider));
            if (formOptions == null)
                throw new ArgumentNullException(nameof(formOptions));
            _formOptions = formOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        HttpContext IHttpContextFactory.Create(IFeatureCollection featureCollection)
        {
            if (featureCollection == null)
            {
                throw new ArgumentNullException(nameof(featureCollection));
            }

            var httpContext = new _HttpContext(featureCollection);
            if (_httpContextAccessor != null)
            {
                _httpContextAccessor.HttpContext = httpContext;
            }

            var formFeature = new FormFeature(httpContext.Request, _formOptions);
            featureCollection.Set<IFormFeature>(formFeature);

            return httpContext;
        }

        void IHttpContextFactory.Dispose(HttpContext httpContext)
        {
            if (_httpContextAccessor != null)
            {
                _httpContextAccessor.HttpContext = null;
            }
        }
    }

    public static class _HttpContextExtensions
    {
        public static IWebHostBuilder UseExtendHttpContext(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services => services
            .Replace(ServiceDescriptor.Transient<IHttpContextFactory, _HttpContextFactory>())
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddSingleton<IStartupFilter, IStartupFilter>((serviceProvider) =>
            {
                _HttpContextExtensions.serviceProvider = serviceProvider;
                return new _StartupFilter();
            }));
        }

        class _StartupFilter : IStartupFilter
        {
            Action<IApplicationBuilder> IStartupFilter.Configure(Action<IApplicationBuilder> next) => next;
        }

        static IServiceProvider serviceProvider;

        public static _HttpContext GetHttpContext()
            => serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext as _HttpContext;
    }
}
namespace System.Web
{
    namespace Hosting
    {
        public static class HostingEnvironment
        {
            public static bool IsHosted
            {
                [DebuggerStepThrough]
                get;
            } = false;
        }
    }

    public static class HttpContext
    {
        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            [DebuggerStepThrough]
            get { return _HttpContextExtensions.GetHttpContext(); }
        }

        //public static Microsoft.AspNetCore.Http.HttpContext Current
        //{
        //    get { return (ServiceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor)?.HttpContext; }
        //}
    }
}
#endif