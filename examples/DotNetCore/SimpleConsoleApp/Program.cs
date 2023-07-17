namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.SimpleConsoleApp
{
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args) {

            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));

            var config = builder.Build();
            Console.WriteLine(config["TestApp:Settings:Message"] ?? "Hello world!");
        }
    }
}


