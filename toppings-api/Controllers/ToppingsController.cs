using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace toppings_api
{
    [Route("toppings")]
    [ApiController]
    public class ToppingsController : Controller
    {
        private readonly IApplicationDbContext _db;
        private readonly ILogger<ToppingsController> _logger;
        private readonly TelemetryClient _telemetry;

        public ToppingsController(ILogger<ToppingsController> logger, IApplicationDbContext db, TelemetryClient telemetry)
        {
            _telemetry = telemetry;
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Topping>>> GetToppings(TelemetryClient telemetry, [FromServices] IDistributedCache cache)
        {
            var toppings = await GetFromCache(cache);

            if (toppings == null || toppings.Count == 0)
            {
                toppings = await _db.Toppings
                    .Find(new BsonDocument())
                    .SortBy(t => t.Name)
                    .ToListAsync();

                var json = JsonSerializer.Serialize(toppings);

                await SetCache(cache, json);

            }

            return toppings;
        }

        private async Task SetCache(IDistributedCache cache, string json)
        {
            using (var operation = _telemetry.StartOperation<DependencyTelemetry>("Redis"))
            {
                operation.Telemetry.Type = "Redis";
                operation.Telemetry.Data = "toppings";
                try
                {
                    await cache.SetStringAsync("toppings", json, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15)
                    });
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
            }
        }

        private async Task<List<Topping>> GetFromCache(IDistributedCache cache)
        {
            using (var operation = _telemetry.StartOperation<DependencyTelemetry>("Redis"))
            {
                operation.Telemetry.Type = "Redis";
                operation.Telemetry.Data = "toppings";
                List<Topping> toppings;
                try
                {
                    var t = await cache.GetStringAsync("toppings");
                    toppings = string.IsNullOrEmpty(t) ? null : JsonSerializer.Deserialize<List<Topping>>(t);
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

                    toppings = new List<Topping>();
                }

                return toppings;
            }
        }

        /*         [HttpPost]
                public async Task Create(Topping topping)
                {
                    await _db.Toppings.InsertOneAsync(topping);
                } */
    }
}