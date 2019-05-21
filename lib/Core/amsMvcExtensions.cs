using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace InnateGlory
{
    public static partial class amsExtensions
    {
        public static IMvcBuilder AddAMS(this IMvcBuilder mvc, Action<ActionSelectorOptions> actionSelectorOptions = null)
        {
            /// <see cref="MvcCoreServiceCollectionExtensions.AddMvcCoreServices"/>
            /// <see cref="MvcRazorMvcCoreBuilderExtensions"/>
            /// <see cref="MvcRazorPagesMvcCoreBuilderExtensions.AddServices"/>
            mvc.Services.Replace<IActionSelector>(d => new ServiceDescriptor(d.ServiceType, typeof(ExtraActionSelector), d.Lifetime));
            if (actionSelectorOptions != null)
                mvc.Services.Configure(actionSelectorOptions);

            mvc.Services.AddRandomID();

            //services.TryAddSingleton<IPageHandlerMethodSelector, DefaultPageHandlerMethodSelector>();

            //services.AddPageContextAccessor();

            //if (!services.HasService<MvcMarkerService>())
            //    return services;
            mvc.Services.AddActionContextAccessor();
            //CurrentControllerAccessor.AddService(services);

            //mvc.Services.AddLang();
            //mvc.Services.AddLocalization();

            #region //

            //services.TryAddTransient<IApplicationModelProvider, _ApplicationModelProvider>();
            //services.TryAddTransient<IPageApplicationModelProvider, _ApplicationModelProvider>();

            //for (int i = 0; i < services.Count; i++)
            //{
            //    var d = services[i];
            //    if (d.ServiceType == typeof(IApplicationModelProvider) &&
            //        d.ImplementationType == typeof(DefaultApplicationModelProvider))
            //        services[i] = ServiceDescriptor.Transient<IApplicationModelProvider, _ApplicationModelProvider>();
            //}

            #region ModelBinding, Validation
            /// <see cref="MvcCoreServiceCollectionExtensions.AddMvcCore(IServiceCollection, Action{MvcOptions})"/>

            //services.TryAddSingleton<IModelMetadataProvider, DefaultModelMetadataProvider>();
            //services.Replace(ServiceDescriptor.Singleton<IModelMetadataProvider, _ModelMetadataProvider>());
            //services.TryAdd(ServiceDescriptor.Transient<ICompositeMetadataDetailsProvider>(s =>
            //{
            //    var options = s.GetRequiredService<IOptions<MvcOptions>>().Value;
            //    return new DefaultCompositeMetadataDetailsProvider(options.ModelMetadataDetailsProviders);
            //}));
            //services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();
            //services.Replace(ServiceDescriptor.Singleton<IModelBinderFactory, _ModelBinderFactory>());
            //services.TryAddSingleton<IObjectModelValidator>(s =>
            //{
            //    var options = s.GetRequiredService<IOptions<MvcOptions>>().Value;
            //    var metadataProvider = s.GetRequiredService<IModelMetadataProvider>();
            //    return new DefaultObjectValidator(metadataProvider, options.ModelValidatorProviders);
            //});
            //services.TryAddSingleton<ClientValidatorCache>();
            //services.TryAddSingleton<ParameterBinder>();

            #endregion

            #endregion

            mvc.Services.TryAddSingleton<Api.ApiResultExecutor>();
            //services.Configure<MvcOptions>(opts =>
            mvc.AddMvcOptions(opts =>
            {
                //opts.ModelMetadataDetailsProviders.Insert(0, new BaseTypeMetadataDetailsProvider());
                opts.Filters.Add<Api.ApiExceptionFilter>();
                opts.Filters.Add<Api.ApiResultFilter>();
                opts.Filters.Add<AclFilter>();
                //opts.ValueProviderFactories.Add(new ApiValueProviderFactory());
                //opts.Filters.Add(new GenericFilter());
                //opts.ModelBinderProviders.Insert(0, new JsonObjectModelBinderProvider());
                opts.Conventions.Add(new _ApplicationModelConvention());
            });
            //services.Configure<MvcJsonOptions>(opts =>
            mvc.AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ConfigureMvcJsonOptions();
            });
            //mvc.Services.Configure<MvcViewOptions>(opts =>
            //{
            //    ;
            //});
            //mvc.Services.Configure<RazorViewEngineOptions>(opts =>
            //{
            //    ;
            //});
            //services.Configure<RazorPagesOptions>(opts =>
            //mvc.AddRazorPagesOptions(opts =>
            //{
            //    opts.Conventions.Add(new _ApplicationModelConvention());
            //});
            return mvc;
        }
    }
}