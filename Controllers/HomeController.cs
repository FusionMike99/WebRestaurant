using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Observability;
using WebApplicationRestaurant.ViewModels;

namespace WebApplicationRestaurant.Controllers;

public class HomeController(ApplicationContext context, ILogger<HomeController> logger) : Controller
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
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionFeature is not null)
        {
            logger.UnhandledException(exceptionFeature.Error, exceptionFeature.Path, requestId);
            Activity.Current?.SetStatus(ActivityStatusCode.Error, exceptionFeature.Error.Message);
        }

        return View(new ErrorViewModel { RequestId = requestId });
    }
}
