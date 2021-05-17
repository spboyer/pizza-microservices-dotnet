using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using MongoDB.Bson;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace toppings_api
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
            services.AddCors(options =>
                options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin(); }
            ));

            services.AddSingleton<ITelemetryInitializer, CloudRoleNameInitializer>();

            services.AddApplicationInsightsTelemetry();

            services.Configure<Settings>(
                  options =>
                  {
                      options.ConnectionString = Configuration.GetConnectionString("mongo");
                      options.Database = Configuration.GetSection("MongoDb:Database").Value;
                      options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
                      options.Development = HostingEnvironment.IsDevelopment();                      
                  });

            // Database Exception Page
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Health Check
            // https://docs.microsoft.com/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0
            services.AddHealthChecks()
            .AddMongoDb(mongodbConnectionString: Configuration.GetConnectionString("mongo"),
                name: "mongo", 
                failureStatus: HealthStatus.Unhealthy); //adding MongoDb Health Check

            services.AddControllers();

            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = Configuration.GetConnectionString("redis");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "toppings_api", Version = "v1" });
            });

            services.AddTransient<IApplicationDbContext, ToppingsDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "toppings_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/startup", new HealthCheckOptions()
                {
                    AllowCachingResponses = false
                });
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapHealthChecks("/ready");
            });

            // Check and Seed the database
            CheckandSeed(app.ApplicationServices);
        }

        private void CheckandSeed(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IApplicationDbContext>();

                if (db.Toppings.CountDocuments(new BsonDocument()) == 0)
                {
                    SeedData.Initialize(db);
                }
            }
        }
    }
}
