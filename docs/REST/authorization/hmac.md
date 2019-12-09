# HMAC Authorization - REST API Reference

When HMAC authentication is used, operations fall in to one of two categories, read or write. Read-write access keys grant permission to call all operations. Read-only access keys grant permission to call only read operations. Whether an access key is read-only or read-write is determined by its `readOnly` property. Any attempt to make a write request with a read-only access key will result in the request being unauthorized.

The specification describing access keys and the API used to obtain them is detailed in the Azure App Configuration resource provider spec [here](https://github.com/Azure/azure-rest-api-specs/blob/master/specification/appconfiguration/resource-manager/Microsoft.AppConfiguration/stable/2019-10-01/appconfiguration.json). Access keys are obtained via the "ConfigurationStores_ListKeys" operation.

## Errors

```sh
HTTP/1.1 403 Forbidden
```
**Reason:** The access key used to authenticate the request does not provide the required permissions to perform the requested operation.
**Solution:** Obtain an access key that provides permission to perform the requested operation and use it to authenticate the request.
