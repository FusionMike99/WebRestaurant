using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата роботи")]
        public DateTime WorkingDate { get; set; }
        [Display(Name = "Офіціант")]
        public string UserId { get; set; }

        [Display(Name = "Офіціант")]
        public User User { get; set; }
        [Display(Name = "Список столиків")]
        public List<Place> Places { get; set; } = new List<Place>();
    }
}
