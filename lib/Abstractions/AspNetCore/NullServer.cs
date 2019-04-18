using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    public sealed class NullServer : IServer
    {
        IFeatureCollection IServer.Features { get; } = new FeatureCollection();

        void IDisposable.Dispose()
        {
        }

        async Task IServer.StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        async Task IServer.StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
    //public sealed class NullServerOptions
    //{
    //}
    //public sealed class NullServerOptionsSetup : IConfigureOptions<NullServerOptions>
    //{
    //    void IConfigureOptions<NullServerOptions>.Configure(NullServerOptions options)
    //    {
    //    }
    //}
    public static class _Extensions
    {
        public static IWebHostBuilder UseNullServer(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                //services.AddTransient<IConfigureOptions<NullServerOptions>, NullServerOptionsSetup>();
                services.AddSingleton<IServer, NullServer>();
            });
        }

        //public static IWebHostBuilder UseNullServer(this IWebHostBuilder hostBuilder, Action<NullServerOptions> options)
        //{
        //    return hostBuilder.UseNullServer().ConfigureServices(services =>
        //    {
        //        services.Configure(options);
        //    });
        //}
    }
}
