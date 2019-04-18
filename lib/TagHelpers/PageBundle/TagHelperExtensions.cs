using InnateGlory.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnateGlory
{
    public static partial class TagHelperExtensions
    {
        public static IServiceCollection AddPageBundlesTagHelper(this IServiceCollection services)
        {
            //services.Configure<StaticFileOptions>((name, p, opts) =>
            //{
            //    opts.FileProvider = p.GetService<PageBundleFileProvider>();
            //});

            services.TryAddSingleton<PageBundleFileProvider>();
            return services;
        }

        public static IApplicationBuilder UsePageBundlesTagHelper(this IApplicationBuilder app, FileServerOptions options = null)
        {
            var obj = app.ApplicationServices.GetService<PageBundleFileProvider>();
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