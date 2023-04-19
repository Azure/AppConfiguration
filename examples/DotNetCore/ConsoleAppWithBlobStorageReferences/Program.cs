using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Storage.Blobs;
using System.Text;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleAppWithBlobStorageReferences
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

        static void Main(string[] args)
        {
            Configure();

            string display = string.Empty;
            StringBuilder sb = new StringBuilder();

            var blobContent = new MyBlobContent();
            Configuration.GetSection("BlobUri").Bind(blobContent);

            sb.AppendLine($"Blob content: \n{string.Join(", ", blobContent.Data)}");
            sb.AppendLine();

            sb.AppendLine("Press any key to exit...");

            display = sb.ToString();

            Console.Clear();
            Console.Write(display);            

            // Finish on key press
            Console.ReadKey();
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
            });

            Configuration = builder.Build();
        }
    }
}
