using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class Category
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Перелік страв")]
    public List<Dish> Dishes { get; set; } = [];
}
