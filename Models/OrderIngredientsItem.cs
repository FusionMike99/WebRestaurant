using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class OrderIngredientsItem
    {
        public int IngredientId { get; set; }
        [Display(Name = "Інгредієнт")]
        public Ingredient Ingredient { get; set; }

        public int OrderIngredientsId { get; set; }
        [Display(Name = "Замовлення")]
        public OrderIngredients OrderIngredients { get; set; }

        [Display(Name = "Кількість")]
        public decimal Count { get; set; } = 1;
    }
}
