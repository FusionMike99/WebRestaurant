using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class OrderIngredientsStatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public OrderIngredientsStatusesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderIngredientsStatuses.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] OrderIngredientsStatus orderIngredientsStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderIngredientsStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderIngredientsStatus);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderIngredientsStatus = await _context.OrderIngredientsStatuses.FindAsync(id);
            if (orderIngredientsStatus == null)
            {
                return NotFound();
            }
            return View(orderIngredientsStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] OrderIngredientsStatus orderIngredientsStatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderIngredientsStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderIngredientsStatusExists(orderIngredientsStatus.Id))
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
            return View(orderIngredientsStatus);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderIngredientsStatus = await _context.OrderIngredientsStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderIngredientsStatus == null)
            {
                return NotFound();
            }

            return View(orderIngredientsStatus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderIngredientsStatus = await _context.OrderIngredientsStatuses.FindAsync(id);
            _context.OrderIngredientsStatuses.Remove(orderIngredientsStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderIngredientsStatusExists(int id)
        {
            return _context.OrderIngredientsStatuses.Any(e => e.Id == id);
        }
    }
}
