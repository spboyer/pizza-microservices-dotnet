using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace specials_api
{
    public class SpecialsDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly TelemetryClient _telemetry;

        public SpecialsDbContext(IOptions<Settings> options, TelemetryClient telemetry)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
            _telemetry = telemetry;
        }

        public IMongoCollection<PizzaSpecial> Specials
        {
            get
            {
                IMongoCollection<PizzaSpecial> results = null;
                using (var operation = _telemetry.StartOperation<DependencyTelemetry>("Specials.Database"))
                {
                    operation.Telemetry.Type = "MongoDB";
                    operation.Telemetry.Data = "specials";
                    try
                    {
                        results = _db.GetCollection<PizzaSpecial>("PizzaSpecial");
                        operation.Telemetry.Success = true;
                    }
                    catch (Exception ex)
                    {
                        operation.Telemetry.Success = false;

                        _telemetry.TrackException(ex);
                    }
                    finally
                    {
                        operation.Telemetry.Stop();
                    }

                    return results;
                }

            }
        }
    }

    public interface IApplicationDbContext
    {
        IMongoCollection<PizzaSpecial> Specials { get; }
    }
}