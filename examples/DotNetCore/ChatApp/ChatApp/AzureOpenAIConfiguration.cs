// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

namespace ChatApp
{
    internal class AzureOpenAIConfiguration
    {
        public required string Endpoint { get; set; }

        public required string DeploymentName { get; set; }
        
        public string? ApiKey { get; set; }
    }
}
