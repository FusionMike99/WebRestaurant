using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class Order
{
    public int Id { get; set; }

    [Display(Name = "Час створення")]
    public DateTime CreateTime { get; set; }

    [Display(Name = "Час закриття")]
    public DateTime? FinishTime { get; set; }

    [Display(Name = "Сума")]
    public double Amount { get; set; }

    [Display(Name = "Статус")]
    public int StatusId { get; set; }

    [Display(Name = "Столик")]
    public int PlaceId { get; set; }

    [Display(Name = "Офіціант")]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Статус")]
    public OrderStatus Status { get; set; } = null!;

    [Display(Name = "Столик")]
    public Place Place { get; set; } = null!;

    [Display(Name = "Офіціант")]
    public User User { get; set; } = null!;

    [Display(Name = "Список страв")]
    public List<Dish> Dishes { get; set; } = [];

    [Display(Name = "Кошик")]
    public List<ShoppingCartItem> ShoppingCart { get; set; } = [];
}
