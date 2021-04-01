using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Кількість")]
        public int Count { get; set; }

        [Display(Name = "Список замовлень")]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
