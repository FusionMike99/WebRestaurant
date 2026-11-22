using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Observability;

namespace WebApplicationRestaurant.Areas.Waiter.Controllers;

[Area("Waiter")]
[Authorize(Roles = "waiter")]
public class ChangingOrderStatusesController(ApplicationContext context, ILogger<ChangingOrderStatusesController> logger) : Controller
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

        using var activity = RestaurantTelemetry.ActivitySource.StartActivity("Order.Close");
        activity?.SetTag("order.id", id.Value);

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
            logger.OrderNotFound(id);
            return NotFound();
        }

        var unreadyCount = order.ShoppingCart.Count(sc => sc.DishStatusId != DishStatus.ReadyId);
        if (unreadyCount > 0)
        {
            logger.OrderCloseRejectedUnreadyDishes(order.Id, unreadyCount);
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

                logger.OrderClosed(order.Id, order.PlaceId);
                RestaurantTelemetry.OrdersClosed.Add(1);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    logger.OrderNotFound(order.Id);
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
