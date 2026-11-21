using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Waiter.Controllers;

[Area("Waiter")]
[Authorize(Roles = "waiter")]
public class ChangingOrderStatusesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var applicationContext = context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Place)
            .Include(o => o.Status)
            .Include(o => o.User)
            .Include(o => o.ShoppingCart);
        return View(await applicationContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await context.Orders
            .Include(o => o.Place)
            .Include(o => o.Status)
            .Include(o => o.User)
            .Include(o => o.ShoppingCart)
                .ThenInclude(sc => sc.Dish)
            .Include(o => o.ShoppingCart)
                .ThenInclude(sc => sc.DishStatus)
            .FirstOrDefaultAsync(m => m.Id == id);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost]
    public async Task<IActionResult> CloseOrder(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await context.Orders
            .Include(o => o.Place)
            .Include(o => o.Status)
            .Include(o => o.User)
            .Include(o => o.ShoppingCart)
                .ThenInclude(sc => sc.Dish)
            .Include(o => o.ShoppingCart)
                .ThenInclude(sc => sc.DishStatus)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order is null)
        {
            return NotFound();
        }

        if (order.ShoppingCart.Any(sc => sc.DishStatusId != DishStatus.ReadyId))
        {
            ModelState.AddModelError("", "Не приготовлені всі страви");
        }

        if (ModelState.IsValid)
        {
            try
            {
                order.StatusId = OrderStatus.ClosedId;
                order.FinishTime = DateTime.Now;
                context.Update(order);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View("Details", order);
    }

    private bool OrderExists(int id) =>
        context.Orders.Any(e => e.Id == id);
}
