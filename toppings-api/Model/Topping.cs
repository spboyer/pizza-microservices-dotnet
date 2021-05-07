using MongoDB.Bson;

namespace toppings_api
{
    public record Topping(string Name, decimal Price)
    {
        public ObjectId Id { get; init; }
        public string GetFormattedPrice() => Price.ToString("0.00");
    }
}