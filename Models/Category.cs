using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Перелік страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
