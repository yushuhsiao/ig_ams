using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.Data;

namespace InnateGlory
{
    internal class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAMS();
            services.AddCookieAuth();

            services.AddActionContextAccessor();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .AddApiServices()
            .AddViewServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddSignalR(opts =>
            //{
            //});
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "ams api", Version = "v1" });
            //    c.IncludeXmlComments(typeof(Startup), typeof(JsonHelper), typeof(amsExtensions), typeof(Models.LoginModel));
            //});
            #region
            //services.Configure<RazorPagesOptions>(options =>
            //{
            //    options.Conventions
            //    .AuthorizeFolder("/")
            //    .AllowAnonymousToPage("/ApiList")
            //    .AllowAnonymousToPage("/Login")
            //    .AllowAnonymousToPage("/Main")
            //    ;
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            //app.UseSqlAppSettings();

            app.UseResponseCompression();
         
            //var asm = Assembly.GetEntryAssembly();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Use((context, next) =>
            //{
            //    return next();
            //});

            //app.UseWebpack(new WebpackOptions()
            //{
            //    EnableES2015 = false,
            //    EntryPoint = "app/index.js",
            //    OutputFileName = "js/app.js"
            //});


            //app.UseStaticFiles(opts =>
            //{
            //    var p = new FileExtensionContentTypeProvider();
            //    p.Mappings.Add(".less", "text/css");
            //    opts.ContentTypeProvider = p;
            //});

            //app.UseDirectoryBrowser();

            app.UsePageBundlesTagHelper();
            app.UseStaticFiles();
            //app.UseFileServer(new FileServerOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(env.ContentRootPath, "node_modules")),
            //    RequestPath = "/libx",
            //    EnableDirectoryBrowsing = true,
            //});
            //app.UseStaticFiles(opts =>
            //{
            //    opts.FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"));
            //    opts.RequestPath = "/libx";
            //});
            //app.UseFileServer(new FileServerOptions()
            //{
            //    // Set root of file server (remember not wwwroot!)
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
            //    // Only react to requests that match this path
            //    RequestPath = "/libx"
            //});


            app.UseAuthentication();
            //app.UseHttpsRedirection();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                ;
            });

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<Hub1>("/hub1", (HttpConnectionDispatcherOptions opts) =>
            //    {
            //        //opts.Transports = HttpTransportType.LongPolling;
            //        //opts.LongPolling.PollTimeout = TimeSpan.FromSeconds(20);
            //    });
            //});

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ams api V1");
            });
            ;
            //app.UseBlazor(new BlazorOptions() { ClientAssemblyPath = Blazor_Path });


            var cn = ConfigurationBinder.GetValue<DbConnectionString>(app.ApplicationServices.GetService<IConfiguration>(), "ConnectionStrings:CoreDB_R");
            ;
            using (var conn = cn.OpenDbConnection(app.ApplicationServices, null))
            {
            }
        }

        //[AppSetting(SectionName = "Blazor", Key = "ClientPath")]
        //public string Blazor_Path => Configuration.GetValue<amsStartup, string>();
    }
}

/*
.AddAMS(actionSelectorOptions: options =>
{
    //options.SelectCandidate = (context, action) =>
    //{
    //    bool result = true;
    //    if (action.RelativePath.IsEquals("/Pages/Home/Main.cshtml"))
    //        result = !context.HttpContext.RequestServices.GetCurrentUser().Id.IsGuest;
    //    else if (action.RelativePath.IsEquals("/Pages/Home/Login.cshtml"))
    //        result = context.HttpContext.RequestServices.GetCurrentUser().Id.IsGuest;
    //    return result;
    //};
})
*/
