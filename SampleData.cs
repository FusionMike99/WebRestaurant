using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Data;

namespace WebApplicationRestaurant
{
    public static class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "First meals" },
                    new Category { Name = "Second meals" },
                    new Category { Name = "Salads" },
                    new Category { Name = "Pizzas" },
                    new Category { Name = "Drinks" }
                );
                context.SaveChanges();
            }

            if (!context.DishStatuses.Any())
            {
                context.DishStatuses.AddRange(
                    new DishStatus { Name = "awaiting" },
                    new DishStatus { Name = "accepted" },
                    new DishStatus { Name = "ready" }
                );
                context.SaveChanges();
            }

            if (!context.DishUnits.Any())
            {
                context.DishUnits.AddRange(
                    new DishUnit { Name = "грам" },
                    new DishUnit { Name = "кілограм" },
                    new DishUnit { Name = "літр" }
                );
                context.SaveChanges();
            }

            if (!context.IngredientUnits.Any())
            {
                context.IngredientUnits.AddRange(
                    new IngredientUnit { Name = "грам" },
                    new IngredientUnit { Name = "кілограм" },
                    new IngredientUnit { Name = "літр" }
                );
                context.SaveChanges();
            }

            if (!context.OrderIngredientsStatuses.Any())
            {
                context.OrderIngredientsStatuses.AddRange(
                    new OrderIngredientsStatus { Name = "open" },
                    new OrderIngredientsStatus { Name = "close" }
                );
                context.SaveChanges();
            }

            if (!context.OrderStatuses.Any())
            {
                context.OrderStatuses.AddRange(
                    new OrderStatus { Name = "open" },
                    new OrderStatus { Name = "close" }
                );
                context.SaveChanges();
            }
        }
    }
}
