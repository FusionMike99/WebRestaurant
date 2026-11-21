using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class DishStatus
{
    public const int AwaitingId = 1;
    public const int AcceptedId = 2;
    public const int ReadyId = 3;

    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Кількість")]
    public int Count { get; set; }

    [Display(Name = "Кошик")]
    public List<ShoppingCartItem> ShoppingCart { get; set; } = [];
}
