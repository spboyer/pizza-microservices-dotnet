
using System.Collections.Generic;

namespace specials_api
{
    public static class SeedData
    {
        public static void Initialize(IApplicationDbContext db)
        {
            var specials = new List<PizzaSpecial>();

            specials.Add(new PizzaSpecial("Basic Cheese Pizza", "It's cheesy and delicious. Why wouldn't you want one?", 9.99m, "img/pizzas/cheese.jpg"));
            specials.Add(new PizzaSpecial("The Baconatorizor", "It has EVERY kind of bacon", 11.99m, "img/pizzas/bacon.jpg"));
            specials.Add(new PizzaSpecial("Classic pepperoni", "It's the pizza you grew up with, but Blazing hot!", 10.50m, "img/pizzas/pepperoni.jpg"));
            specials.Add(new PizzaSpecial("Buffalo chicken", "Spicy chicken, hot sauce and bleu cheese, guaranteed to warm you up", 12.75m, "img/pizzas/meaty.jpg"));
            specials.Add(new PizzaSpecial("Mushroom Lovers", "It has mushrooms. Isn't that obvious?", 11.00m, "img/pizzas/mushroom.jpg"));
            specials.Add(new PizzaSpecial("The Brit", "When in London...", 10.25m, "img/pizzas/brit.jpg"));
            specials.Add(new PizzaSpecial("Veggie Delight", "It's like salad, but on a pizza", 11.50m, "img/pizzas/salad.jpg"));
            specials.Add(new PizzaSpecial("Margherita", "Traditional Italian pizza with tomatoes and basil", 9.99m, "img/pizzas/margherita.jpg"));

            specials.ForEach(t => db.Specials.InsertOne(t));
        }
    }
}