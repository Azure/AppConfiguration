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

            IEnumerable<Product> products = Configuration.GetSection("MyShop:Inventory").Get<List<Product>>();

            Console.Clear();

            Console.WriteLine(FormatProducts(products));

            Console.WriteLine(Environment.NewLine + "Press any key to exit...");

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
                            switch (setting.ContentType)
                            {
                                case "application/x.example.tablereference.product":

                                    return await MapProductTableReference(setting).ConfigureAwait(false);

                                default:

                                    return setting;
                            }
                        });
            });

            Configuration = builder.Build();
        }

        private static string FormatProducts(List<Product> products)
        {
            StringBuilder sb = new StringBuilder();

            List<string> columnsToDisplay = Configuration.GetSection("MyShop:DisplayedColumns").Get<List<string>>();

            sb.AppendLine("Product table:");

            sb.AppendLine();

            foreach (Product product in products)
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

            return sb.ToString();
        }

        private static async ValueTask<ConfigurationSetting> MapProductTableReference(ConfigurationSetting setting)
        {
            var tableUri = new Uri(setting.Value);

            string[] pathSegments = tableUri.Segments;

            // The last segment in the path should be the table name
            string tableName = pathSegments[pathSegments.Length - 1];

            var tableClient = new TableClient(tableUri, tableName, new DefaultAzureCredential());

            var products = new List<Product>();

            await foreach (var product in tableClient.QueryAsync<Product>().ConfigureAwait(false))
            {
                products.Add(product);
            }

            setting = new ConfigurationSetting(setting.Key, JsonSerializer.Serialize(products), setting.Label);

            setting.ContentType = "application/json";

            return setting;
        }
    }
}
