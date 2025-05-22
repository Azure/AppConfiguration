// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Azure.Identity;
using Microsoft.FeatureManagement;
using WebDemo;

var builder = WebApplication.CreateBuilder(args);

// Get the Azure App Configuration endpoint from environment or settings
string? appConfigEndpoint = builder.Configuration["AppConfig:Endpoint"] ?? 
                            Environment.GetEnvironmentVariable("AppConfigEndpoint");

// Load configuration from Azure App Configuration
if (!string.IsNullOrEmpty(appConfigEndpoint))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        // Use DefaultAzureCredential for Microsoft Entra ID authentication
        options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
               // Load all keys that start with `WebDemo:` and have no label
               .Select("WebDemo:*")
               // Reload configuration if any selected key-values have changed
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.RegisterAll();
               })
               // Load all feature flags with no label
               .UseFeatureFlags();
    });
}

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// Add Azure App Configuration and feature management services to the container
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();

// Bind configuration to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("WebDemo:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
    app.UseHsts();
}

// Use Azure App Configuration middleware for dynamic configuration refresh
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
