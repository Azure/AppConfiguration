# Key-Value Revisions - REST API Reference
api-version: 1.0
#
**Defines chronological/historical representation of key-value resource(s).**
Revisions eventually expire (default 7 days)

Supports the following operations:
- List

For all operations ``key`` is an optional parameter. If ommited it implies **any** key.
For all operations ``label`` is an optional parameter. If ommited it implies **any** label.

#
#
**Prerequisites**: 
- All HTTP requests must be authenticated. See the [authentication](./authentication.md) section.
- All HTTP requests must provide explicit ``api-version``. See the [versioning](./versioning.md) section.

#
#
## List Revisions
#
```
GET /revisions?label=*&api-version={api-version} HTTP/1.1
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.kvset+json; charset=utf-8"
Accept-Ranges: items
```

```sh
{
    "items": [
        {
          "etag": "4f6dd610dd5e4deebc7fbaef685fb903",
          "key": "{key}",
          "label": "{label}",
          "content_type": null,
          "value": "example value",
          "last_modified": "2017-12-05T02:41:26.4874615+00:00",
          "tags": []
        },
        ...
    ],
    "@nextLink": "{relative uri}"
}
```

#
#
#
## Pagination
#
The result is paginated if the number of items returned exceeds the response limit. Follow the optional ``Link`` response header and use ``rel="next"`` for navigation. 
Alternatively the content provides a next link in form of the ``@nextLink`` property.
```
GET /revisions?api-version={api-version} HTTP/1.1
```
**Response:**
```
HTTP/1.1 OK
Content-Type: application/vnd.microsoft.appconfig.kvs+json; charset=utf-8
Accept-Ranges: items
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
## List subset of revisions
#
Use ``Range`` request header. The response will contain ``Content-Range`` header. 
If the server can't satisfy the requested range it will respond with HTTP ``416`` (RangeNotSatisfiable)
```
GET /revisions?api-version={api-version} HTTP/1.1
Range: items=0-2
```
**Response**
```
HTTP/1.1 206 Partial Content
Content-Type: application/vnd.microsoft.appconfig.revs+json; charset=utf-8
Content-Range: items 0-2/80
```

#
#
#
## Filtering
#
A combination of ```key``` and ```label``` filtering is supported. 
Use the optional ```key``` and ```label``` query string parameters. 

```
GET /revisions?key={key}&label={label}&api-version={api-version}
```

**Supported filters**

|Key||
|--|--|
|```key``` is omitted or ```key=*```|Matches **any** key|
|```key=abc```|Matches a key named  **abc**|
|```key=abc*```|Matches keys names that start with **abc**|
|```key=*abc```|Matches keys names that end with **abc**|
|```key=*abc*```|Matches keys names that contain **abc**|
|```key=abc,xyz```|Matche keys names **abc** or **xyz** (limited to 5 CSV)|

|Label||
|--|--|
|```label``` is omitted or ```label=```|Matches entry without label|
|```label=*```|Matches **any** label|
|```label=prod```|Matches the label **prod**|
|```label=prod*```|Matches labels that start with **prod**|
|```label=*prod```|Matches labels that end with **prod**|
|```label=*prod*```|Matches labels that contain **prod**|
|```label=prod,test```|Matches labels **prod** or **test** (limited to 5 CSV)|


***Reserved characters***

```*```, ```\```, ```,```

If a reserved character is part of the value, then it must be escaped using ```\{Reserved Character}```. Non-reserved characters can also be escaped.


***Filter Validation***

In case of a filter validation error, the response is HTTP ```400``` with error details:

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
GET /revisions
```

- Items where key name starts with **abc**
```
GET /revisions?key=abc*&api-version={api-version}
```

- Items where key name is either **abc** or **xyz** and labels contain **prod**
```
GET /revisions?key=abc,xyz&label=*prod*&api-version={api-version}
```

#
#
#
## Request specific fields
#
Use the optional ``$select`` query string parameter and provide comma separated list of requested fields. If the ``$select`` parameter is ommited, the response contains the default set.
```
GET /revisions?$select=value,label,last_modified&api-version={api-version} HTTP/1.1
```

#
#
#
## Time-Based Access
#
Obtain a representation of the result as it was at a past time. See section [2.1.1](https://tools.ietf.org/html/rfc7089#section-2.1)
```
GET /revisions?api-version={api-version} HTTP/1.1
Accept-Datetime: Sat, 12 May 2018 02:10:00 GMT
```

**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.revs+json"
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