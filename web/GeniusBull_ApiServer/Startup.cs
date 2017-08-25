using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiServer
{
    class Startup
    {
        static string Url
        {
            get { return ConfigurationManager.AppSettings["url"] ?? "http://*:8001"; }
        }

        public static T InvokeApi<T>(string baseurl, string func_url, Func<string, T> result, string method = "GET")
        {
            string url = $"{baseurl}/{func_url}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            HttpWebResponse response = null;
            string response_text;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            try
            {
                using (response)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        response_text = sr.ReadToEnd();
                    if (response.StatusCode == HttpStatusCode.OK)
                        return result(response_text);
                }
            }
            catch { }
            return default(T);
        }



        static void Main(string[] args)
        {
            WebApp.Start(Url);
            Console.WriteLine($"ApiServer start at {Url}, type 'exit' or press Ctrl-C to exit...");
            Console.WriteLine($"Testing page : {Url}/Test.html");
            while (true)
            {
                string command = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrEmpty(command)) continue;
                else if (0 == string.Compare(command, "exit", true))
                    break;
            }
        }

        public void Configuration(IAppBuilder app)
        {
            try
            {
                HttpConfiguration config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                app.Use(async (context, next) =>
                {
                    Console.WriteLine($"{context.Request.Uri}");
                    await next();
                });
                app.UseWebApi(config);
                app.UseFileServer(new FileServerOptions
                {
                    EnableDirectoryBrowsing = true,
                    FileSystem = new PhysicalFileSystem(".\\wwwroot"),
                    //RequestPath = new PathString("/wwwroot"),
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}