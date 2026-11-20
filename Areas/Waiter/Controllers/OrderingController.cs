using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Waiter.Controllers;

[Area("Waiter")]
[Authorize(Roles = "waiter")]
public class OrderingController(ApplicationContext context) : Controller
{
    private const string CartSessionKey = "shoppingCart";

    public async Task<IActionResult> Index()
    {
        var currentDate = DateTime.Now;
        var menuPlan = await context.MenuPlans.Include(m => m.Dishes)
            .SingleOrDefaultAsync(mp => currentDate >= mp.PlanStartDate && currentDate <= mp.PlanEndDate);
        return View(menuPlan?.Dishes.ToList() ?? []);
    }

    public IActionResult GoToCart()
    {
        var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey);
        return shoppingCart is null ? RedirectToAction(nameof(Index)) : View("Cart", shoppingCart);
    }

    public async Task<IActionResult> AddToCart(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? [];
            var shoppingCartItem = shoppingCart.FirstOrDefault(sc => sc.DishId == id);
            if (shoppingCartItem is null)
            {
                var dish = await context.Dishes.FindAsync(id);
                if (dish is null)
                {
                    return NotFound();
                }

                shoppingCart.Add(new ShoppingCartItem
                {
                    Dish = dish,
                    DishId = id.Value
                });
            }
            else
            {
                shoppingCartItem.Count++;
            }

            HttpContext.Session.Set(CartSessionKey, shoppingCart);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult ChangeCart(int? dishId, int newCount)
    {
        if (dishId is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? [];
            var shoppingCartItem = shoppingCart.FirstOrDefault(sc => sc.DishId == dishId);
            if (shoppingCartItem is not null)
            {
                shoppingCartItem.Count = newCount;
            }

            HttpContext.Session.Set(CartSessionKey, shoppingCart);
        }

        return RedirectToAction(nameof(GoToCart));
    }

    public IActionResult DeleteFromCart(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? [];
            shoppingCart.RemoveAll(sc => sc.DishId == id);
            HttpContext.Session.Set(CartSessionKey, shoppingCart);
        }

        return RedirectToAction(nameof(GoToCart));
    }

    public IActionResult ClearCart()
    {
        if (ModelState.IsValid)
        {
            var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? [];
            shoppingCart.Clear();
            HttpContext.Session.Set(CartSessionKey, shoppingCart);
        }

        return RedirectToAction(nameof(GoToCart));
    }

    public IActionResult GoToOrder()
    {
        ViewData["PlaceId"] = new SelectList(context.Places.Where(p => p.Available), "Id", "Number");
        Order order = new()
        {
            ShoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? []
        };
        return View("Order", order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order)
    {
        order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        order.ShoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>(CartSessionKey) ?? [];
        order.ShoppingCart.ForEach(sc => sc.Dish = null!);
        if (ModelState.IsValid)
        {
            context.Add(order);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["PlaceId"] = new SelectList(context.Places.Where(p => p.Available), "Id", "Number");
        return View("Order", order);
    }
}
