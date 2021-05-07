using System.Collections.Generic;

namespace toppings_api
{
    public static class SeedData
    {
        public static void Initialize(IApplicationDbContext db)
        {
            var toppings = new List<Topping>();

            toppings.Add(new Topping("Extra cheese", 2.50m));
            toppings.Add(new Topping("American bacon", 2.99m));
            toppings.Add(new Topping("British bacon", 2.99m));
            toppings.Add(new Topping("Canadian bacon", 2.99m));
            toppings.Add(new Topping("Tea and crumpets", 5.00m));
            toppings.Add(new Topping("Fresh-baked scones", 4.50m));
            toppings.Add(new Topping("Bell peppers", 1.00m));
            toppings.Add(new Topping("Onions", 1.00m));
            toppings.Add(new Topping("Mushrooms", 1.00m));
            toppings.Add(new Topping("Pepperoni", 1.00m));
            toppings.Add(new Topping("Duck sausage", 3.20m));
            toppings.Add(new Topping("Venison meatballs", 2.50m));
            toppings.Add(new Topping("Served on a silver platter", 250.99m));
            toppings.Add(new Topping("Lobster on top", 64.50m));
            toppings.Add(new Topping("Sturgeon caviar", 101.75m));
            toppings.Add(new Topping("Artichoke hearts", 3.40m));
            toppings.Add(new Topping("Fresh tomatoes", 1.50m));
            toppings.Add(new Topping("Basil", 1.50m));
            toppings.Add(new Topping("Steak (medium-rare)", 8.50m));
            toppings.Add(new Topping("Blazing hot peppers", 4.20m));
            toppings.Add(new Topping("Buffalo chicken", 5.00m));
            toppings.Add(new Topping("Blue cheese", 2.50m));

            toppings.ForEach(t => db.Toppings.InsertOne(t));
        }
    }
}