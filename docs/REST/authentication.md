# Authentication - REST API Reference
#
All HTTP requests must be authenticated using the **HMAC-SHA256** authentication scheme and transmitted over TLS. 

*Prerequisites*: 
- **Credential** - \<Access Key ID\>
- **Secret** - base64 decoded Access Key Value. ``base64_decode(<Access Key Value>)``

The values for credential (also called 'id') and secret (also called 'value') must be obtained from the Azure App Configuration instance. This can be done using the [Azure Portal](https://portal.azure.com) or the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest).

Provide each request with all HTTP headers required for Authentication. The minimum required are:

|  Request Header | Description  |
| --------------- | ------------ |
| **Host** | Internet host and port number. See section  [3.2.2](https://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.2.2) |
| **Date** | Date and Time at which the request was originated. It can not be more than 15 min off from current GMT. The value is an HTTP-date, as described in section [3.3.1](https://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.3.1)
| **x-ms-date** | Same as ```Date``` above. It can be used instead when the agent can't directly access ```Date``` request header or a proxy modifies it. If ```x-ms-date``` and ```Date``` are both provided, ```x-ms-date``` takes precedence. |
| **x-ms-content-sha256** | base64 encoded SHA256 hash of the request body. It must be provided even of there is no body. ```base64_encode(SHA256(body))```|
| **Authorization** | Authentication information required by **HMAC-SHA256** scheme. Format and details are explained below. |


**Example:**
```
Host: example.azconfig.io
Date: Fri, 11 May 2018 18:48:36 GMT
x-ms-content-sha256: 47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=
Authorization: HMAC-SHA256 Credential=a4016f0fa8fb0ef2&SignedHeaders=Host;x-ms-date;x-ms-content-sha256&Signature=jMXmttaxBJ0NmLlFKLZUkI8jdFu/8yqcTYzbkI3DGdU=
```
#
#
# Authorization Header
#
**Syntax:**

``Authorization``: **HMAC-SHA256** ```Credential```=\<value\>&```SignedHeaders```=\<value\>&```Signature```=\<value\>
#
#
|  Argument | Description  |
| ------ | ------ |
| **HMAC-SHA256** | Authorization Scheme _(required)_ |
| **Credential** | The ID of the access key used to compute the Signature. _(required)_ |
| **SignedHeaders** | HTTP Request Headers added to the signature. _(required)_ |
| **Signature** | base64 encoded HMACSHA256 of **String-To-Sign**. _(required)_|


#
#
### Credential

ID of the access key used to compute the **Signature**.

#
### Signed Headers

Semicolon separated HTTP request header names required to sign the request. These HTTP headers must be correctly provided with the request as well. **Don't use whitespaces**.

**Required HTTP request headers**:

```Host```;```x-ms-content-sha256```;```x-ms-date```[or ```Date```]

Any other HTTP request headers can also be added to the signing. Just append them to the ```SignedHeaders``` argument.

**Example:** 
Host;x-ms-content-sha256;x-ms-date;```Content-Type```;```Accept```

#
#
### Signature
Base64 encoded HMACSHA256 hash of the **String-To-Sign** using the access key identified by `Credential`.
```base64_encode(HMACSHA256(String-To-Sign, Secret))```

#
#
##### String-To-Sign
It represents canonical representation of the request:

_String-To-Sign=_

**HTTP_METHOD** + '\n' +
**path_and_query** + '\n' +
**signed_headers_values**


|  Argument | Description  |
| ------ | ------ |
| **HTTP_METHOD** | Uppercased HTTP method name used with the request. See [section 9](https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html) |
|**path_and_query** | Concatenation of request absolute URI path and query string. See [section 3.3](https://tools.ietf.org/html/rfc3986#section-3.3).
| **signed_headers_values** | Semicolon separated values of all HTTP request headers listed in **SignedHeaders**. The format follows **SignedHeaders** semantic. |

**Example:**
```js
string-To-Sign=
            "GET" + '\n' +                                                                                      // VERB
            "/kv?fields=*" + '\n' +                                                                             // path_and_query
            "Fri, 11 May 2018 18:48:36 GMT;example.azconfig.io;47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="    // signed_headers_values
```

#
#
### **Errors**
#
#

```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256
```
**Reason:** Authorization request header with HMAC-SHA256 scheme is not provided.
**Solution:** Provide valid ```Authorization``` HTTP request header
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="The access token has expired"
```
**Reason:** ```Date``` or ```x-ms-date``` request header is more than 15 minutes off from the current GMT time.
**Solution:** Provide correct date and time
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="Invalid access token date"
```
**Reason:** Missing or invalid ```Date``` or ```x-ms-date``` request header
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="[Credential][SignedHeaders][Signature] is required"
```
**Reason:** Missing a required parameter from ```Authorization``` request header
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="Invalid Credential"
```
**Reason:** Provided [```Host```]/[Access Key ID] is not found.
**Solution:** Check the ```Credential``` parameter of the ```Authorization``` request header and make sure it is a valid Access Key ID. Make sure the ```Host``` header points to the registered account.
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="Invalid Signature"
```
**Reason:** The ```Signature``` provided doesn't match what the server expects.
**Solution:** Make sure the ```String-To-Sign``` is correct. Make sure the ```Secret``` is correct and properly used (base64 decoded prior to using). See **Examples** section.
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="Signed request header 'xxx' is not provided"
```
**Reason:** Missing request header required by ```SignedHeaders``` parameter in ```Authorization``` header.
**Solution:** Provide the required header with correct value.
#
#
#
```sh
HTTP/1.1 401 Unauthorized
WWW-Authenticate: HMAC-SHA256 error="invalid_token" error_description="XXX is required as a signed header"
```
**Reason:** Missing parameter in ```SignedHeaders```.
**Solution:** Check **Signed Headers** minimum requirements.
#
#
#



## Code snippets

### JavaScript
*Prerequisites*: [Crypto-JS](https://code.google.com/archive/p/crypto-js/)

```js
function signRequest(host, 
                     method,      // GET, PUT, POST, DELETE
                     url,         // path+query
                     body,        // request body (undefined of none)
                     credential,  // access key id
                     secret)      // access key value (base64 encoded)
{
        var verb = method.toUpperCase();
        var utcNow = new Date().toUTCString();
        var contentHash = CryptoJS.SHA256(body).toString(CryptoJS.enc.Base64);

        //
        // SignedHeaders
        var signedHeaders = "x-ms-date;host;x-ms-content-sha256"; // Semicolon separated header names

        //
        // String-To-Sign
        var stringToSign = 
            verb + '\n' +                              // VERB
            url + '\n' +                               // path_and_query
            utcNow + ';' + host + ';' + contentHash;   // Semicolon separated SignedHeaders values

        //
        // Signature
        var signature = CryptoJS.HmacSHA256(stringToSign, CryptoJS.enc.Base64.parse(secret)).toString(CryptoJS.enc.Base64);

        //
        // Result request headers
        return [
            { name: "x-ms-date", value: utcNow },
            { name: "x-ms-content-sha256", value: contentHash },
            { name: "Authorization", value: "HMAC-SHA256 Credential=" + credential + "&SignedHeaders=" + signedHeaders + "&Signature=" + signature }
        ];
}
```

### C#

```cs
using (var client = new HttpClient())
{
    var request = new HttpRequestMessage()
    {
        RequestUri = new Uri("http://example.azconfig.io/kv"),
        Method = HttpMethod.Get
    };

    //
    // Sign the request
    request.Sign(<Credential>, <Secret>);

    await client.SendAsync(request);
}

static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage Sign(this HttpRequestMessage request, string credential, byte[] secret)
    {
        string host = request.RequestUri.Authority;
        string verb = request.Method.ToString().ToUpper();
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        string contentHash = Convert.ToBase64String(request.Content.ComputeSha256Hash());

        //
        // SignedHeaders
        string signedHeaders = "date;host;x-ms-content-sha256"; // Semicolon separated header names

        //
        // String-To-Sign
        var stringToSign = $"{verb}\n{request.RequestUri.PathAndQuery}\n{utcNow.ToString("r")};{host};{contentHash}";

        //
        // Signature
        string signature;

        using (var hmac = new HMACSHA256(secret))
        {
            signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(stringToSign)));
        }

        //
        // Add headers
        request.Headers.Date = utcNow;
        request.Headers.Add("x-ms-content-sha256", contentHash);
        request.Headers.Authorization = new AuthenticationHeaderValue("HMAC-SHA256", $"Credential={credential}&SignedHeaders={signedHeaders}&Signature={signature}");

        return request;
    }
}

static class HttpContentExtensions
{
    public static byte[] ComputeSha256Hash(this HttpContent content)
    {
        using (var stream = new MemoryStream())
        {
            if (content != null)
            {
                content.CopyToAsync(stream).Wait();
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (var alg = SHA256.Create())
            {
                return alg.ComputeHash(stream.ToArray());
            }
        }
    }
}
```
### Java
```java
public CloseableHttpResponse signRequest(HttpUriRequest request, String credential, String secret)
        throws IOException, URISyntaxException {
    Map<String, String> authHeaders = generateHeader(request, credential, secret);
    authHeaders.forEach(request::setHeader);

    return httpClient.execute(request);
}

private static Map<String, String> generateHeader(HttpUriRequest request, String credential, String secret) 
        throws URISyntaxException, IOException {
    String requestTime = GMT_DATE_FORMAT.format(new Date());

    String contentHash = buildContentHash(request);
    // SignedHeaders
    String signedHeaders = "x-ms-date;host;x-ms-content-sha256";

    // Signature
    String methodName = request.getRequestLine().getMethod().toUpperCase();
    URIBuilder uri = new URIBuilder(request.getRequestLine().getUri());
    String scheme = uri.getScheme() + "://";
    String requestPath = uri.toString().substring(scheme.length()).substring(uri.getHost().length());
    String host = new URIBuilder(request.getRequestLine().getUri()).getHost();
    String toSign = String.format("%s\n%s\n%s;%s;%s", methodName, requestPath, requestTime, host, contentHash);

    byte[] decodedKey = Base64.getDecoder().decode(secret);
    String signature = Base64.getEncoder().encodeToString(new HmacUtils(HMAC_SHA_256, decodedKey).hmac(toSign));

    // Compose headers
    Map<String, String> headers = new HashMap<>();
    headers.put("x-ms-date", requestTime);
    headers.put("x-ms-content-sha256", contentHash);

    String authorization = String.format("HMAC-SHA256 Credential=%s, SignedHeaders=%s, Signature=%s",
            credential, signedHeaders, signature);
    headers.put("Authorization", authorization);

    return headers;
}

private static String buildContentHash(HttpUriRequest request) throws IOException {
    String content = "";
    if (request instanceof HttpEntityEnclosingRequest) {
        try {
            StringWriter writer = new StringWriter();
            IOUtils.copy(((HttpEntityEnclosingRequest) request).getEntity().getContent(), writer,
                    StandardCharsets.UTF_8);

            content = writer.toString();
        }
        finally {
            ((HttpEntityEnclosingRequest) request).getEntity().getContent().close();
        }
    }

    byte[] digest = new DigestUtils(SHA_256).digest(content);
    return Base64.getEncoder().encodeToString(digest);
}
```
### Golang
```golang
import (
	"bytes"
	"crypto/hmac"
	"crypto/sha256"
	"encoding/base64"
	"io/ioutil"
	"net/http"
	"strings"
	"time"
)

//SignRequest Setup the auth header for accessing Azure AppConfiguration service
func SignRequest(id string, secret string, req *http.Request) error {
	method := req.Method
	host := req.URL.Host
	pathAndQuery := req.URL.Path
	if req.URL.RawQuery != "" {
		pathAndQuery = pathAndQuery + "?" + req.URL.RawQuery
	}

	content, err := ioutil.ReadAll(req.Body)
	if err != nil {
		return err
	}
	req.Body = ioutil.NopCloser(bytes.NewBuffer(content))

	key, err := base64.StdEncoding.DecodeString(secret)
	if err != nil {
		return err
	}

	timestamp := time.Now().UTC().Format(http.TimeFormat)
	contentHash := getContentHashBase64(content)
	stringToSign := fmt.Sprintf("%s\n%s\n%s;%s;%s", strings.ToUpper(method), pathAndQuery, timestamp, host, contentHash)
	signature := getHmac(stringToSign, key)

	req.Header.Set("x-ms-content-sha256", contentHash)
	req.Header.Set("x-ms-date", timestamp)
	req.Header.Set("Authorization", "HMAC-SHA256 Credential="+id+", SignedHeaders=x-ms-date;host;x-ms-content-sha256, Signature="+signature)

	return nil
}

func getContentHashBase64(content []byte) string {
	hasher := sha256.New()
	hasher.Write(content)
	return base64.StdEncoding.EncodeToString(hasher.Sum(nil))
}

func getHmac(content string, key []byte) string {
	hmac := hmac.New(sha256.New, key)
	hmac.Write([]byte(content))
	return base64.StdEncoding.EncodeToString(hmac.Sum(nil))
}
```