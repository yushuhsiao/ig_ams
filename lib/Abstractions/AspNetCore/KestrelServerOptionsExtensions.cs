using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Microsoft.AspNetCore.Hosting
{
    public static class KestrelServerOptionsExtensions
    {
        private const string SectionName = "KestrelServer";

        private class HttpOptions
        {
            public int? Port { get; set; }
            public bool Logging { get; set; } = false;
            public string pfx { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// Set KestrelServerOptions from appsettings.json
        /// </summary>
        /// <param name="options"></param>
        public static void BindConfiguration(this KestrelServerOptions options, string sectionName = SectionName)
        {
            //var s_opt = options.ApplicationServices.GetService<IOptions<ServerOptions>>()?.Value;
            var config = options.ApplicationServices.GetService<IConfiguration>()?.GetSection(sectionName ?? SectionName);
            options.AddServerHeader = false;
            if (config != null)
            {
                config.Bind(options);
                var http = config.Bind<HttpOptions>("Http");
                var https = config.Bind<HttpOptions>("Https");

                if (http.Port.HasValue)
                {
                    options.Listen(IPAddress.Any, http.Port.Value, listenOptions =>
                    {
                        if (http.Logging)
                            listenOptions.UseConnectionLogging();
                    });
                }
                if (https.Port.HasValue && !string.IsNullOrEmpty(https.pfx) && !string.IsNullOrEmpty(https.Password))
                {
                    options.Listen(IPAddress.Any, https.Port.Value, listenOptions =>
                    {
                        listenOptions.UseHttps(https.pfx, https.Password);
                        if (http.Logging)
                            listenOptions.UseConnectionLogging();
                    });
                }

            }
        }

        //public static IServiceCollection ConfigureKestrel(this IServiceCollection services) => services.ConfigureKestrel(null);
        //public static IServiceCollection ConfigureKestrel(this IServiceCollection services, Action<KestrelServerOptions> options)
        //{
        //    services.Configure<KestrelServerOptions>(_options =>
        //    {
        //        _options.AddServerHeader = false;
        //        var config = _options.ApplicationServices.GetRequiredService<IConfiguration>();
        //        int? http = config.GetValue<int>("Http");
        //        config = config.GetSection("Https");
        //        int? https = config.GetValue<int>("Port");
        //        string pfx = config.GetValue<string>("pfx");
        //        string pwd = config.GetValue<string>("Password");

        //        if (http.HasValue)
        //        {
        //            _options.Listen(IPAddress.Any, http.Value, listenOptions =>
        //            {
        //                //listenOptions.UseConnectionLogging();
        //            });
        //        }
        //        if (https.HasValue && !string.IsNullOrEmpty(pfx) && !string.IsNullOrEmpty(pwd))
        //        {
        //            _options.Listen(IPAddress.Any, https.Value, listenOptions =>
        //            {
        //                listenOptions.UseHttps(pfx, pwd);
        //                //listenOptions.UseConnectionLogging();
        //            });
        //        }
        //        options?.Invoke(_options);
        //    });
        //    return services;
        //}
    }
}