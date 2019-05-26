using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace InnateGlory
{
    public static class LangServiceExtensions
    {
        /// <see cref="Microsoft.Extensions.DependencyInjection.LocalizationServiceCollectionExtensions.AddLocalizationServices(IServiceCollection)"/>
        /// <see cref="Microsoft.AspNetCore.Mvc.Localization.Internal.MvcLocalizationServices.AddMvcLocalizationServices(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat, Action{LocalizationOptions})"/>
        public static IMvcBuilder AddLang(this IMvcBuilder mvc, PlatformId defaultPlatformId)
        {
            mvc.Services.AddLocalization();
            //mvc.Services.TryAddSingleton<LangService>();
            mvc.Services.TryAddTransient<IViewLang, ViewLang>();
            mvc.Services.Configure<ViewLangOptions>(opts => opts.DefaultPlatformId = defaultPlatformId);
            mvc.AddDataAnnotationsLocalization(/*opts =>
            {
                opts.DataAnnotationLocalizerProvider = (modelType, stringLocalizerFactory) =>
                {
                    return stringLocalizerFactory.Create(modelType);
                };
            }*/);
            //InnateGlory.loc
            //mvc.InitializeTagHelper()
            mvc.AddViewLocalization();
            return mvc;
        }

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);
        //private static IViewLang GetViewLang(IServiceProvider services) => new ViewLang(services.GetRequiredService<LangService>());
    }

    public class ViewLangOptions
    {
        public PlatformId DefaultPlatformId;
    }
}
