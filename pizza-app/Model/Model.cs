using MongoDB.Bson;

namespace pizza_app.Model
{
    public record PizzaSpecial(string name, string description, decimal basePrice, string imageUrl)
    {
        public ObjectId id { get; init; }
        public string GetFormattedBasePrice => basePrice.ToString("0.00");
    }

    public record Topping(string name, decimal price)
    {
        public ObjectId Id { get; init; }
        public string GetFormattedPrice() => price.ToString("0.00");
    }
}