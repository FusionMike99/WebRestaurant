using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;

public class MenuDishesViewModel
{
    public int MenuId { get; set; }
    public List<Dish> Dishes { get; set; } = [];
    public List<int> MenuItems { get; set; } = [];
}
