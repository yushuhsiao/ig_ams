using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ams_blazor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Mono.WebAssembly.Interop.MonoWebAssemblyJSRuntime>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            //app.AddComponent<App>("app");
            var js = app.Services.GetService<Mono.WebAssembly.Interop.MonoWebAssemblyJSRuntime>();

            //js.Invoke<object>("test", 1, 2, 3);
            //Console.WriteLine("app");
            var js = app.Services.GetService<Mono.WebAssembly.Interop.MonoWebAssemblyJSRuntime>();

            js.Invoke<object>("test", 1, 2, 3);
            Console.WriteLine("app");
        }
    }
}
