# Keys - REST API Reference
#
**Represents Key resource**.

```
{
    "name": [string]             // Name of the key
}
```

Supports the following operations:
- List

For all operations ``name`` is an optional filter parameter. If ommited it implies **any** key.


#
#
*Prerequisites*: 
All HTTP requests must be authenticated. See the [authentication](./authentication) section.

#
#
## List Keys
#
```
GET /keys HTTP/1.1
```
**Responses:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.keyset+json; charset=utf-8"
```

```sh
{
    "items": [
        {
          "name": "{key-name}"
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
Alternatively the content provides a next link in the form of the ``@nextLink`` property.
```
GET /keys HTTP/1.1
```
**Response:**
```
HTTP/1.1 OK
Content-Type: application/vnd.microsoft.appconfig.keyset+json; charset=utf-8
Link: </keys?after={token}>; rel="next"
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
GET /keys?name={key-name}
```

**Supported filters**

|Key Name||
|--|--|
|```name``` is omitted or ```name=*```|Matches **any** key|
|```name=abc```|Matches a key named  **abc**|
|```name=abc*```|Matches key names that start with **abc**|
|```name=*abc```|Matches key names that end with **abc**|
|```name=*abc*```|Matches key names that contains **abc**|
|```name=abc,xyz```|Matches key names **abc** or **xyz** (limited to 5 CSV)|

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
  "title": "Invalid request parameter 'name'",
  "name": "name",
  "detail": "name(2): Invalid character",
  "status": 400
}
```

**Examples**

- All
```
GET /keys
```

- Key name starts with **abc**
```
GET  /keys?name=**abc***
```

- Key name is either **abc** or **xyz**
```
GET /revisions?name=**abc,xyz**
```

#
#
#
## Request specific fields
#
Use the optional ``$select`` query string parameter and provide comma separated list of requested fields. If the ``$select`` parameter is ommited, the response contains the default set.
```
GET /keys?$select=name HTTP/1.1
```

#
#
#
## Time-Based Access
#
Obtain a representation of the result as it was at a past time. See section [2.1.1](https://tools.ietf.org/html/rfc7089#section-2.1)
```
GET /keys HTTP/1.1
Accept-Datetime: Sat, 12 May 2018 02:10:00 GMT
```

**Response:**
```
HTTP/1.1 200 OK
Content-Type: application/vnd.microsoft.appconfig.keyset+json"
Memento-Datetime: Sat, 12 May 2018 02:10:00 GMT
Link: </keys>; rel="original"
```
```
{
    "items": [
        ....
    ]
}
```