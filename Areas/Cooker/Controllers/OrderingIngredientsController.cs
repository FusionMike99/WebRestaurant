using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers;

[Area("Cooker")]
[Authorize(Roles = "cooker")]
public class OrderingIngredientsController(ApplicationContext context) : Controller
{
    private const string CartSessionKey = "ingredientsCart";

    public async Task<IActionResult> Index()
    {
        var applicationContext = context.Ingredients.Include(i => i.IngredientUnit);
        return View(await applicationContext.ToListAsync());
    }

    public IActionResult GoToCart()
    {
        var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey);
        return ingredientsCart is null ? RedirectToAction(nameof(Index)) : View("Cart", ingredientsCart);
    }

    public async Task<IActionResult> AddToCart(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? [];
            var ingredientsCartItem = ingredientsCart.FirstOrDefault(ic => ic.IngredientId == id);
            if (ingredientsCartItem is null)
            {
                var ingredient = await context.Ingredients.FindAsync(id);
                if (ingredient is null)
                {
                    return NotFound();
                }

                ingredientsCart.Add(new OrderIngredientsItem
                {
                    Ingredient = ingredient,
                    IngredientId = id.Value
                });
            }
            else
            {
                ingredientsCartItem.Count++;
            }

            HttpContext.Session.Set(CartSessionKey, ingredientsCart);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult ChangeCart(int? ingredientId, decimal newCount)
    {
        if (ingredientId is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? [];
            var ingredientsCartItem = ingredientsCart.FirstOrDefault(ic => ic.IngredientId == ingredientId);
            if (ingredientsCartItem is not null)
            {
                ingredientsCartItem.Count = newCount;
            }

            HttpContext.Session.Set(CartSessionKey, ingredientsCart);
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
            var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? [];
            ingredientsCart.RemoveAll(ic => ic.IngredientId == id);
            HttpContext.Session.Set(CartSessionKey, ingredientsCart);
        }

        return RedirectToAction(nameof(GoToCart));
    }

    public IActionResult ClearCart()
    {
        if (ModelState.IsValid)
        {
            var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? [];
            ingredientsCart.Clear();
            HttpContext.Session.Set(CartSessionKey, ingredientsCart);
        }

        return RedirectToAction(nameof(GoToCart));
    }

    public IActionResult GoToOrder()
    {
        OrderIngredients orderIngredients = new()
        {
            OrderIngredientsItems = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? []
        };
        return View("Order", orderIngredients);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderIngredients order)
    {
        order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        order.OrderIngredientsItems = HttpContext.Session.Get<List<OrderIngredientsItem>>(CartSessionKey) ?? [];
        order.OrderIngredientsItems.ForEach(ic => ic.Ingredient = null!);
        if (ModelState.IsValid)
        {
            context.Add(order);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View("Order", order);
    }
}
