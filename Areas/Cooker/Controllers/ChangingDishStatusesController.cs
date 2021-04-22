using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Cooker.Controllers
{
    [Area("Cooker")]
    public class ChangingDishStatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public ChangingDishStatusesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ShoppingCartItem.Where(sci => !sci.DishStatusId.Equals(3)).ToListAsync());
        }

        public async Task<IActionResult> ChangeStatus(int? idOrder, int? idDish, int idStatus)
        {
            try
            {
                var shoppingCartItem = await _context.ShoppingCartItem.FindAsync(idOrder, idDish);
                shoppingCartItem.DishStatusId = idStatus;
                _context.Update(shoppingCartItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartItemExists(idOrder, idDish))
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

        private bool ShoppingCartItemExists(int? idOrder, int? idDish)
        {
            return _context.ShoppingCartItem.Any(e => e.OrderId == idOrder && e.DishId == idDish);
        }
    }
}
