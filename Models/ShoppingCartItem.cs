using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public partial class ShoppingCartItem
{
    public int DishId { get; set; }

    [Display(Name = "Страва")]
    public Dish Dish { get; set; } = null!;

    public int OrderId { get; set; }

    [Display(Name = "Замовлення")]
    public Order Order { get; set; } = null!;

    [Display(Name = "Кількість")]
    public int Count { get; set; } = 1;

    public int DishStatusId { get; set; }

    public string? UserId { get; set; }

    [Display(Name = "Статус страви")]
    public DishStatus DishStatus { get; set; } = null!;

    [Display(Name = "Кухар")]
    public User? User { get; set; }
}
