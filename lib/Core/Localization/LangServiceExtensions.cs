using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics;

namespace InnateGlory
{
    public static class LangServiceExtensions
    {
        /// <see cref="Microsoft.Extensions.DependencyInjection.LocalizationServiceCollectionExtensions.AddLocalizationServices(IServiceCollection)"/>
        /// <see cref="Microsoft.AspNetCore.Mvc.Localization.Internal.MvcLocalizationServices.AddMvcLocalizationServices(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat, Action{LocalizationOptions})"/>
        public static IServiceCollection AddLang(this IServiceCollection services)
        {
            services.AddLocalization();
            services.TryAddSingleton<LangService>();
            services.TryAddTransient(GetViewLang);
            return services;
        }

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);

        [DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);
        private static IViewLang GetViewLang(IServiceProvider services) => new ViewLang(services.GetRequiredService<LangService>());
    }
}
