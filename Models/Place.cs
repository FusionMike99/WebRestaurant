using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.Models;

public class Place
{
    public int Id { get; set; }

    [Display(Name = "Номер столику")]
    public int Number { get; set; }

    [Display(Name = "Доступність")]
    public bool Available { get; set; } = true;

    [Display(Name = "Ім'я того, хто забронював")]
    public string? ReserverName { get; set; }

    [Display(Name = "Час бронювання")]
    public DateTime? ReserveTime { get; set; }

    [Display(Name = "Список замовлень")]
    public List<Order> Orders { get; set; } = [];

    [Display(Name = "Список графіків")]
    public List<Schedule> Schedules { get; set; } = [];
}
