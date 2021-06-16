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

namespace WebApplicationRestaurant.Areas.Waiter.Controllers
{
    [Area("Waiter")]
    [Authorize(Roles = "waiter")]
    public class OrderingController : Controller
    {
        private readonly ApplicationContext _context;

        public OrderingController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentDate = DateTime.Now;
            var menuPlan = await _context.MenuPlans.Include(m => m.Dishes)
                .SingleOrDefaultAsync(mp => currentDate >= mp.PlanStartDate && currentDate <= mp.PlanEndDate);
            return View(menuPlan.Dishes.ToList());
        }

        public IActionResult GoToCart()
        {
            var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
            if(shoppingCart == null)
                return RedirectToAction(nameof(Index));
            else
                return View("Cart", shoppingCart);
        }

        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart") ?? new List<ShoppingCartItem>();
                var shoppingCartItem = shoppingCart.FirstOrDefault(sc => sc.DishId == id);
                if(shoppingCartItem == null)
                {
                    shoppingCart.Add(new ShoppingCartItem
                    {
                        Dish = await _context.Dishes.FindAsync(id),
                        DishId = (int)id
                    });
                }
                else
                {
                    shoppingCartItem.Count++;
                }
                HttpContext.Session.Set("shoppingCart", shoppingCart);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangeCart(int? dishId, int newCount)
        {
            if (dishId == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
                var shoppingCartItem = shoppingCart.FirstOrDefault(sc => sc.DishId == dishId);
                if(shoppingCartItem != null)
                    shoppingCartItem.Count = newCount;
                HttpContext.Session.Set("shoppingCart", shoppingCart);
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
                var shoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
                shoppingCart.RemoveAll(sc => sc.DishId == id);
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
            order.ShoppingCart = HttpContext.Session.Get<List<ShoppingCartItem>>("shoppingCart");
            order.ShoppingCart.ForEach(sc => sc.Dish = null);
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
