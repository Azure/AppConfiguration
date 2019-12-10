# Key-Value - REST API Reference
api-version: 1.0
#
**Identity:**

Key-Value is a resource identified by unique combination of ``key`` + ``label``. 
``label`` is optional. To explicitly reference a key-value without a label use "\0" (url encoded as ``%00``). See details for each operation.

**Operations:**
- Get
- List multiple
- Set 
- Delete

#
#
**Prerequisites:** 
- All HTTP requests must be authenticated. See the [authentication](./authentication/index.md) section.
- All HTTP requests must provide explicit ``api-version``. See the [versioning](./versioning.md) section.

#
#
## Syntax
#

```sh
{
  "etag": [string]
  "key": [string]
  "label": [string, optional]
  "content_type": [string, optional]
  "value": [string]
  "last_modified": [datetime ISO 8601]
  "locked": [boolean]
  "tags": [object with string properties, optional]
}
```


#
#
## Get Key-Value
#
**Required:** ``{key}``, ``{api-version}``  
*Optional:* ``label`` - If ommited it implies a key-value without a label
```
GET /kv/{key}?label={label}&api-version={api-version}
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8;
Last-Modified: Tue, 05 Dec 2017 02:41:26 GMT
ETag: "4f6dd610dd5e4deebc7fbaef685fb903"
```

```sh
{
  "etag": "4f6dd610dd5e4deebc7fbaef685fb903",
  "key": "{key}",
  "label": "{label}",
  "content_type": null,
  "value": "example value",
  "last_modified": "2017-12-05T02:41:26+00:00",
  "locked": "false",
  "tags": {
    "t1": "value1",
    "t2": "value2"
  }
}
```
#
#
**If it doesn't exist**
```
HTTP/1.1 404 Not Found
```

