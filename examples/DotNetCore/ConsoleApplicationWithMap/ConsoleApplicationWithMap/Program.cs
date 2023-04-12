using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleApplication
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureAppConfiguration;
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /*
        Example Blob Content
        -------------------------

        {
            "Data": [
                "Lorem",
                "ipsum",
                "dolor",
                "sit",
                "amet"
            ]
        }
    */

    class MyBlobContent
    {
        public IList<string> Data { get; set; }
    }

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

        private static async Task<MyBlobContent?> ReadBlobContentAsync(Uri blobUri)
        {
            var blobClient = new BlobClient(blobUri, new DefaultAzureCredential());

            // Read blob content
            var contentStream = new MemoryStream();
            await blobClient.DownloadToAsync(contentStream);

            // Go to the beginning of the content stream
            contentStream.Position = 0;

            // Convert content to json format
            var reader = new StreamReader(contentStream);
            var jsonReader = new JsonTextReader(reader);

            var serializer = new JsonSerializer();

            // Save the string values in the JSON array "Data" to MyBlobContent.Data
            return serializer.Deserialize<MyBlobContent>(jsonReader);
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
                            // key: BlobUri
                            // value: https://{account_name}.blob.core.windows.net/{container_name}/{blob_name}
                            refresh.Register("BlobUri")
                                    .SetCacheExpiration(TimeSpan.FromSeconds(10));
                        })
                        .Map(async (setting) =>
                        {
                            if (setting.ContentType.Equals("application/storage.blob"))
                            {
                                MyBlobContent? blobContent = await ReadBlobContentAsync(new Uri(setting.Value));
                                string newSettingValue = "";

                                if (blobContent != null)
                                {
                                    newSettingValue = $"[{string.Join(", ", blobContent.Data)}]";
                                }
                                else
                                {
                                    newSettingValue = "Error parsing blob content";
                                }

                                setting = new ConfigurationSetting(setting.Key, newSettingValue, setting.Label, setting.ETag);
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

                sb.AppendLine($"Blob content: {Configuration["BlobUri"]}");
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
