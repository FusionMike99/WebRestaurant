using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class OrderIngredients
    {
        public int Id { get; set; }
        [Display(Name = "Дата створення")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Дата замовлення")]
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Статус")]
        public OrderIngredientsStatus Status { get; set; }
        [Display(Name = "Кухар")]
        public User User { get; set; }
        [Display(Name = "Список інгредієнтів")]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        [Display(Name = "Кошик")]
        public List<OrderIngredientsItem> OrderIngredientsItems { get; set; } = new List<OrderIngredientsItem>();

    }
}
