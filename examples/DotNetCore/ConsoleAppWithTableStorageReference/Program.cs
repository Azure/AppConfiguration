using Azure;
using Azure.Data.AppConfiguration;
using Azure.Data.Tables;
using Azure.Identity;
using System.Text;
using System.Text.Json;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleAppWithTableStorageReference
{
    // C# record type for items in the table
    public record Product : ITableEntity
    {
        public string RowKey { get; set; } = default!;

        public string PartitionKey { get; set; } = default!;

        public string Name { get; init; } = default!;

        public int Quantity { get; init; }

        public bool OnSale { get; init; }

        public ETag ETag { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

    class Program
    {
        static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configure();

            string display = string.Empty;

            StringBuilder sb = BuildProductTable();

            sb.AppendLine();
            sb.AppendLine("Press any key to exit...");

            Console.Clear();
            Console.Write(sb.ToString());            

            // Finish on key press
            Console.ReadKey();
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder();

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
                        .Map(async (setting) =>
                        {
                            if (setting.ContentType.Equals("application/storage.table"))
                            {
                                // Example value for key "MyShop:Inventory" in App Configuration: https://{account_name}.table.core.windows.net/{table_name}
                                string tableContent = await ReadTableContentAsync(new Uri(setting.Value));

                                setting = new ConfigurationSetting(setting.Key, tableContent, setting.Label);
                                setting.ContentType = "application/json";
                            }

                            return setting;
                        });
            });

            Configuration = builder.Build();
        }

        private static StringBuilder BuildProductTable()
        {
            StringBuilder sb = new StringBuilder();

            List<Product> tableContent = new List<Product>();
            Configuration.GetSection("MyShop:Inventory").Bind(tableContent);

            // Example value for "MyShop:DisplayedColumns": [Name, OnSale]
            // Content-Type: application/json
            List<string> columnsToDisplay = Configuration.GetValue<List<string>>("MyShop:DisplayedColumns");
            List<string> allowedProductFields = new List<string> { "Name", "Quantity", "OnSale" };

            if (columnsToDisplay == null)
            {
                columnsToDisplay = allowedProductFields;
            }

            sb.AppendLine("Your shop inventory:\n");
            var tableFormat = string.Empty;

            for (int i = 0; i < columnsToDisplay.Count(); i++)
            {
                tableFormat += "{" + i + ", -20}";
            }

            sb.AppendFormat(tableFormat, columnsToDisplay.ToArray());
            sb.AppendLine();

            foreach (Product product in tableContent)
            {
                List<string> productValues = new List<string> { product.Name, product.Quantity.ToString(), product.OnSale.ToString() };
                List<string> fieldsToPrint = new List<string>(new string[columnsToDisplay.Count()]);
                int fieldsToPrintIndex = 0;

                foreach (string field in allowedProductFields)
                {
                    if (columnsToDisplay.Contains(field))
                    {
                        fieldsToPrint[fieldsToPrintIndex] = productValues[columnsToDisplay.IndexOf(field)];
                        fieldsToPrintIndex++;
                    }
                }

                sb.AppendFormat(tableFormat, fieldsToPrint.ToArray());
                sb.AppendLine();
            }

            return sb;
        }

        private static async Task<string> ReadTableContentAsync(Uri tableUri)
        {
            string[] pathSegments = tableUri.Segments;

            // The last segment in the path should be the table name
            string tableName = pathSegments[pathSegments.Length - 1];

            var tableClient = new TableClient(tableUri, tableName, new DefaultAzureCredential());

            // Adding example products to display
            var prod1 = new Product()
            {
                RowKey = "68719518388",
                PartitionKey = "gear-surf-surfboards",
                Name = "Ocean Surfboard",
                Quantity = 8,
                OnSale = true
            };

            var prod2 = new Product()
            {
                RowKey = "68719518390",
                PartitionKey = "gear-surf-surfboards",
                Name = "Sand Surfboard",
                Quantity = 5,
                OnSale = false
            };

            await tableClient.UpsertEntityAsync(prod1);
            await tableClient.UpsertEntityAsync(prod2);

            var products = tableClient.Query<Product>();

            return JsonSerializer.Serialize(products.ToList());
        }
    }
}
