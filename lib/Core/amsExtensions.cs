using InnateGlory.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
    public static partial class amsExtensions
    {
        //[DebuggerStepThrough]
        //public static bool IsSuccess(this Status s) => s == Status.Success;
        //[DebuggerStepThrough]
        //public static bool IsNotSuccess(this Status s) => s != Status.Success;

        //private class ServerOptionsSetup : IConfigureOptions<ServerOptions> { void IConfigureOptions<ServerOptions>.Configure(ServerOptions options) { } }


        private static SqlConnection CreateSqlConnection(DbConnectionString cn) => new SqlConnection(cn);
        private static void RegisterForDispose(object state, IDisposable item)
        {
            HttpContext context = state as HttpContext;
            if (context != null)
                context.Response.RegisterForDispose(item);
        }

        public static IServiceCollection AddAMS(this IServiceCollection services/*, Action<ServerOptions> options = null*/)
        {
            services.AddHttpContextAccessor();
            //services.AddConfigurationBinder();

            services.AddSqlCmdPooling(
                HttpContextExtensions.GetHttpContext,
                RegisterForDispose);

            services.AddDbConnectionPooling(
                CreateSqlConnection,
                HttpContextExtensions.GetHttpContext,
                RegisterForDispose);

            services.AddLogging(logging => logging.InjectConsole().AddSql().AddTextFile());
            //services.AddSingleton<ILoggerProvider, Tools.Logging.SqlLoggerProvider>();
            //services.AddSingleton<ILoggerProvider, Tools.Logging.TextFileLoggerProvider>();

            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ServerOptions>, ServerOptionsSetup>());
            //services.Configure(options ?? _null.noop);
            services.TryAddSingleton(serviceProvider => new DbCache(serviceProvider));
            //services.AddSqlConfig();
            services.AddConfigurationBinder();
            services.TryAddSingleton<ServerInfo>();
            services.TryAddSingleton<MessageInvoker<RedisActionAttribute>>();
            services.TryAddSingleton<Redis.RedisMessager>();
            services.TryAddSingleton(serviceProvider => new DataService(serviceProvider));
            services.TryAddTransient(RequestBody.GetRequestBody);
            //services.AddStartup(startup);
            //services.UseHttpContext<_HttpContext>();
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AddServerHeader = false;
            });
            return services.AddStartup(app =>
            {
                Global.ServiceProvider = app.ApplicationServices;
                //app.ApplicationServices.GetService<ServerInfo>();
                try
                {
                    //app.ApplicationServices.GetService<Redis.RedisMessager>();
                }
                catch { }
                //LoggerHelper.LoggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                //app.ApplicationServices.GetServiceOrCreateInstance<ServerInfo>();
                app.Use((context, next) =>
                {
                    Global.ServiceProvider = context.RequestServices;
                    return next();
                });
            });
        }

        //private static async Task _ServiceProvider_Middleware(HttpContext context, Func<Task> next)
        //{
        //    Global.ServiceProvider = context.RequestServices;
        //    //context.User.SetUserId(1001);
        //    //var userId = context.RequestServices.GetCurrentUserId();
        //    try { await next(); }
        //    finally
        //    {
        //        //SqlCmdPooling.Release(context);
        //        //context.RemoveItems<IHttpContextDispose>(x => x.Dispose(context));
        //    }
        //    Global.ServiceProvider = null;
        //}

        //private static void startup(IApplicationBuilder app)
        //{
        //    app.ApplicationServices.GetServiceOrCreateInstance<ServerInfo>();
        //    app.Use(middleware);
        //}

        //private static async Task middleware(HttpContext context, Func<Task> next)
        //{
        //    try { await next(); }
        //    finally { context.RemoveItems<IHttpContextDispose>(x => x.Dispose(context)); }
        //}


        //public static T DataService<T>(this IServiceProvider services) => services.DataService().GetServiceOrCreateInstance<T>();

        //private class CurrentControllerAccessor : IActionFilter
        //{
        //    void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        //    {
        //        context.HttpContext.Items[typeof(CurrentControllerAccessor)] = context.Controller;
        //    }
        //    void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        //    {
        //        context.HttpContext.Items.Remove(typeof(CurrentControllerAccessor));
        //    }

        //    public static void AddService(IServiceCollection services)
        //    {
        //        return;
        //        services.TryAddScoped(_CurrentController);
        //        services.Configure<MvcOptions>(opts => opts.Filters.Add<CurrentControllerAccessor>());
        //    }
        //    private static Controller _CurrentController(IServiceProvider provider)
        //    {
        //        return provider.GetService<IHttpContextAccessor>()?.HttpContext?.Items[typeof(CurrentControllerAccessor)] as Controller;
        //    }
        //}

        //class _ServiceInject<T>
        //{
        //    protected T _inner;

        //    public _ServiceInject(IServiceProvider s)
        //    {
        //        _inner = s.CreateInstance<T>();
        //    }

        //    public static ServiceDescriptor inject(ServiceDescriptor d)
        //    {
        //        if (d.ImplementationType == typeof(T))
        //            return new ServiceDescriptor(d.ServiceType, typeof(T), d.Lifetime);
        //        return d;
        //    }
        //}

        //class _ActionInvokerFactory : _ServiceInject<ActionInvokerFactory>, IActionInvokerFactory
        //{
        //    public _ActionInvokerFactory(IServiceProvider s) : base(s) { }

        //    public IActionInvoker CreateInvoker(ActionContext actionContext)
        //    {
        //        return ((IActionInvokerFactory)_inner).CreateInvoker(actionContext);
        //    }
        //}




        //public static IWebHostBuilder RegisterServer(this IWebHostBuilder builder, Action<ServerOptions> options = null)
        //{
        //    return builder.ConfigureServices(services => services.RegisterServer()).ReplaceHttpContextFactory();
        //}

        // memo
        //static void xxx(IServiceProvider provider)
        //{
        //    provider.GetRequiredService<IHostingEnvironment>();
        //    provider.GetRequiredService<WebHostBuilderContext>();
        //    provider.GetRequiredService<DiagnosticListener>();
        //    provider.GetRequiredService<DiagnosticSource>();
        //    //provider.GetRequiredService<IApplicationBuilderFactory>();
        //    provider.GetRequiredService<IHttpContextFactory>();
        //    provider.GetRequiredService<IMiddlewareFactory>();
        //    provider.GetRequiredService<ObjectPoolProvider>();
        //}
    }
    //public class _Scoped
    //{
    //    public _Scoped()
    //    {
    //    }
    //}
    //public class _Transient
    //{
    //    public _Transient()
    //    {
    //    }
    //}
    /*
IHostingEnvironment	
WebHostBuilderContext	
IConfiguration	
IApplicationBuilderFactory	ApplicationBuilderFactory
IHttpContextFactory	HttpContextFactory
IMiddlewareFactory	MiddlewareFactory
IOptions`1 <TOptions>	OptionsManager`1 <TOptions>
IOptionsSnapshot`1 <TOptions>	OptionsManager`1 <TOptions>
IOptionsMonitor`1 <TOptions>	OptionsMonitor`1 <TOptions>
IOptionsFactory`1 <TOptions>	OptionsFactory`1 <TOptions>
IOptionsMonitorCache`1 <TOptions>	OptionsCache`1 <TOptions>
ILoggerFactory	LoggerFactory
ILogger`1 <TCategoryName>	Logger`1 <T>
IConfigureOptions`1 <LoggerFilterOptions>	
IStartupFilter	AutoRequestServicesStartupFilter
ObjectPoolProvider	DefaultObjectPoolProvider
ITransportFactory	LibuvTransportFactory
IConfigureOptions`1 <KestrelServerOptions>	KestrelServerOptionsSetup
IServer	KestrelServer
IConfigureOptions`1 <LoggerFilterOptions>	
IOptionsChangeTokenSource`1 <LoggerFilterOptions>	
ILoggerProvider	ConsoleLoggerProvider
ILoggerProvider	DebugLoggerProvider
IServiceProviderFactory`1 <IServiceCollection>	
ITransportFactory	LibuvTransportFactory
IConfigureOptions`1 <KestrelServerOptions>	KestrelServerOptionsSetup
IServer	KestrelServer
IConfigureOptions`1 <KestrelServerOptions>	
IStartup	
IHttpContextAccessor	HttpContextAccessor
ITelemetryInitializer	AzureWebAppRoleEnvironmentTelemetryInitializer
ITelemetryInitializer	DomainNameRoleInstanceTelemetryInitializer
ITelemetryInitializer	ComponentVersionTelemetryInitializer
ITelemetryInitializer	ClientIpHeaderTelemetryInitializer
ITelemetryInitializer	OperationNameTelemetryInitializer
ITelemetryInitializer	OperationCorrelationTelemetryInitializer
ITelemetryInitializer	SyntheticTelemetryInitializer
ITelemetryInitializer	WebSessionTelemetryInitializer
ITelemetryInitializer	WebUserTelemetryInitializer
ITelemetryInitializer	AspNetCoreEnvironmentTelemetryInitializer
ITelemetryInitializer	HttpDependenciesParsingTelemetryInitializer
ITelemetryModule	
TelemetryConfiguration	
ICorrelationIdLookupHelper	
TelemetryClient	TelemetryClient
ApplicationInsightsInitializer	ApplicationInsightsInitializer
IApplicationInsightDiagnosticListener	HostingDiagnosticListener
IApplicationInsightDiagnosticListener	MvcDiagnosticsListener
IStartupFilter	ApplicationInsightsStartupFilter
JavaScriptSnippet	JavaScriptSnippet
ApplicationInsightsLoggerEvents	ApplicationInsightsLoggerEvents
IOptions`1 <TelemetryConfiguration>	TelemetryConfigurationOptions
IConfigureOptions`1 <TelemetryConfiguration>	TelemetryConfigurationOptionsSetup
IConfigureOptions`1 <ApplicationInsightsServiceOptions>	DefaultApplicationInsightsServiceConfigureOptions
IStartupFilter	ApplicationInsightsLoggerStartupFilter
ITagHelperComponent	JavaScriptSnippetTagHelperComponent
DiagnosticListener	
DiagnosticSource	
IApplicationLifetime	ApplicationLifetime
HostedServiceExecutor	HostedServiceExecutor
ApplicationPartManager	
IInlineConstraintResolver	DefaultInlineConstraintResolver
UrlEncoder	
ObjectPool`1 <UriBuildingContext>	
TreeRouteBuilder	TreeRouteBuilder
RoutingMarkerService	RoutingMarkerService
IConfigureOptions`1 <MvcOptions>	MvcCoreMvcOptionsSetup
IConfigureOptions`1 <RouteOptions>	MvcCoreRouteOptionsSetup
IApplicationModelProvider	DefaultApplicationModelProvider
IActionDescriptorProvider	ControllerActionDescriptorProvider
IActionDescriptorCollectionProvider	ActionDescriptorCollectionProvider
IActionSelector	ActionSelector
ActionConstraintCache	ActionConstraintCache
IActionConstraintProvider	DefaultActionConstraintProvider
IControllerFactory	DefaultControllerFactory
IControllerActivator	DefaultControllerActivator
IControllerFactoryProvider	ControllerFactoryProvider
IControllerActivatorProvider	ControllerActivatorProvider
IControllerPropertyActivator	DefaultControllerPropertyActivator
IActionInvokerFactory	ActionInvokerFactory
IActionInvokerProvider	ControllerActionInvokerProvider
ControllerActionInvokerCache	ControllerActionInvokerCache
IFilterProvider	DefaultFilterProvider
RequestSizeLimitResourceFilter	RequestSizeLimitResourceFilter
DisableRequestSizeLimitResourceFilter	DisableRequestSizeLimitResourceFilter
IModelMetadataProvider	DefaultModelMetadataProvider
ICompositeMetadataDetailsProvider	
IModelBinderFactory	ModelBinderFactory
IObjectModelValidator	
ClientValidatorCache	ClientValidatorCache
ParameterBinder	ParameterBinder
MvcMarkerService	MvcMarkerService
ITypeActivatorCache	TypeActivatorCache
IUrlHelperFactory	UrlHelperFactory
IHttpRequestStreamReaderFactory	MemoryPoolHttpRequestStreamReaderFactory
IHttpResponseStreamWriterFactory	MemoryPoolHttpResponseStreamWriterFactory
ArrayPool`1 <Byte>	
ArrayPool`1 <Char>	
ObjectResultExecutor	ObjectResultExecutor
PhysicalFileResultExecutor	PhysicalFileResultExecutor
VirtualFileResultExecutor	VirtualFileResultExecutor
FileStreamResultExecutor	FileStreamResultExecutor
FileContentResultExecutor	FileContentResultExecutor
RedirectResultExecutor	RedirectResultExecutor
LocalRedirectResultExecutor	LocalRedirectResultExecutor
RedirectToActionResultExecutor	RedirectToActionResultExecutor
RedirectToRouteResultExecutor	RedirectToRouteResultExecutor
RedirectToPageResultExecutor	RedirectToPageResultExecutor
ContentResultExecutor	ContentResultExecutor
MvcRouteHandler	MvcRouteHandler
MvcAttributeRouteHandler	MvcAttributeRouteHandler
MiddlewareFilterConfigurationProvider	MiddlewareFilterConfigurationProvider
MiddlewareFilterBuilder	MiddlewareFilterBuilder
IApiDescriptionGroupCollectionProvider	ApiDescriptionGroupCollectionProvider
IApiDescriptionProvider	DefaultApiDescriptionProvider
IAuthenticationService	AuthenticationService
IClaimsTransformation	NoopClaimsTransformation
IAuthenticationHandlerProvider	AuthenticationHandlerProvider
IAuthenticationSchemeProvider	AuthenticationSchemeProvider
IAuthorizationService	DefaultAuthorizationService
IAuthorizationPolicyProvider	DefaultAuthorizationPolicyProvider
IAuthorizationHandlerProvider	DefaultAuthorizationHandlerProvider
IAuthorizationEvaluator	DefaultAuthorizationEvaluator
IAuthorizationHandlerContextFactory	DefaultAuthorizationHandlerContextFactory
IAuthorizationHandler	PassThroughAuthorizationHandler
IPolicyEvaluator	PolicyEvaluator
IApplicationModelProvider	AuthorizationApplicationModelProvider
FormatFilter	FormatFilter
IConfigureOptions`1 <MvcOptions>	MvcDataAnnotationsMvcOptionsSetup
IValidationAttributeAdapterProvider	ValidationAttributeAdapterProvider
IActivator	TypeForwardingActivator
RegistryPolicyResolver	RegistryPolicyResolver
IConfigureOptions`1 <KeyManagementOptions>	KeyManagementOptionsSetup
IConfigureOptions`1 <DataProtectionOptions>	DataProtectionOptionsSetup
IKeyManager	XmlKeyManager
IApplicationDiscriminator	HostingApplicationDiscriminator
IStartupFilter	DataProtectionStartupFilter
IDefaultKeyResolver	DefaultKeyResolver
IKeyRingProvider	KeyRingProvider
IDataProtectionProvider	
ICertificateResolver	CertificateResolver
IConfigureOptions`1 <AntiforgeryOptions>	AntiforgeryOptionsSetup
IAntiforgery	DefaultAntiforgery
IAntiforgeryTokenGenerator	DefaultAntiforgeryTokenGenerator
IAntiforgeryTokenSerializer	DefaultAntiforgeryTokenSerializer
IAntiforgeryTokenStore	DefaultAntiforgeryTokenStore
IClaimUidExtractor	DefaultClaimUidExtractor
IAntiforgeryAdditionalDataProvider	DefaultAntiforgeryAdditionalDataProvider
ObjectPool`1 <AntiforgerySerializationContext>	
HtmlEncoder	
JavaScriptEncoder	
IConfigureOptions`1 <MvcViewOptions>	MvcViewOptionsSetup
IConfigureOptions`1 <MvcOptions>	TempDataMvcOptionsSetup
ICompositeViewEngine	CompositeViewEngine
ViewResultExecutor	ViewResultExecutor
PartialViewResultExecutor	PartialViewResultExecutor
IControllerPropertyActivator	ViewDataDictionaryControllerPropertyActivator
IHtmlHelper	HtmlHelper
IHtmlHelper`1 <TModel>	HtmlHelper`1 <TModel>
IHtmlGenerator	DefaultHtmlGenerator
ExpressionTextCache	ExpressionTextCache
IModelExpressionProvider	ModelExpressionProvider
ValidationHtmlAttributeProvider	DefaultValidationHtmlAttributeProvider
IJsonHelper	JsonHelper
JsonOutputFormatter	
IViewComponentSelector	DefaultViewComponentSelector
IViewComponentFactory	DefaultViewComponentFactory
IViewComponentActivator	DefaultViewComponentActivator
IViewComponentDescriptorCollectionProvider	DefaultViewComponentDescriptorCollectionProvider
ViewComponentResultExecutor	ViewComponentResultExecutor
ViewComponentInvokerCache	ViewComponentInvokerCache
IViewComponentDescriptorProvider	DefaultViewComponentDescriptorProvider
IViewComponentInvokerFactory	DefaultViewComponentInvokerFactory
IViewComponentHelper	DefaultViewComponentHelper
IApplicationModelProvider	TempDataApplicationModelProvider
SaveTempDataFilter	SaveTempDataFilter
ControllerSaveTempDataPropertyFilter	ControllerSaveTempDataPropertyFilter
ITempDataProvider	CookieTempDataProvider
ValidateAntiforgeryTokenAuthorizationFilter	ValidateAntiforgeryTokenAuthorizationFilter
AutoValidateAntiforgeryTokenAuthorizationFilter	AutoValidateAntiforgeryTokenAuthorizationFilter
ITempDataDictionaryFactory	TempDataDictionaryFactory
ArrayPool`1 <ViewBufferValue>	
IViewBufferScope	MemoryPoolViewBufferScope
CSharpCompiler	CSharpCompiler
RazorReferenceManager	DefaultRazorReferenceManager
IConfigureOptions`1 <MvcViewOptions>	MvcRazorMvcViewOptionsSetup
IConfigureOptions`1 <RazorViewEngineOptions>	RazorViewEngineOptionsSetup
IRazorViewEngineFileProviderAccessor	DefaultRazorViewEngineFileProviderAccessor
IRazorViewEngine	RazorViewEngine
IViewCompilerProvider	RazorViewCompilerProvider
IRazorPageFactoryProvider	DefaultRazorPageFactoryProvider
RazorProject	FileProviderRazorProject
RazorTemplateEngine	MvcRazorTemplateEngine
LazyMetadataReferenceFeature	LazyMetadataReferenceFeature
RazorEngine	
IRazorPageActivator	RazorPageActivator
ITagHelperActivator	DefaultTagHelperActivator
ITagHelperFactory	DefaultTagHelperFactory
ITagHelperComponentManager	TagHelperComponentManager
IMemoryCache	MemoryCache
IConfigureOptions`1 <RazorViewEngineOptions>	RazorPagesRazorViewEngineOptionsSetup
IActionDescriptorProvider	PageActionDescriptorProvider
IActionDescriptorChangeProvider	PageActionDescriptorChangeProvider
IPageRouteModelProvider	RazorProjectPageRouteModelProvider
IPageRouteModelProvider	CompiledPageRouteModelProvider
IPageApplicationModelProvider	DefaultPageApplicationModelProvider
IPageApplicationModelProvider	AutoValidateAntiforgeryPageApplicationModelProvider
IPageApplicationModelProvider	AuthorizationPageApplicationModelProvider
IPageApplicationModelProvider	TempDataFilterPageApplicationModelProvider
IActionInvokerProvider	PageActionInvokerProvider
IPageModelActivatorProvider	DefaultPageModelActivatorProvider
IPageModelFactoryProvider	DefaultPageModelFactoryProvider
IPageActivatorProvider	DefaultPageActivatorProvider
IPageFactoryProvider	DefaultPageFactoryProvider
IPageLoader	DefaultPageLoader
IPageHandlerMethodSelector	DefaultPageHandlerMethodSelector
PageArgumentBinder	DefaultPageArgumentBinder
PageResultExecutor	PageResultExecutor
PageSaveTempDataPropertyFilter	PageSaveTempDataPropertyFilter
IDistributedCacheTagHelperStorage	DistributedCacheTagHelperStorage
IDistributedCacheTagHelperFormatter	DistributedCacheTagHelperFormatter
IDistributedCacheTagHelperService	DistributedCacheTagHelperService
IDistributedCache	MemoryDistributedCache
CacheTagHelperMemoryCacheFactory	CacheTagHelperMemoryCacheFactory
IConfigureOptions`1 <MvcOptions>	MvcJsonMvcOptionsSetup
IApiDescriptionProvider	JsonPatchOperationsArrayProvider
JsonResultExecutor	JsonResultExecutor
ICorsService	CorsService
ICorsPolicyProvider	DefaultCorsPolicyProvider
IApplicationModelProvider	CorsApplicationModelProvider
CorsAuthorizationFilter	CorsAuthorizationFilter
IConfigureOptions`1 <ServerOptions>	
DbCache	DbCache
DbCache`1 <TValue>	DbCache`1 <TValue>
SqlConfig	SqlConfig
SqlConfig`1 <T>	SqlConfig`1 <T>
MessageInvoker`1 <RedisActionAttribute>	MessageInvoker`1 <RedisActionAttribute>
ServerInfo	ServerInfo

    */
}