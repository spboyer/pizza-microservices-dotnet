using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace toppings_api
{
    public class ToppingsDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly TelemetryClient _telemetry;

        public ToppingsDbContext(IOptions<Settings> options, TelemetryClient telemetry)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
            _telemetry = telemetry;
        }

        public IMongoCollection<Topping> Toppings
        {
            get
            {
                IMongoCollection<Topping> results = null;
                using (var operation = _telemetry.StartOperation<DependencyTelemetry>("Database"))
                {
                    operation.Telemetry.Type = "MongoDB";
                    operation.Telemetry.Data = "toppings";
                    try
                    {
                        results = _db.GetCollection<Topping>("Toppings");
                    }
                    catch (Exception ex)
                    {
                        operation.Telemetry.Success = false;

                        _telemetry.TrackException(ex);
                    }
                    finally
                    {
                        operation.Telemetry.Success = true;
                        operation.Telemetry.Stop();
                    }

                    return results;
                }
            }   
        }

    }

    public interface IApplicationDbContext
    {
        IMongoCollection<Topping> Toppings { get; }
    }
}