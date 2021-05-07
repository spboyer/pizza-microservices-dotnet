using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace specials_api
{
    public class SpecialsDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _db;

        public SpecialsDbContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<PizzaSpecial> Specials => _db.GetCollection<PizzaSpecial>("PizzaSpecial");
    }

    public interface IApplicationDbContext
    {
        IMongoCollection<PizzaSpecial> Specials { get; }
    }
}