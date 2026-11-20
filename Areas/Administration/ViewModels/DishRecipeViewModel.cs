using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.ViewModels;

public class DishRecipeViewModel
{
    public int DishId { get; set; }
    public List<Ingredient> Ingredients { get; set; } = [];
    public List<int> Recipe { get; set; } = [];
}
