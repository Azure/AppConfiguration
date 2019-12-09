﻿# Authorization

Authorization refers to the procedure used to determine the permissions that a caller has when making a request. There are multiple authorization models. The authorization model that is used for a request depends on the [authentication](../authentication/index.md) method that is used. The authorization models are listed below.

## HMAC

The [authorization model](./hmac.md) model associated with HMAC authentication splits permissions into read-only or read-write. See the [HMAC authorization](./hmac.md) page for details.

## Azure Active Directory

The [authorization model](./aad.md) associated with Azure Active Directory (AAD) authentication uses Azure RBAC to control permissions. See the [AAD authorization](./aad.md) page for details.