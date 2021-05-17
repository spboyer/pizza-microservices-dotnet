using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace yarp_pizza
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
            services.AddCors(options =>
                options.AddPolicy("proxyPolicy", builder => { builder.AllowAnyOrigin(); }
            ));
            // Initialize the reverse proxy from the "ReverseProxy" section of configuration
            services.AddReverseProxy()
                .LoadFromConfig(Configuration.GetSection("ReverseProxy"))
                .AddConfigFilter<CustomConfigFilter>();

            services.AddSingleton<ITelemetryInitializer, CloudRoleNameInitializer>();
            services.AddApplicationInsightsTelemetry();
            
            services.AddHttpContextAccessor();
            // Interface that collects general metrics about the proxy

            services.AddSingleton<IProxyMetricsConsumer, ProxyMetricsConsumer>();

            // Registration of a consumer to events for proxy telemetry
            services.AddTelemetryConsumer<ProxyTelemetryConsumer>();

            // Registration of a consumer to events for HttpClient telemetry
            // Note: this depends on changes implemented in .NET 5
            services.AddTelemetryConsumer<HttpClientTelemetryConsumer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePerRequestMetricCollection();

            app.UseCors();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}
