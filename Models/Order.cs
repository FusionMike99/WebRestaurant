using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Час створення")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "Час закриття")]
        public DateTime? FinishTime { get; set; }
        [Display(Name = "Сума")]
        public double Amount { get; set; }
        public int StatusId { get; set; }
        public int PlaceId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Статус")]
        public OrderStatus Status { get; set; }
        [Display(Name = "Столик")]
        public Place Place { get; set; }
        [Display(Name = "Офіціант")]
        public User User { get; set; }
        [Display(Name = "Список страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        [Display(Name = "Кошик")]
        public List<ShoppingCartItem> ShoppingCart { get; set; } = new List<ShoppingCartItem>();
    }
}
