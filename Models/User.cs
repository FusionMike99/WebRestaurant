using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Display(Name = "Список замовлень")]
        public List<Order> Orders { get; set; } = new List<Order>();
        [Display(Name = "Список замовлень інгредієнтів")]
        public List<OrderIngredients> OrderIngredients { get; set; } = new List<OrderIngredients>();
        [Display(Name = "Кошик")]
        public List<ShoppingCartItem> ShoppingCart { get; set; } = new List<ShoppingCartItem>();
        [Display(Name = "Графік робіт")]
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
