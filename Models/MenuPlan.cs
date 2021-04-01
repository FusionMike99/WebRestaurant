using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class MenuPlan
    {
        public int Id { get; set; }

        [Display(Name = "Дата початку плану")]
        public DateTime PlanStartDate { get; set; }

        [Display(Name = "Дата закінчення плану")]
        public DateTime PlanEndDate { get; set; }

        [Display(Name = "Список страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
