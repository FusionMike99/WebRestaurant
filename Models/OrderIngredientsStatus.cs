using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class OrderIngredientsStatus
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Список замовлень")]
        public List<OrderIngredients> OrderIngredients { get; set; } = new List<OrderIngredients>();
    }
}
