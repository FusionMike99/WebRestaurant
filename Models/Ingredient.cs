using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Кількість")]
    public double Count { get; set; }

    [Display(Name = "Дата доставки")]
    public DateTime DeliveryDate { get; set; }

    public int IngredientUnitId { get; set; }

    [Display(Name = "Одиниця виміру")]
    public IngredientUnit IngredientUnit { get; set; } = null!;

    [Display(Name = "Список страв")]
    public List<Dish> Dishes { get; set; } = [];

    [Display(Name = "Список замовлень")]
    public List<OrderIngredients> Orders { get; set; } = [];

    [Display(Name = "Кошик")]
    public List<OrderIngredientsItem> OrderIngredientsItems { get; set; } = [];
}
