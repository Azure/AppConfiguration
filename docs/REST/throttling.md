# Throttling - REST API Reference
#
Configuration stores have limits on the requests that they may serve. Any requests that exceeds an allotted quota for a configuration store will receive an HTTP 429 (Too Many Requests) response.

Throttling is divided into different quota policies:

**TotalRequests** - total amount of requests/sec
		
**WriteRequests** - total amount of write requests/second

**Bandwidth** - outbound data in bytes

**Storage** - total storage size of user data in bytes


## Handling Throttled Responses

When the rate limit for a given request type has been reached, the configuration store will respond to further requests of that type with a _429_ status code. The _429_ response will contain a _retry-after-ms_ header providing the client with a suggested wait time (in milliseconds) to allow the request quota to replinish.

```
HTTP/1.1 429 (Too Many Requests)
retry-after-ms: 10
Content-Type: application/problem+json; charset=utf-8
```
```sh
{
  "type": "https://azconfig.io/errors/too-many-requests",
  "title": "Resource utilization has surpassed the assigned quota",
  "policy": "TotalRequests" | "WriteRequests" | "Bandwidth" | "Storage",
  "status": 429
}
```

In the above example, the client has exceeded its allowed requests per second and is advised to slow down and wait 10 milliseconds before attempting any further requests. Clients should consider progressive backoff as well.


# Other Retry
#
The service may identify situations other than throttling that need a client retry (ex: 503 Service Unavailable). 
In all such cases, the ``retry-after-ms`` response header will be provided. To increase robustness, the client is advised to follow the suggested interval and perform a retry.

```
HTTP/1.1 503 Service Unavailable
retry-after-ms: 787
```