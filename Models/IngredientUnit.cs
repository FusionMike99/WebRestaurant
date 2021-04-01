using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class IngredientUnit
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Список інгредієнтів")]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
