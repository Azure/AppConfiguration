# Lock Key-Value - REST API Reference
#
**Provides lock/unlock semantics for the key-value resource.**

Supports the following operations:
- Place lock
- Remove lock

If present, ``label`` must be an explcit label value (**not** a wildcard). For all operations it's an optional parameter. If ommited it implies no label. 

#
#
*Prerequisites*: 
All HTTP requests must be authenticated. See the [authentication](./authentication.md) section.

#
#
## Lock Key-Value
#
**Required:** ``{key}``
*Optional:* ``label``
```
PUT /locks/{key}?label={label} HTTP/1.1
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8"
```

```sh
{
  "etag": "4f6dd610dd5e4deebc7fbaef685fb903",
  "key": "{key}",
  "label": "{label}",
  "content_type": null,
  "value": "example value",
  "created": "2017-12-05T02:41:26.4874615+00:00",
  "locked": true,
  "tags": []
}
```
#
#
**If the key-value doesn't exist**
```
HTTP/1.1 404 Not Found
```

#
#
#
## Unlock Key-Value
#
**Required:** ``{key}``
*Optional:* ``label``
```
DELETE /locks/{key}?label={label} HTTP/1.1
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8"
```

```sh
{
  "etag": "4f6dd610dd5e4deebc7fbaef685fb903",
  "key": "{key}",
  "label": "{label}",
  "content_type": null,
  "value": "example value",
  "created": "2017-12-05T02:41:26.4874615+00:00",
  "locked": true,
  "tags": []
}
```
#
#
**If the key-value doesn't exist**
```
HTTP/1.1 404 Not Found
```

#
#
# Conditional Lock/Unlock
To prevent race conditions, use ``If-Match`` or ``If-None-Match`` request headers. The ``etag`` argument is part of the key representation.
If ``If-Match`` or ``If-None-Match`` are omitted, the operation will be unconditional.

**Apply operation only if the current key-value representation matches the specified ``etag``**
```
PUT|DELETE /locks/{key}?label={label} HTTP/1.1
If-Match: "4f6dd610dd5e4deebc7fbaef685fb903"
```
**Apply operation only if the current key-value representation exists, but doesn't match the specified ``etag``**
```
PUT|DELETE /kv/{key}?label={label} HTTP/1.1
If-None-Match: "4f6dd610dd5e4deebc7fbaef685fb903"
```