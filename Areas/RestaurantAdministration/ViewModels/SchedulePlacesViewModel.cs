using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;

public class SchedulePlacesViewModel
{
    public int ScheduleId { get; set; }
    public List<Place> Places { get; set; } = [];
    public List<int> ScheduleItems { get; set; } = [];
}
