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
    public class ChangingOrderStatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public ChangingOrderStatusesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationContext = _context.Orders.Where(o => o.UserId.Equals(userId)).Include(o => o.Place)
                .Include(o => o.Status).Include(o => o.User).Include(o => o.ShoppingCart);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Place)
                .Include(o => o.Status)
                .Include(o => o.User)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.Dish)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.DishStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> CloseOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            order.StatusId = 2;
            order.FinishTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
