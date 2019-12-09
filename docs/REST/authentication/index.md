# Authentication

All HTTP requests must be authenticated. The following authentication schemes are supported.

## HMAC

[HMAC authentication](./hmac.md) uses a randomly generated secret to sign request payloads. Details on how requests using this authentication method are authorized can be found in the [HMAC authorization](../authorization/hmac.md) section.

## Azure Active Directory

[Azure Active Directory (AAD) authentication](./aad.md) utilizes a bearer token that is obtained from Azure Active Directory to authenticate requests. Details on how requests using this authentication method are authorized can be found in the [AAD authorization](../authorization/aad.md) section.