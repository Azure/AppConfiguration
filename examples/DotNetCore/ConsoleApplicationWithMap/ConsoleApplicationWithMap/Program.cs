using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleApplication
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static IConfiguration Configuration { get; set; }
        static IConfigurationRefresher _refresher;

        static void Main(string[] args)
        {
            Configure();

            var cts = new CancellationTokenSource();
            _ = Run(cts.Token);

            // Finish on key press
            Console.ReadKey();
            cts.Cancel();
        }

        private static BlobServiceClient GetBlobServiceClient(string accountName)
        {
            BlobServiceClient client = new(
                new Uri($"https://{accountName}.blob.core.windows.net"),
                new DefaultAzureCredential());

            return client;
        }

        private static async Task<string> ListAllBlobsFlatListing(BlobServiceClient blobServiceClient)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                foreach (BlobContainerItem containerItem in blobServiceClient.GetBlobContainers())
                {
                    BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerItem.Name);

                    // Call the listing operation and return pages of the specified size.
                    var resultSegment = blobContainerClient.GetBlobsAsync()
                    .AsPages(default);

                    // Enumerate the blobs returned for each page.
                    await foreach (Page<BlobItem> blobPage in resultSegment)
                    {
                        foreach (BlobItem blobItem in blobPage.Values)
                        {
                            builder.AppendLine($"Blob name: {blobItem.Name}");
                        }

                        builder.AppendLine();
                    }
                }

                return builder.ToString();
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder();

            // Load a subset of the application's configuration from a json file and environment variables
            builder.AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();

            if (string.IsNullOrEmpty(configuration["ConnectionString"]))
            {
                Console.WriteLine("Connection string not found.");
                Console.WriteLine("Please set the 'ConnectionString' environment variable to a valid Azure App Configuration connection string and re-run this example.");
                return;
            }

            // Augment the configuration builder with Azure App Configuration
            // Pull the connection string from an environment variable
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(configuration["ConnectionString"])
                       .Select("*")
                       .ConfigureRefresh(refresh =>
                       {
                           refresh.Register("StorageAccountName")
                                  .SetCacheExpiration(TimeSpan.FromSeconds(10));
                       })
                        .Map(async (setting) =>
                        {
                            if (setting.ContentType.Equals("application/storageaccount"))
                            {
                                try
                                {
                                    BlobServiceClient blobServiceClient = GetBlobServiceClient(setting.Value);

                                    string blobList = await ListAllBlobsFlatListing(blobServiceClient);

                                    setting = new ConfigurationSetting(setting.Key, blobList, setting.Label, setting.ETag);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Error getting blob information: {e.Message}");
                                }
                            }

                             return setting;
                         });

                // Get an instance of the refresher that can be used to refresh data
                _refresher = options.GetRefresher();
            });

            Configuration = builder.Build();
        }

        private static async Task Run(CancellationToken token)
        {
            string display = string.Empty;
            StringBuilder sb = new StringBuilder();

            do
            {
                sb.Clear();

                // Trigger and wait for an async refresh for registered configuration settings
                await _refresher.TryRefreshAsync();

                sb.AppendLine($"{Configuration["StorageAccountName"]}");
                sb.AppendLine();

                sb.AppendLine("Press any key to exit...");

                display = sb.ToString();

                Console.Clear();
                Console.Write(display);

                await Task.Delay(1000);
            } while (!token.IsCancellationRequested);
        }
    }
}
