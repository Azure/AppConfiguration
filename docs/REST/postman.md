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

    var isBodyEmpty = pm.request.body === null || pm.request.body === undefined || pm.request.body.isEmpty();

    var headers = signRequest(
        pm.request.url.getHost(),
        pm.request.method,
        pm.request.url.getPathWithQuery(),
        isBodyEmpty ? undefined : pm.request.body.toString(),
        credential,
        secret);

    // Add headers to the request
    headers.forEach(header => {
        pm.request.headers.upsert({key: header.name, value: header.value});
    })
    ```

4. Send the request
