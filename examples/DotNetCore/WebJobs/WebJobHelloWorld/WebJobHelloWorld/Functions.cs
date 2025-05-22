// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace WebJobHelloWorld;

public class Functions
{
    public static void ProcessQueueMessage(
        [QueueTrigger("%QueueName%")] string message, // Get queue name from config
        ILogger logger)
    {
        //
        // Insert code to process the message here.
        // 

        logger.LogInformation(message);
    }
}
