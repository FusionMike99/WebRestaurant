using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        [Display(Name = "Дата роботи")]
        public DateTime WorkingDate { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Офіціант")]
        public User User { get; set; }
        [Display(Name = "Список столиків")]
        public List<Place> Places { get; set; } = new List<Place>();
    }
}
