using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationRestaurant.Models;

public class User : IdentityUser
{
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Прізвище")]
    public string Surname { get; set; } = string.Empty;

    [Display(Name = "Список замовлень")]
    public List<Order> Orders { get; set; } = [];

    [Display(Name = "Список замовлень інгредієнтів")]
    public List<OrderIngredients> OrderIngredients { get; set; } = [];

    [Display(Name = "Кошик")]
    public List<ShoppingCartItem> ShoppingCart { get; set; } = [];

    [Display(Name = "Графік робіт")]
    public List<Schedule> Schedules { get; set; } = [];
}
