using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleApplication
{
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

        private static async Task<string> ReadBlobContentAsync(Uri blobUri)
        {
            var blobClient = new BlobClient(blobUri, new DefaultAzureCredential());

            // Read blob content
            using (var contentStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(contentStream);

                // Go to the beginning of the content stream
                contentStream.Position = 0;

                // Convert content to string
                using (var reader = new StreamReader(contentStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder();

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
                        // key: BlobUri
                        // value: https://{account_name}.blob.core.windows.net/{container_name}/{blob_name}
                        .Select("BlobUri")
                        .ConfigureRefresh(refresh =>
                        {
                            // Changes to BlobSentinel will refresh all key-values, including BlobUri
                            refresh.Register("BlobSentinel", true);
                        })
                        .Map(async (setting) =>
                        {
                            if (setting.ContentType.Equals("application/storage.blob"))
                            {
                                string blobContent = await ReadBlobContentAsync(new Uri(setting.Value));

                                setting = new ConfigurationSetting(setting.Key, blobContent, setting.Label);
                                setting.ContentType = "application/json";
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

                IEnumerable<string?> blobData = Configuration.GetSection("BlobUri:Data").AsEnumerable().Where(kv => !string.IsNullOrEmpty(kv.Value)).Select(kv => $"{kv.Key} = {kv.Value}");

                sb.AppendLine($"Blob content: \n{string.Join("\n", blobData)}");
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
