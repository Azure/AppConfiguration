using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebDemoWithEventHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            // We add a Settings and EventHubConnection models to the service container, which takes its values from the applications configuration.
            services.Configure<Settings>(Configuration.GetSection("WebDemo:Settings"));
            services.Configure<EventHubConnection>(Configuration.GetSection("WebDemo:EventHubConnection"));

            // Add Azure App Configuration required services to the DI
            services.AddAzureAppConfiguration();

            // Add the service containing listener methods for EventHub updates.
            services.AddSingleton<EventHubService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Warmup the EventHub service
            app.ApplicationServices.GetService<EventHubService>();

            // Enable automatic configuration refresh from Azure App Configuration
            app.UseAzureAppConfiguration();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
