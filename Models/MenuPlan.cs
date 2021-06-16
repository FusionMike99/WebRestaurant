using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models
{
    public class MenuPlan
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата початку плану")]
        public DateTime PlanStartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Дата закінчення плану")]
        public DateTime PlanEndDate { get; set; }

        [Display(Name = "Список страв")]
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
