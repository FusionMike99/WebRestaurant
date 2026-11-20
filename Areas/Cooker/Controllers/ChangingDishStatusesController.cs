using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers;

[Area("Cooker")]
[Authorize(Roles = "cooker")]
public class ChangingDishStatusesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.ShoppingCartItem
            .Where(sci => sci.DishStatusId != 3)
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
                return NotFound();
            }

            shoppingCartItem.DishStatusId = idStatus;
            context.Update(shoppingCartItem);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShoppingCartItemExists(idOrder, idDish))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ShoppingCartItemExists(int? idOrder, int? idDish) =>
        context.ShoppingCartItem.Any(e => e.OrderId == idOrder && e.DishId == idDish);
}
