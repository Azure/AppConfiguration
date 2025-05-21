// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;

namespace ChatApp
{
    internal class Message
    {
        [ConfigurationKeyName("role")]
        public required string Role { get; set; }

        [ConfigurationKeyName("content")]
        public string? Content { get; set; }
    }
}
