using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using InnateGlory;

namespace InnateGlory
{
    internal class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUserManager();
            services.AddAMS();
            services.AddMvc(options =>
            {
            }).AddAMS(actionSelectorOptions: options =>
            {
            })
            .AddLang()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR(opts =>
            {
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ams api", Version = "v1" });
                c.IncludeXmlComments(typeof(Startup), typeof(JsonHelper), typeof(amsExtensions), typeof(Models.LoginModel));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseSqlAppSettings();

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            //app.UseHttpsRedirection();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                ;
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<Hub1>("/hub1", (HttpConnectionDispatcherOptions opts) =>
                {
                    //opts.Transports = HttpTransportType.LongPolling;
                    //opts.LongPolling.PollTimeout = TimeSpan.FromSeconds(20);
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ams api V1");
            });
            ;
        }
    }
}
namespace Swashbuckle.AspNetCore.Swagger
{
    public static class SwaggerExtensions
    {
        public static void IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var xmlFile = $"{types[i].Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);
            }
        }
    }
}