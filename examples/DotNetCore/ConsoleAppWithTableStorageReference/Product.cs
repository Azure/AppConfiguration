using Azure.Data.Tables;
using Azure;

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
}
