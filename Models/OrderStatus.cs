using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class OrderStatus
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Кількість")]
    public int Count { get; set; }

    [Display(Name = "Список замовлень")]
    public List<Order> Orders { get; set; } = [];
}
