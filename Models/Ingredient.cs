using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Кількість")]
        public double Count { get; set; }
        [Display(Name = "Дата доставки")]
        public DateTime DeliveryDate { get; set; }
        public int IngredientUnitId { get; set; }

        [Display(Name = "Одиниця виміру")]
        public IngredientUnit IngredientUnit { get; set; }

        [Display(Name = "Список страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        [Display(Name = "Список замовлень")]
        public List<OrderIngredients> Orders { get; set; } = new List<OrderIngredients>();
        [Display(Name = "Кошик")]
        public List<OrderIngredientsItem> OrderIngredientsItems { get; set; } = new List<OrderIngredientsItem>();
    }
}
