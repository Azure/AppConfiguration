using System;
using Newtonsoft.Json;

namespace SimpleAppConfigEventHub
{
    class EventContract
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }

        [JsonProperty("dataVersion")]
        public int DataVersion { get; set; }

        [JsonProperty("metadataVersion")]
        public int MetadataVersion { get; set; }
    }

    public class Data
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("etag")]
        public string ETag { get; set; }
    }
}
