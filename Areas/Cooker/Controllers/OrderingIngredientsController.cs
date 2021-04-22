using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers
{
    [Area("Cooker")]
    public class OrderingIngredientsController : Controller
    {
        private readonly ApplicationContext _context;

        public OrderingIngredientsController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Ingredients.Include(i => i.IngredientUnit);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult GoToCart()
        {
            return View("Cart", HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart"));
        }

        public IActionResult AddToCart(Ingredient model)
        {
            if (ModelState.IsValid)
            {
                List<OrderIngredientsItem> ingredientsCart;
                if (HttpContext.Session.Keys.Contains("ingredientsCart"))
                {
                    ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
                }
                else
                {
                    ingredientsCart = new List<OrderIngredientsItem>();
                }
                var ingredientsCartItem = new OrderIngredientsItem();
                ingredientsCartItem.Ingredient = model;
                ingredientsCart.Add(ingredientsCartItem);
                HttpContext.Session.Set("ingredientsCart", ingredientsCart);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFromCart(OrderIngredientsItem model)
        {
            if (ModelState.IsValid)
            {
                var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
                ingredientsCart.Remove(model);
                HttpContext.Session.Set("ingredientsCart", ingredientsCart);
            }
            return RedirectToAction(nameof(GoToCart));
        }

        public IActionResult ClearCart()
        {
            if (ModelState.IsValid)
            {
                var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
                ingredientsCart.Clear();
                HttpContext.Session.Set("ingredientsCart", ingredientsCart);
            }
            return RedirectToAction(nameof(GoToCart));
        }

        public IActionResult GoToOrder()
        {
            var orderIngredients = new OrderIngredients();
            orderIngredients.OrderIngredientsItems = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
            return View("Order", orderIngredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderIngredients order)
        {
            order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Order", order);
        }
    }
}
