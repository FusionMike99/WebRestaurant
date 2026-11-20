using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.ViewModels;

namespace WebApplicationRestaurant.Controllers;

public class HomeController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("waiter"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var schedule = await context.Schedules.Include(s => s.Places)
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.WorkingDate.Date == DateTime.Today.Date);
                ViewBag.Places = schedule?.Places ?? [];
            }

            return View();
        }

        return RedirectToAction("Login", "Account");
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
