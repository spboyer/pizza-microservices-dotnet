using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace toppings_api
{
    public class ToppingsDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _db;

        public ToppingsDbContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<Topping> Toppings => _db.GetCollection<Topping>("Toppings");
    }

    public interface IApplicationDbContext
    {
        IMongoCollection<Topping> Toppings { get; }
    }
}