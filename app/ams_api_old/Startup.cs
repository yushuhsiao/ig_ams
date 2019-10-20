using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace InnateGlory
{
    internal class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAMS();
            //services.AddCookieAuth();
            services.AddApiAuth();
            services.AddActionContextAccessor();
            services.AddMvc(options =>
            {

            })
            .AddApiServices()

            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR(opts =>
            {
            });
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    //.WithOrigins("")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    ;
                });
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
            app.UseCors("CorsPolicy"); // global enable cors
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
