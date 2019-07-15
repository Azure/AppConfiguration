using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.AppConfiguration.WebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
                //
            // We add a Settings model to the service container, which takes its values from the applications configuration.
            services.Configure<Settings>(Configuration.GetSection("WebDemo:Settings"));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //
            // Use Azure App Configuration to allow requests to trigger refresh of the configuration
            app.UseAzureAppConfiguration();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
