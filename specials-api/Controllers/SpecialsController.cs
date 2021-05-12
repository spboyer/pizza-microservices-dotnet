using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace specials_api
{
    [Route("specials")]
    [ApiController]
    public class SpecialsController : Controller
    {
        private readonly IApplicationDbContext _db;
        private readonly ILogger<SpecialsController> _logger;

        public SpecialsController(ILogger<SpecialsController> logger, IApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<PizzaSpecial>>> GetPizzaSpecials()
        {
            return await _db.Specials
                .Find(new BsonDocument())
                .SortBy(t => t.Name)
                .ToListAsync();
        }

/*         [HttpPost]
        public async Task Create(PizzaSpecial special)
        {
            await _db.PizzaSpecials.InsertOneAsync(special);
        } */
    }
}