#
#
## Get (Conditionally)
To improve client caching, use ``If-Match`` or ``If-None-Match`` request headers. The ``etag`` argument is part of the key representation. See [Sec 14.24](https://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html)

**Get only if the current representation doesn't match the specified ``etag``**
```
GET /kv/{key}?api-version={api-version} HTTP/1.1
Accept: application/vnd.microsoft.appconfig.kv+json;
If-None-Match: "{etag}"
```

**Responses:**
```
HTTP/1.1 304 NotModified
```
or
```
HTTP/1.1 200 OK
```

#
#
#
## List Key-Values
See **Filtering** for additional options
#
*Optional:* ``key`` - if not specified it implies **any** key.
*Optional:* ``label`` - if not specified it implies **any** label.

```
GET /kv?label=*&api-version={api-version} HTTP/1.1
```

**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kvset+json; charset=utf-8
```

#
#
#
## Pagination
#
The result is paginated if the number of items returned exceeds the response limit. Follow the optional ``Link`` response headers and use ``rel="next"`` for navigation. 
Alternatively the content provides a next link in form of the ``@nextLink`` property. The linked uri includes ``api-version`` argument.
```
GET /kv?api-version={api-version} HTTP/1.1
```
**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kvs+json; charset=utf-8
Link: <{relative uri}>; rel="next"
```
```
{
    "items": [
        ...
    ],
    "@nextLink": "{relative uri}"
}
```


#
#
#
## Filtering
#
A combination of ```key``` and ```label``` filtering is supported. 
Use the optional ```key``` and ```label``` query string parameters. 

```
GET /kv?key={key}&label={label}&api-version={api-version}
```

**Supported filters**

|Key||
|--|--|
|```key``` is omitted or ```key=*```|Matches **any** key|
|```key=abc```|Matches a key named **abc**|
|```key=abc*```|Matches keys names that start with **abc**|
|```key=*abc```|Matches keys names that end with **abc**|
|```key=*abc*```|Matches keys names that contain **abc**|
|```key=abc,xyz```|Matches keys names **abc** or **xyz** (limited to 5 CSV)|

|Label||
|--|--|
|```label``` is omitted or ```label=*```|Matches **any** label|
|```label=%00```|Matches KV without label|
|```label=prod```|Matches the label **prod**|
|```label=prod*```|Matches labels that start with **prod**|
|```label=*prod```|Matches labels that end with **prod**|
|```label=*prod*```|Matches labels that contain **prod**|
|```label=prod,test```|Matches labels **prod** or **test** (limited to 5 CSV)|

***Reserved characters***

```*```, ```\```, ```,```

If a reserved character is part of the value, then it must be escaped using ```\{Reserved Character}```. Non-reserved characters can also be escaped.


***Filter Validation***

In the case of a filter validation error, the response is HTTP ```400``` with error details:

```
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json; charset=utf-8
```
```sh 
{
  "type": "https://azconfig.io/errors/invalid-argument",
  "title": "Invalid request parameter '{filter}'",
  "name": "{filter}",
  "detail": "{filter}(2): Invalid character",
  "status": 400
}
```

**Examples**

- All
```
GET /kv?api-version={api-version}
```

- Key name starts with **abc** and include all labels
```
GET /kv?key=*abc&label=*&api-version={api-version}
```

- Key name is either **abc** or **xyz** and include all labels that contain **prod**
```
GET /kv?key=abc,xyz&label=*prod*&api-version={api-version}
```

#
#
#
## Request specific fields
#
Use the optional ``$select`` query string parameter and provide comma separated list of requested fields. If the ``$select`` parameter is ommited, the response contains the default set.
```
GET /kv?$select=key,value&api-version={api-version} HTTP/1.1
```

#
#
#
## Time-Based Access
#
Obtain a representation of the result as it was at a past time. See section [2.1.1](https://tools.ietf.org/html/rfc7089#section-2.1). Pagination is still supported as defined above.
```
GET /kv?api-version={api-version} HTTP/1.1
Accept-Datetime: Sat, 12 May 2018 02:10:00 GMT
```

**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kvset+json"
Memento-Datetime: Sat, 12 May 2018 02:10:00 GMT
Link: <{relative uri}>; rel="original"
```
```
{
    "items": [
        ....
    ]
}
```

#
#
#
## Set Key
#
**Required:** ``{key}`` 
*Optional:* ``label`` - if not specified or label=%00 it implies KV without label.
```
PUT /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
Content-Type: application/vnd.microsoft.appconfig.kv+json
```
```sh
{
  "value": "example value",         // optional
  "content_type": "user defined",   // optional
  "tags": {                         // optional
    "tag1": "value1",
    "tag2": "value2",
  }
}
```

**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8
Last-Modified: Tue, 05 Dec 2017 02:41:26 GMT
ETag: "4f6dd610dd5e4deebc7fbaef685fb903"
```
```sh
{
  "etag": "4f6dd610dd5e4deebc7fbaef685fb903",
  "key": "{key}",
  "label": "{label}",
  "content_type": "user defined",
  "value": "example value",
  "last_modified": "2017-12-05T02:41:26.4874615+00:00",
  "tags": {
    "tag1": "value1",
    "tag2": "value2",
  }
}
```
#
#
**If the item is locked**
```
HTTP/1.1 409 Conflict
Content-Type: application/problem+json; charset="utf-8"
```
```
{
	"type": "https://azconfig.io/errors/key-locked"
	"title": "Modifing key '{key}' is not allowed",
	"name": "{key}",
	"detail": "The key is read-only. To allow modification unlock it first.",
	"status": "409"
}
```

#
#
# Set Key (Conditionally)
To prevent race conditions, use ``If-Match`` or ``If-None-Match`` request headers. The ``etag`` argument is part of the key representation.
If ``If-Match`` or ``If-None-Match`` are omitted, the operation will be unconditional.

**Update only if the current representation matches the specified ``etag``**
```
PUT /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
Content-Type: application/vnd.microsoft.appconfig.kv+json
If-Match: "4f6dd610dd5e4deebc7fbaef685fb903"
```
**Update only if the current representation doesn't match the specified ``etag``**
```
PUT /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
Content-Type: application/vnd.microsoft.appconfig.kv+json;
If-None-Match: "4f6dd610dd5e4deebc7fbaef685fb903"
```
**Update if any representation exist**
```
PUT /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
Content-Type: application/vnd.microsoft.appconfig.kv+json;
If-Match: "*"
```
**Add only if representation doesn't exist**
```
PUT /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
Content-Type: application/vnd.microsoft.appconfig.kv+json
If-None-Match: "*"
```

#
#
**Responses**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8
...
```
or
```
HTTP/1.1 412 PreconditionFailed
```

#
#
#
# Delete
#
**Required:** ``{key}``, ``{api-version}``  
*Optional:* ``{label}`` - if not specified or label=%00 it implies KV without label.
```
DELETE /kv/{key}?label={label}&api-version={api-version} HTTP/1.1
```

**Response:**
Return the deleted key-value or none if didn't exist.
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kv+json; charset=utf-8
...
```
or
```
HTTP/1.1 204 No Content
```

#
#
# Delete Key (Conditionally)
Similar to **Set Key (Conditionally)**
