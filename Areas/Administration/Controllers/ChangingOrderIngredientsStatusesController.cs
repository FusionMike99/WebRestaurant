﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationRestaurant.Data;

namespace WebApplicationRestaurant.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class ChangingOrderIngredientsStatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public ChangingOrderIngredientsStatusesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.OrderIngredients.Include(o => o.Status)
                .Include(o => o.User).Include(o => o.OrderIngredientsItems);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderIngredients
                .Include(o => o.Status)
                .Include(o => o.User)
                .Include(o => o.OrderIngredientsItems)
                    .ThenInclude(sc => sc.Ingredient)
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

            var order = await _context.OrderIngredients.FindAsync(id);
            order.StatusId = 2;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderIngredientsExists(order.Id))
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

        private bool OrderIngredientsExists(int id)
        {
            return _context.OrderIngredients.Any(e => e.Id == id);
        }
    }
}
