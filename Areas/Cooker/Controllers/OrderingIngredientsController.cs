using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers
{
    [Area("Cooker")]
    [Authorize(Roles = "cooker")]
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
            var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
            if (ingredientsCart == null)
                return RedirectToAction(nameof(Index));
            else
                return View("Cart", ingredientsCart);
        }

        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<OrderIngredientsItem> ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart") 
                    ?? new List<OrderIngredientsItem>();
                var ingredientsCartItem = ingredientsCart.FirstOrDefault(ic => ic.IngredientId == id);
                if (ingredientsCartItem == null)
                {
                    ingredientsCart.Add(new OrderIngredientsItem
                    {
                        Ingredient = await _context.Ingredients.FindAsync(id),
                        IngredientId = (int)id
                    });
                }
                else
                {
                    ingredientsCartItem.Count++;
                }
                HttpContext.Session.Set("ingredientsCart", ingredientsCart);
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult ChangeCart(int? id, int count)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
                var ingredientsCartItem = ingredientsCart.FirstOrDefault(ic => ic.IngredientId == id);
                if (ingredientsCartItem != null)
                    ingredientsCartItem.Count += count;
                HttpContext.Session.Set("ingredientsCart", ingredientsCart);
            }
            return RedirectToAction(nameof(GoToCart));
        }

        public IActionResult DeleteFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var ingredientsCart = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
                ingredientsCart.RemoveAll(ic => ic.IngredientId == id);
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
            order.OrderIngredientsItems = HttpContext.Session.Get<List<OrderIngredientsItem>>("ingredientsCart");
            order.OrderIngredientsItems.ForEach(ic => ic.Ingredient = null);
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
