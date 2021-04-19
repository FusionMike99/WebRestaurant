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
    public class DishUnitsController : Controller
    {
        private readonly ApplicationContext _context;

        public DishUnitsController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DishUnits.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DishUnit dishUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishUnit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishUnit = await _context.DishUnits.FindAsync(id);
            if (dishUnit == null)
            {
                return NotFound();
            }
            return View(dishUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] DishUnit dishUnit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishUnitExists(dishUnit.Id))
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
            return View(dishUnit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishUnit = await _context.DishUnits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishUnit == null)
            {
                return NotFound();
            }

            return View(dishUnit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishUnit = await _context.DishUnits.FindAsync(id);
            _context.DishUnits.Remove(dishUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishUnitExists(int id)
        {
            return _context.DishUnits.Any(e => e.Id == id);
        }
    }
}
