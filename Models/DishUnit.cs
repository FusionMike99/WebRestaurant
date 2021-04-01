using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class DishUnit
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Список страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
