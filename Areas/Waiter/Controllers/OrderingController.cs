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

namespace WebApplicationRestaurant.Areas.Waiter.Controllers
{
    [Area("Waiter")]
    public class OrderingController : Controller
    {
        private readonly ApplicationContext _context;

        public OrderingController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Dishes.Include(d => d.Category).Include(d => d.DishUnit);
            return View(await applicationContext.ToListAsync());
        }

        public IActionResult GoToCart()
        {
            return View("Cart", HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart"));
        }

        public IActionResult AddToCart(Dish model)
        {
            if (ModelState.IsValid)
            {
                List<ShoppingCartItem> shoppingCart;
                if (HttpContext.Session.Keys.Contains("shoppingCart"))
                {
                    shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
                }
                else
                {
                    shoppingCart = new List<ShoppingCartItem>();
                }
                var shoppingCartItem = new ShoppingCartItem();
                shoppingCartItem.Dish = model;
                shoppingCart.Add(shoppingCartItem);
                HttpContext.Session.Set("shoppingCart", shoppingCart);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFromCart(ShoppingCartItem model)
        {
            if (ModelState.IsValid)
            {
                var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
                shoppingCart.Remove(model);
                HttpContext.Session.Set("shoppingCart", shoppingCart);
            }
            return RedirectToAction(nameof(GoToCart));
        }

        public IActionResult ClearCart()
        {
            if (ModelState.IsValid)
            {
                var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
                shoppingCart.Clear();
                HttpContext.Session.Set("shoppingCart", shoppingCart);
            }
            return RedirectToAction(nameof(GoToCart));
        }

        public IActionResult GoToOrder()
        {
            ViewData["PlaceId"] = new SelectList(_context.Places.Where(p => p.Available), "Id", "Number");
            var order = new Order();
            order.ShoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
            return View("Order", order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(_context.Places.Where(p => p.Available), "Id", "Number");
            return View("Order", order);
        }
    }
}
