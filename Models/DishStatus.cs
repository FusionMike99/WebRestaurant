using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class DishStatus
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Кошик")]
        public List<ShoppingCartItem> ShoppingCart { get; set; } = new List<ShoppingCartItem>();
    }
}
