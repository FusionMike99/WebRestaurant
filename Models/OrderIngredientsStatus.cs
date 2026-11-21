using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class OrderIngredientsStatus
{
    public const int OpenId = 1;
    public const int ClosedId = 2;

    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Кількість")]
    public int Count { get; set; }

    [Display(Name = "Список замовлень")]
    public List<OrderIngredients> OrderIngredients { get; set; } = [];
}
