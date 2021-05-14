using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace toppings_api
{
    [Route("toppings")]
    [ApiController]
    public class ToppingsController : Controller
    {
        private readonly IApplicationDbContext _db;
        private readonly ILogger<ToppingsController> _logger;

        public ToppingsController(ILogger<ToppingsController> logger, IApplicationDbContext db )
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Topping>>> GetToppings()
        {
            return await _db.Toppings
                .Find(new BsonDocument())
                .SortBy(t => t.Name)
                .ToListAsync();
        }

/*         [HttpPost]
        public async Task Create(Topping topping)
        {
            await _db.Toppings.InsertOneAsync(topping);
        } */
    }
}