using MongoDB.Bson;

namespace specials_api
{
    public record PizzaSpecial(string Name, string Description, decimal BasePrice, string ImageUrl)
    {
        public ObjectId Id { get; init; }
        public string GetFormattedBasePrice => BasePrice.ToString("0.00");
    }
}