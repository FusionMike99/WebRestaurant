using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Observability;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers;

[Area("Cooker")]
[Authorize(Roles = "cooker")]
public class ChangingDishStatusesController(ApplicationContext context, ILogger<ChangingDishStatusesController> logger) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.ShoppingCartItem
            .Where(sci => sci.DishStatusId != DishStatus.ReadyId)
            .Include(sc => sc.Order)
            .Include(sc => sc.Dish)
            .Include(sc => sc.DishStatus)
            .ToListAsync());

    public async Task<IActionResult> ChangeStatus(int? idOrder, int? idDish, int idStatus)
    {
        try
        {
            var shoppingCartItem = await context.ShoppingCartItem.FindAsync(idOrder, idDish);
            if (shoppingCartItem is null)
            {
                logger.DishStatusItemNotFound(idOrder, idDish);
                return NotFound();
            }

            shoppingCartItem.DishStatusId = idStatus;
            context.Update(shoppingCartItem);
            await context.SaveChangesAsync();

            logger.DishStatusChanged(shoppingCartItem.OrderId, shoppingCartItem.DishId, idStatus);
            RestaurantTelemetry.DishStatusChanges.Add(1, new KeyValuePair<string, object?>("dish.status_id", idStatus));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShoppingCartItemExists(idOrder, idDish))
            {
                logger.DishStatusItemNotFound(idOrder, idDish);
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ShoppingCartItemExists(int? idOrder, int? idDish) =>
        context.ShoppingCartItem.Any(e => e.OrderId == idOrder && e.DishId == idDish);
}
