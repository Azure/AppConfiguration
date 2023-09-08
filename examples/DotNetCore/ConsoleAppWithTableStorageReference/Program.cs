using Azure.Data.AppConfiguration;
using Azure.Data.Tables;
using Azure.Identity;
using System.Text;
using System.Text.Json;

namespace Microsoft.Extensions.Configuration.AzureAppConfiguration.Examples.ConsoleAppWithTableStorageReference
{
    class Program
    {
        static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Configure();

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
                        .Map((setting) =>
                        {
                            if (setting.ContentType.Equals("application/x.example.table.product"))
                            {
                                // Example value for key "MyShop:Inventory" in App Configuration: https://{account_name}.table.core.windows.net/{table_name}
                                IEnumerable<Product> products = ReadProducts(new Uri(setting.Value));

                                setting = new ConfigurationSetting(setting.Key, JsonSerializer.Serialize(products.ToList()), setting.Label);

                                setting.ContentType = "application/json";
                            }

                            return new ValueTask<ConfigurationSetting>(setting);
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
            List<string> columnsToDisplay = new List<string>();

            Configuration.GetSection("MyShop:DisplayedColumns").Bind(columnsToDisplay);

            sb.AppendLine("Product table:");

            sb.AppendLine();

            foreach (Product product in tableContent)
            {
                if (columnsToDisplay.Contains(nameof(product.Name)))
                {
                    sb.AppendLine($"Name = {product.Name}");
                }

                if (columnsToDisplay.Contains(nameof(product.Quantity)))
                {
                    sb.AppendLine($"Quantity = {product.Quantity.ToString()}");
                }

                if (columnsToDisplay.Contains(nameof(product.OnSale)))
                {
                    sb.AppendLine($"OnSale = {product.OnSale.ToString()}");
                }

                sb.AppendLine();
            }

            return sb;
        }

        private static IEnumerable<Product> ReadProducts(Uri tableUri)
        {
            string[] pathSegments = tableUri.Segments;

            // The last segment in the path should be the table name
            string tableName = pathSegments[pathSegments.Length - 1];

            var tableClient = new TableClient(tableUri, tableName, new DefaultAzureCredential());

            return tableClient.Query<Product>();
        }
    }
}
