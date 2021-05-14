using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza_app
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameInitializer>();
            services.AddApplicationInsightsTelemetry();
            
            services.Configure<Settings>(
                  options => {
                      options.SpecialsApi = Configuration.GetServiceUri("specials-api") ?? new Uri(Configuration["Api:Specials"]);
                      options.ToppingsApi = Configuration.GetServiceUri("toppings-api") ?? new Uri(Configuration["Api:Toppings"]);
                      options.ProxyUri = Configuration.GetServiceUri("yarp-pizza") ?? new Uri(Configuration["Api:Yarp"]);
                      options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
                      options.Development = HostingEnvironment.IsDevelopment();
                  });
            services.AddHttpClient<Services.SpecialsService>();
            services.AddHttpClient<Services.ToppingsService>();

            services.AddRazorPages();
            //services.AddAzureAppConfiguration();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseAzureAppConfiguration();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
