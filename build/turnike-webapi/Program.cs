using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace turnike_webapi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Init();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls($"{Config.GetValue<string>("Host:IP")}:{Config.GetValue<ushort>("Host:Port")}")
                    .UseStartup<Startup>();
                });
    }


    public partial class Program
    {
        private static void Init()
        {
            Config = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            string configPath = "ConnectionString:Value";
            SqlConnectionString = Config.GetValue<string>(configPath);

        }

        public static IConfigurationRoot Config { get; private set; }
        internal static string SqlConnectionString { get; private set; }

    }
}
