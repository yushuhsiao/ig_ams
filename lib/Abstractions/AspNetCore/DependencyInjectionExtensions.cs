using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class _Extensions
    {
        #region AddStartup

        private class _StartupFilter : IStartupFilter
        {
            Action<IApplicationBuilder> startup;
            Action<IApplicationBuilder> next;
            public _StartupFilter(Action<IApplicationBuilder> startup)
            {
                this.startup = startup;
            }
            Action<IApplicationBuilder> IStartupFilter.Configure(Action<IApplicationBuilder> next)
            {
                this.next = next;
                return Configure;
            }
            void Configure(IApplicationBuilder app)
            {
                startup?.Invoke(app);
                next(app);
            }
        }

        public static IServiceCollection AddStartup(this IServiceCollection services, Action<IApplicationBuilder> startup) => services.AddSingleton<IStartupFilter>(new _StartupFilter(startup));

        #endregion

        #region Configure

        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Action<string, IServiceProvider, TOptions> configureOptions) where TOptions : class
          => services.Configure(Options.Options.DefaultName, configureOptions);

        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string name, Action<string, IServiceProvider, TOptions> configureOptions)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            int index = -1;
            for (int i = 0; index == -1 && i < services.Count; i++)
            {
                var d = services[i];
                if (d.ImplementationType == typeof(_ConfigureOptions<TOptions>))
                    index = i;
            }


            //services.AddSingleton<IConfigureOptions<TOptions>>(new _ConfigureOptions<TOptions>(null, name, configureOptions));
            _ConfigureOptions<TOptions>.Add(name, configureOptions);
            //services.TryAddSingleton<xxx<TOptions>, _ConfigureOptions<TOptions>>(); //(new _ConfigureOptions<TOptions>(null, name, configureOptions));
            if (index == -1)
                services.AddSingleton<IConfigureOptions<TOptions>, _ConfigureOptions<TOptions>>(); //(new _ConfigureOptions<TOptions>(null, name, configureOptions));
            //services.AddSingleton<IConfigureOptions<TOptions>>(new _ConfigureOptions<TOptions>(null, name, configureOptions));
            //services.TryAddSingleton<IConfigureOptions<TOptions>, _ConfigureOptions<TOptions>>();
            return services;
        }

        //interface xxx<TOptions> : IConfigureNamedOptions<TOptions> where TOptions : class
        //{
        //}

        private class _ConfigureOptions<TOptions> : IConfigureNamedOptions<TOptions> where TOptions : class
        {
            private static Dictionary<string, List<Action<string, IServiceProvider, TOptions>>> _actions = new Dictionary<string, List<Action<string, IServiceProvider, TOptions>>>();
            public static void Add(string name, Action<string, IServiceProvider, TOptions> action)
            {
                lock (_actions)
                {
                    _actions.GetValue(name ?? "", () => new List<Action<string, IServiceProvider, TOptions>>()).TryAdd(action);
                }
            }

            private IServiceProvider ServiceProvider { get; }
            public string Name { get; }
            public Action<IServiceProvider, TOptions> Action { get; }

            public _ConfigureOptions(IServiceProvider services)
            {
                this.ServiceProvider = services;
            }

            public void Configure(string name, TOptions options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                lock (_actions)
                {
                    if (_actions.TryGetValue(name, out var list))
                        for (int i = 0; i < list.Count; i++)
                            list[i](name, ServiceProvider, options);
                }

                // Null name is used to configure all named options.
                //if (Name == null || name == Name)
                //{
                //    Action?.Invoke(ServiceProvider, options);
                //}
            }

            public void Configure(TOptions options) => this.Configure(Options.Options.DefaultName, options);
        }

        //private class _PostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions> where TOptions : class
        //{
        //    private string name;
        //    private Action<IServiceProvider, TOptions> configureOptions;

        //    public _PostConfigureOptions(string name, Action<IServiceProvider, TOptions> configureOptions)
        //    {
        //        this.name = name;
        //        this.configureOptions = configureOptions;
        //    }

        //    void IPostConfigureOptions<TOptions>.PostConfigure(string name, TOptions options)
        //    {
        //    }
        //}

        //public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, Action<IServiceProvider, TOptions> configureOptions) where TOptions : class
        //    => services.PostConfigure(Options.Options.DefaultName, configureOptions);

        //public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, string name, Action<IServiceProvider, TOptions> configureOptions)
        //    where TOptions : class
        //{
        //    if (services == null)
        //    {
        //        throw new ArgumentNullException(nameof(services));
        //    }

        //    if (configureOptions == null)
        //    {
        //        throw new ArgumentNullException(nameof(configureOptions));
        //    }

        //    services.AddSingleton<IPostConfigureOptions<TOptions>>(new _PostConfigureOptions<TOptions>(name, configureOptions));
        //    return services;
        //}

        #endregion
    }
}
namespace Microsoft.AspNetCore.Builder
{
    public static class _Extensions
    {
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app, Action<StaticFileOptions> configure)
        {
            var opts = app.ApplicationServices.GetService<IOptions<StaticFileOptions>>().Value
                ?? new StaticFileOptions();
            configure?.Invoke(opts);
            return app.UseStaticFiles(opts);
        }
    }
}