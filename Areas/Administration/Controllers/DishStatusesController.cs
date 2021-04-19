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
    public class DishStatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public DishStatusesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DishStatuses.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DishStatus dishStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishStatus);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishStatus = await _context.DishStatuses.FindAsync(id);
            if (dishStatus == null)
            {
                return NotFound();
            }
            return View(dishStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] DishStatus dishStatus)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishStatusExists(dishStatus.Id))
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
            return View(dishStatus);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishStatus = await _context.DishStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishStatus == null)
            {
                return NotFound();
            }

            return View(dishStatus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishStatus = await _context.DishStatuses.FindAsync(id);
            _context.DishStatuses.Remove(dishStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishStatusExists(int id)
        {
            return _context.DishStatuses.Any(e => e.Id == id);
        }
    }
}
