# Multi-tenant application setup

The .NET Azure App Configuration Provider delivers configuration settings to the existing IConfiguration system. Multi-tenant applications using the provider can take advantage of IConfiguration features to retrieve tenant information from requests. The following instructions will demonstrate one way to achieve this application setup.

1. Define the per tenant settings in App Configuration as key-values using key prefixes.

    #### Tenant 1

    | Key | Value |
    |---- |-------|
    | Contoso:Name | Contoso Corp. |
    | Contoso:Color | Azure |

    #### Tenant 2

    | Key | Value |
    |---- |-------|
    | ExampleTenant:Name | Example Tenant, Inc. |
    | ExampleTenant:Color | Green |

2. Setup the options pattern by creating the following classes.

```cs
//
// Tenant info
//
public class TenantSettings
{
    public string Name { get; set; }
    public string Color { get; set; }
}
```

```cs
//
// Dynamically configure TenantSettings
//
public class ConfigureTenantSettings : IConfigureOptions<TenantSettings>
{
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfigureTenantSettings(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Configure(TenantSettings options)
        {
            //
            // Get tenant id from the request
            // ex. Read tenant from a request header
            if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(
                   "X-Tenant-Id",
                   out StringValues tenantId))
            {
                return;
            }
            
            //
            // Initialize from config section by TenantId
            _configuration.GetSection(tenantId).Bind(options);
        }
}
```

3. Register the options as a scoped DI service.

```cs
services.AddScoped<IConfigureOptions<TenantSettings>, ConfigureTenantSettings>();
```

4. Use in a DI Service or Controller via `IOptionsSnapshot`.

```cs
public class MyController : Controller
{
        private readonly IOptionsSnapshot<TenantSettings> _tenantSettings;

        public MyController(IOptionsSnapshot<TenantSettings> tenantSettings)
        {
            _tenantSettings = tenantSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            TenantSettings tenantSettings = _tenantSettings.Value;
  
            ...
        }
}
```