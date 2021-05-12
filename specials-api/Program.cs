using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;

namespace specials_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
            webBuilder.ConfigureAppConfiguration(config =>
            {
                var settings = config.Build();

                // Only use AppConfig in environment not Development
                if (!String.Equals(settings.GetValue<string>("ASPNETCORE_ENVIRONMENT"), "Development", StringComparison.CurrentCultureIgnoreCase))
                {
                    var connection = settings.GetConnectionString("AppConfig");

                    // https://docs.microsoft.com/azure/azure-app-configuration/quickstart-aspnet-core-app?tabs=core5x
                    config.AddAzureAppConfiguration(connection);
                }
            }).UseStartup<Startup>());
    }
}
