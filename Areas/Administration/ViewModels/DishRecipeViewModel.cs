using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.ViewModels
{
    public class DishRecipeViewModel
    {
        public DishRecipeViewModel()
        {
            Ingredients = new List<Ingredient>();
            Recipe = new List<int>();
        }

        public int DishId { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<int> Recipe { get; set; }
    }
}
