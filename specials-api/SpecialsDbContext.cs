using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Diagnostics;

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
                using (var operation = _telemetry.StartOperation<DependencyTelemetry>("Database"))
                {
                    operation.Telemetry.Type = "MongoDB";
                    operation.Telemetry.Data = "specials";
                    try
                    {
                        results = _db.GetCollection<PizzaSpecial>("PizzaSpecial");
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

                // USING TrackDependency method - long hand to telemetry.StartOperation<DependencyTelemetry>
                //IMongoCollection<PizzaSpecial> results = null;
                //var timer = StartLogger("GetSpecials");
                //var success = true;
                //try
                //{
                //    results = _db.GetCollection<PizzaSpecial>("PizzaSpecial");
                //}
                //catch (Exception ex)
                //{
                //    success = false;
                //    _telemetry.TrackException(ex);
                //}
                //finally
                //{
                //    timer.Item1.Stop();
                //    _telemetry.TrackDependency("Database", "mongo", "specials", timer.Item2, timer.Item1.Elapsed, success);
                //}

                //return results;

            }
        }

        //private Tuple<Stopwatch, DateTime>  StartLogger(string eventName)
        //{
        //    var startTime = DateTime.UtcNow;
        //    var timer = System.Diagnostics.Stopwatch.StartNew();
        //    _telemetry.TrackEvent(eventName);

        //    return new Tuple<Stopwatch, DateTime>(timer, startTime);
        //}
    }

   

    public interface IApplicationDbContext
    {
        IMongoCollection<PizzaSpecial> Specials { get; }
    }
}