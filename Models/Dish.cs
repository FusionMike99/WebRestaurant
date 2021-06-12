using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Вага")]
        public double Weight { get; set; }
        [Display(Name = "Вартість")]
        public double Cost { get; set; }
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        [Display(Name = "Одиниця виміру")]
        public int DishUnitId { get; set; }

        [Display(Name = "Категорія")]
        public Category Category { get; set; }
        [Display(Name = "Одиниця виміру")]
        public DishUnit DishUnit { get; set; }
        [Display(Name = "Список інгредієнтів")]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        [Display(Name = "Список замовлень")]
        public List<Order> Orders { get; set; } = new List<Order>();
        [Display(Name = "Кошик")]
        public List<ShoppingCartItem> ShoppingCart { get; set; } = new List<ShoppingCartItem>();
        [Display(Name = "Список планів меню")]
        public List<MenuPlan> MenuPlans { get; set; } = new List<MenuPlan>();
    }
}
