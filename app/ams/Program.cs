using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace InnateGlory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ConsoleLogWriter.Enabled = true;
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddSqlAppSettings();
            })
            //.ConfigureLogging(logging => logging.AddTextFile())
            //.UseKestrel(opts => opts.BindConfiguration())
            .UseStartup<Startup>();
    }
}
