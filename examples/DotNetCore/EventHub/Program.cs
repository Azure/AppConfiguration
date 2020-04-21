
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SimpleAppConfigEventHub
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
                {
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {
                        var settings = builder.Build();
                        var appConfigConnectionString = settings["EventHubConnection:AppConfigConnectionString"];
                        if (!string.IsNullOrEmpty(appConfigConnectionString))
                        {
                            builder.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(appConfigConnectionString)
                                       .Select(keyFilter: "Demo:Settings:*");
                            });
                        }
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
