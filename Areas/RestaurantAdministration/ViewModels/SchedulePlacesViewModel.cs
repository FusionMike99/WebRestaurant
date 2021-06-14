using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels
{
    public class SchedulePlacesViewModel
    {
        public SchedulePlacesViewModel()
        {
            Places = new List<Place>();
            ScheduleItems = new List<int>();
        }

        public int ScheduleId { get; set; }
        public List<Place> Places { get; set; }
        public List<int> ScheduleItems { get; set; }
    }
}
