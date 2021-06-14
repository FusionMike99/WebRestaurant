using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels
{
    public class MenuDishesViewModel
    {
        public MenuDishesViewModel()
        {
            Dishes = new List<Dish>();
            MenuItems = new List<int>();
        }

        public int MenuId { get; set; }
        public List<Dish> Dishes { get; set; }
        public List<int> MenuItems { get; set; }
    }
}
