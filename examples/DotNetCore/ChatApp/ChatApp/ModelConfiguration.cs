// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;

namespace ChatApp
{
    internal class ModelConfiguration
    {
        [ConfigurationKeyName("model")]
        public string? Model { get; set; }

        [ConfigurationKeyName("messages")]
        public List<Message>? Messages { get; set; }

        [ConfigurationKeyName("max_tokens")]
        public int MaxTokens { get; set; }

        [ConfigurationKeyName("temperature")]
        public float Temperature { get; set; }

        [ConfigurationKeyName("top_p")]
        public float TopP { get; set; }
    }
}
