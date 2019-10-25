# Labels - REST API Reference
api-version: 1.0
#
**Represents Label resource**.

```
 {
      "name": [string]             // Name of the label
}
```

Supports the following operations:
- List

For all operations ``name`` is an optional filter parameter. If ommited it implies **any** label.


#
#
**Prerequisites**: 
- All HTTP requests must be authenticated. See the [authentication](./authentication.md) section.
- All HTTP requests must provide explicit ``api-version``. See the [versioning](./versioning.md) section.
#
#
## List Labels
#
```
GET /labels?api-version={api-version} HTTP/1.1
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.labelset+json; charset=utf-8"
```

```sh
{
    "items": [
        {
          "name": "{label-name}"
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
The result is paginated if the number of items returned exceeds the response limit. Follow the optional ``Link`` response headers and use ``rel="next"`` for navigation. 
Alternatively the content provides a next link in form of the ``@nextLink`` property. The next link contains ``api-version`` parameter.
```
GET /labels?api-version={api-version} HTTP/1.1
```
**Response:**
```
HTTP/1.1 OK
Content-Type: application/vnd.microsoft.appconfig.labelset+json; charset=utf-8
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
## Filtering
#
Filtering by ```name``` is supported. 

```
GET /labels?name={label-name}&api-version={api-version}
```

**Supported filters**

|Key Name||
|--|--|
|```name``` is omitted or ```name=*```|Matches **any** label|
|```name=abc```|Matches a label named  **abc**|
|```name=abc*```|Matches label names that start with **abc**|
|```name=*abc```|Matches label names that end with **abc**|
|```name=*abc*```|Matches label names that contains **abc**|
|```name=abc,xyz```|Matches label names **abc** or **xyz** (limited to 5 CSV)|


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
  "title": "Invalid request parameter 'name'",
  "name": "name",
  "detail": "name(2): Invalid character",
  "status": 400
}
```

**Examples**

- All
```
GET /labels?api-version={api-version}
```

- Label name starts with **abc**
```
GET  /labels?name=abc*&api-version={api-version}
```

- Label name ends with **abc**
```
GET  /labels?name=*abc&api-version={api-version}
```

- Label name contains **abc**
```
GET  /labels?name=*abc*&api-version={api-version}
```

- Label name is either **abc** or **xyz**
```
GET /labels?name=abc,xyz&api-version={api-version}
```

#
#
#
## Request specific fields
#
Use the optional ``$select`` query string parameter and provide comma separated list of requested fields. If the ``$select`` parameter is ommited, the response contains the default set.
```
GET /labels?$select=name&api-version={api-version} HTTP/1.1
```

#
#
#
## Time-Based Access
#
Obtain a representation of the result as it was at a past time. See section [2.1.1](https://tools.ietf.org/html/rfc7089#section-2.1)
```
GET /labels&api-version={api-version} HTTP/1.1
Accept-Datetime: Sat, 12 May 2018 02:10:00 GMT
```

**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.labelset+json"
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