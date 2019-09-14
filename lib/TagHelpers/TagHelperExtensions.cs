using InnateGlory.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnateGlory
{
    public static partial class TagHelperExtensions
    {
        public static IMvcBuilder AddViewServices(this IMvcBuilder mvc)
        {
            return mvc
                .AddRazorPagesOptions(opts =>
                {
                    ; //opts.Conventions.AuthorizeFolder("/").AllowAnonymousToPage("/Login.cshtml");
                })
                .AddViewConfig()
                .AddViewLang(defaultPlatformId: 0)
                .AddRandomId()
                .AddPageBundlesTagHelper();
        }

        public static IMvcBuilder AddPageBundlesTagHelper(this IMvcBuilder mvc)
        {
            //services.Configure<StaticFileOptions>((name, p, opts) =>
            //{
            //    opts.FileProvider = p.GetService<PageBundleFileProvider>();
            //});

            mvc.Services.TryAddSingleton<PageBundleFileProvider>();
            return mvc;
        }

        public static IApplicationBuilder UsePageBundlesTagHelper(this IApplicationBuilder app, FileServerOptions options = null)
        {
            var obj = ActivatorUtilities.GetServiceOrCreateInstance<PageBundleFileProvider>(app.ApplicationServices);
            //var obj = app.ApplicationServices.GetService<PageBundleFileProvider>();
            if (obj != null)
            {
                if (options == null)
                    options = new FileServerOptions();
                options.FileProvider = obj;
                app.UseFileServer(options);
            }
            return app;
        }
    }
}