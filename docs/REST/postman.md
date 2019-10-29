# Postman Configuration - REST API Reference
#
To test the REST API using [Postman](https://www.getpostman.com/), requests need to include the headers required for [authentication](./authentication.md). Here's how to configure Postman for testing the REST API, generating the authentication headers automatically:

1. Create a new [request](https://learning.getpostman.com/docs/postman/sending_api_requests/requests/)

2. Add the `signRequest` function from the [JavaScript authentication sample](./authentication.md#JavaScript) to the [pre-request script](https://learning.getpostman.com/docs/postman/scripts/pre_request_scripts/) for the request

3. Add the following code to the end of the pre-request script and update the access key as indicated by the TODO comment

    ```js
    // TODO: Replace the following placeholders with your access key
    var credential = "<Credential>"; // Id
    var secret = "<Secret>"; // Value
    
    var sdk = require('postman-collection');
    var url = new sdk.Url(request.url);
    
    // request.data is an empty object for GET requests.
    var isBodyEmpty = JSON.stringify(request.data) === "{}";
    
    var headers = signRequest(
        url.getHost(),
        request.method,
        url.getPathWithQuery(),
        isBodyEmpty ? undefined : request.data,
        credential,
        secret);
    
    // Add headers to the request
    headers.forEach(header => {
        pm.request.headers.upsert({key: header.name, value: header.value});
    })
    ```

4. Send the request
