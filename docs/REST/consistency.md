# Real-time Consistency - REST API Reference
#
**Problem:**
Due to the nature of some distributed systems **real-time** consistency between requests can't (or it's very hard) to be enforced implicitly. A solution is to allow protocol support in the form of mutliple **Synchronization Tokens**. Synchronization tokens are optional.

**Objectives:**
To guarantee **real-time** consistency between different client instances and requests.

**Implementation:**

Uses optional ``Sync-Token`` request/response headers.

Syntax:
```
Sync-Token: <id>=<value>;sn=<sn>
```


|Parameter||
| -- | -- |
| ```<id>``` | Token ID (opaque) |
| ```<value>``` | Token value  (opaque). Allows base64 encoded string |
| ```<sn>``` | Token sequence number (version). Higher means newer version of the same token. Allows for better concurrency and client cachability. The client may choose to use only token's last version, since token versions are inclusive. Not required for requests. |

**Response:**

The service provides a ``Sync-Token`` header with each response. 

```
Sync-Token: jtqGc1I4=MDoyOA==;sn=28
```

**Request:**

Any subsequent request is guaranteed **real-time** consistent response in relation to the provided ``Sync-Token``.
```
Sync-Token: <id>=<value>
```

If the ``Sync-Token`` header is omitted from the request, then it's possible for the service to respond with cached data during a short period of time (up to a few seconds), before it settles internally. This may cause inconsistent reads if changes have occurred immediately before reading.


**Mutiple Sync-Token**

The server MAY respond with multiple sync-tokens for a single request. To keep **real-time** consistency for the next request, the client MUST respond with all of the received sync-tokens. Per RFC, multiple header values must be comma separated.
```
Sync-Token: <token1-id>=<value>,<token2-id>=<value>
```