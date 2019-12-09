# Azure Active Directory Authorization - REST API Reference

When Azure Active Directory (AAD) authentication is used, authorization is handled by Azure Role Based Access Control (RBAC). Azure RBAC requires users to be assigned to roles in order to grant access to resources. Each role contains a set of actions that users assigned to the role will be able to perform.

## Roles

There following roles are built-in roles that are available in Azure subscriptions by default.

### Azure App Configuration Data Owner

This role provides full access to all operations.

### Azure App Configuration Data Reader

This role enables read operations.

## Actions

Roles contain a list of actions that users assigned to that role can perform. Azure App Configuration supports the following actions.

### Microsoft.AppConfiguration/configurationStores/keyValues/read

This action allows read access to App Configuration key-value resources such as /kv and /labels.

### Microsoft.AppConfiguration/configurationStores/keyValues/write

This action allows write access to App Configuration key-value resources.

### Microsoft.AppConfiguration/configurationStores/keyValues/delete

This action allows App Configuration key-value resources to be deleted. Note, deleting a resource returns the key-value that was deleted.

## Errors

```sh
HTTP/1.1 403 Forbidden
```
**Reason:** The principal making the request does not have the required permissions to perform the requested operation.
**Solution:** Assign the role required to perform the requested operation to the principal making the request.

## Managing Role Assignments

Managing role assignments is done using [Azure RBAC](https://docs.microsoft.com/en-us/azure/role-based-access-control/overview) procedures that are standard across all Azure services. It is possible to do this through Azure CLI, PowerShell, the Azure Portal, and more. Official documentation on how to make role assignments can be found [here](https://docs.microsoft.com/en-us/azure/role-based-access-control/role-assignments-portal).