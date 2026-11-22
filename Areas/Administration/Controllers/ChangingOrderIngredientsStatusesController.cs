using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Observability;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class ChangingOrderIngredientsStatusesController(
    ApplicationContext context,
    ILogger<ChangingOrderIngredientsStatusesController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var applicationContext = context.OrderIngredients
            .Include(o => o.Status)
            .Include(o => o.User)
            .Include(o => o.OrderIngredientsItems);
        return View(await applicationContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await context.OrderIngredients
            .Include(o => o.Status)
            .Include(o => o.User)
            .Include(o => o.OrderIngredientsItems)
                .ThenInclude(sc => sc.Ingredient)
            .FirstOrDefaultAsync(m => m.Id == id);

        return order is null ? NotFound() : View(order);
    }

    public async Task<IActionResult> CloseOrder(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await context.OrderIngredients.FindAsync(id);
        if (order is null)
        {
            logger.IngredientOrderNotFound(id);
            return NotFound();
        }

        order.StatusId = OrderIngredientsStatus.ClosedId;

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(order);
                await context.SaveChangesAsync();

                logger.IngredientOrderClosed(order.Id);
                RestaurantTelemetry.IngredientOrdersClosed.Add(1);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderIngredientsExists(order.Id))
                {
                    logger.IngredientOrderNotFound(order.Id);
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(order);
    }

    private bool OrderIngredientsExists(int id) =>
        context.OrderIngredients.Any(e => e.Id == id);
}
