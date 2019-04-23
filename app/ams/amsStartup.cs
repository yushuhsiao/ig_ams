﻿using InnateGlory.TagHelpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
//using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
//using Webpack;
//[assembly: System.Reflection.AssemblyTitle("amsxx")]


namespace InnateGlory
{
    public class amsStartup
    {
        public static void Main(string[] args)
        {
            try
            {
                //ConsoleLogWriter.Enabled = true;
                WebHost.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(config =>
                    {
                        config.AddSqlAppSettings();
                    })
                    //.ConfigureLogging(logging => logging.AddTextFile())
                    //.UseKestrel(opts => opts.BindConfiguration())
                    .UseStartup<amsStartup>()
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        //public amsStartup(IConfiguration configuration)
        //{
        //    Configuration = configuration;

        //    //var x = configuration.GetSection("Authentication").GetValue<bool>("InternalApiServer");
        //}

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddWebpack();
            services.AddUserManager<amsUser>();
            services.AddAMS();
            services.AddMvc().AddAMS(actionSelectorOptions: options =>
            {
                options.SelectCandidate = (context, action) =>
                {
                    bool result = true;
                    if (action.RelativePath.IsEquals("/Pages/Home/Index.cshtml"))
                        result = !context.HttpContext.RequestServices.GetCurrentUser().Id.IsGuest;
                    else if (action.RelativePath.IsEquals("/Pages/Home/Login.cshtml"))
                        result = context.HttpContext.RequestServices.GetCurrentUser().Id.IsGuest;
                    return result;
                };
            }).AddRazorPagesOptions(opts =>
            {
                //opts.Conventions.AuthorizeFolder("/").AllowAnonymousToPage("/Login.cshtml");
            });
            services.AddSignalR(opts=>
            {
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddPageBundlesTagHelper();
            services.AddResponseCompression();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ams api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            #region
            //services.Configure<RazorPagesOptions>(options =>
            //{
            //    options.Conventions
            //    .AuthorizeFolder("/")
            //    .AllowAnonymousToPage("/ApiList")
            //    .AllowAnonymousToPage("/Login")
            //    .AllowAnonymousToPage("/Index")
            //    ;
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            app.UseSqlAppSettings();

            app.UseResponseCompression();
         
            //var asm = Assembly.GetEntryAssembly();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.UseStaticFiles(opts =>
            {
                opts.FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"));
                opts.RequestPath = "/libx";
            });
            //app.UseFileServer(new FileServerOptions()
            //{
            //    // Set root of file server (remember not wwwroot!)
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
            //    // Only react to requests that match this path
            //    RequestPath = "/libx"
            //});


            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //       name: "default",
                //       template: "{controller}/{action=Index}/{id?}");
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
            //app.UseBlazor(new BlazorOptions() { ClientAssemblyPath = Blazor_Path });
        }

        //[AppSetting(SectionName = "Blazor", Key = "ClientPath")]
        //public string Blazor_Path => Configuration.GetValue<amsStartup, string>();
    }

    //class _ServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    //{
    //    private DefaultServiceProviderFactory _inner;

    //    public _ServiceProviderFactory()
    //    {
    //        _inner = new DefaultServiceProviderFactory();
    //    }
    //    public _ServiceProviderFactory(ServiceProviderOptions options)
    //    {
    //        _inner = new DefaultServiceProviderFactory(options);
    //    }

    //    IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
    //    {
    //        return _inner.CreateBuilder(services);
    //    }

    //    IServiceProvider IServiceProviderFactory<IServiceCollection>.CreateServiceProvider(IServiceCollection containerBuilder)
    //    {
    //        return _inner.CreateServiceProvider(containerBuilder);
    //    }
    //}

    //class _MvcJsonMvcOptionsSetup : IConfigureOptions<MvcOptions>
    //{
    //    private readonly IServiceProvider _provider;
    //    private readonly ILoggerFactory _loggerFactory;
    //    private readonly JsonSerializerSettings _jsonSerializerSettings;
    //    private readonly ArrayPool<char> _charPool;
    //    private readonly ObjectPoolProvider _objectPoolProvider;

    //    public _MvcJsonMvcOptionsSetup(
    //        IServiceProvider provider,
    //        ILoggerFactory loggerFactory,
    //        IOptions<MvcJsonOptions> jsonOptions,
    //        ArrayPool<char> charPool,
    //        ObjectPoolProvider objectPoolProvider)
    //    {
    //        if (loggerFactory == null)
    //        {
    //            throw new ArgumentNullException(nameof(loggerFactory));
    //        }

    //        if (jsonOptions == null)
    //        {
    //            throw new ArgumentNullException(nameof(jsonOptions));
    //        }

    //        if (charPool == null)
    //        {
    //            throw new ArgumentNullException(nameof(charPool));
    //        }

    //        if (objectPoolProvider == null)
    //        {
    //            throw new ArgumentNullException(nameof(objectPoolProvider));
    //        }

    //        _provider = provider;
    //        _loggerFactory = loggerFactory;
    //        _jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
    //        _charPool = charPool;
    //        _objectPoolProvider = objectPoolProvider;
    //    }

    //    public void Configure(MvcOptions options)
    //    {
    //        for (int i = 0; i < options.OutputFormatters.Count; i++)
    //        {
    //            var f = options.OutputFormatters[i];
    //            if (f == null)
    //                continue;
    //            else if (f is HttpNoContentOutputFormatter)
    //            {
    //            }
    //            else if (f is StringOutputFormatter)
    //            {
    //            }
    //            else if (f is StreamOutputFormatter)
    //            {
    //            }
    //            else if (f is JsonOutputFormatter)
    //            {
    //            }
    //            else
    //            {
    //            }
    //        }
    //        for (int i = 0; i < options.InputFormatters.Count; i++)
    //        {
    //            var f = options.InputFormatters[i];
    //            if (f == null) continue;
    //            else if (f is JsonPatchInputFormatter)
    //            {
    //            }
    //            else if (f is JsonInputFormatter)
    //            {
    //            }
    //            else
    //            {
    //            }
    //        }

    //        //options.OutputFormatters.Add(new JsonOutputFormatter(_jsonSerializerSettings, _charPool));

    //        //// Register JsonPatchInputFormatter before JsonInputFormatter, otherwise
    //        //// JsonInputFormatter would consume "application/json-patch+json" requests
    //        //// before JsonPatchInputFormatter gets to see them.
    //        //var jsonInputPatchLogger = _loggerFactory.CreateLogger<JsonPatchInputFormatter>();
    //        //options.InputFormatters.Add(new JsonPatchInputFormatter(
    //        //    jsonInputPatchLogger,
    //        //    _jsonSerializerSettings,
    //        //    _charPool,
    //        //    _objectPoolProvider,
    //        //    options.SuppressInputFormatterBuffering));

    //        //var jsonInputLogger = _loggerFactory.CreateLogger<JsonInputFormatter>();
    //        //options.InputFormatters.Add(new JsonInputFormatter(
    //        //    jsonInputLogger,
    //        //    _jsonSerializerSettings,
    //        //    _charPool,
    //        //    _objectPoolProvider,
    //        //    options.SuppressInputFormatterBuffering));

    //        //options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

    //        //options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(IJsonPatchDocument)));
    //        //options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(JToken)));
    //    }
    //}
}