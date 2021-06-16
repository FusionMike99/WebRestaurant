using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.ViewModels;

namespace WebApplicationRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("waiter"))
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var schedule = await _context.Schedules.Include(s => s.Places)
                        .FirstOrDefaultAsync(s => s.UserId.Equals(userId) && s.WorkingDate.Date.Equals(System.DateTime.Today.Date));
                    ViewBag.Places = schedule.Places;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
