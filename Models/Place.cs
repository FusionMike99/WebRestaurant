using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplicationRestaurant.Models
{
    public class Place
    {
        public int Id { get; set; }
        [Display(Name = "Доступність")]
        public bool Available { get; set; }
        [Display(Name = "Ім'я того, хто забронював")]
        public string ReserverName { get; set; }
        [Display(Name = "Час бронювання")]
        public DateTime? ReserveTime { get; set; }

        [Display(Name = "Список замовлень")]
        public List<Order> Orders { get; set; } = new List<Order>();
        [Display(Name = "Список графіків")]
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
