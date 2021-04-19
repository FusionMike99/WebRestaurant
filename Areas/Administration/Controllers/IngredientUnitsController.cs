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
    public class IngredientUnitsController : Controller
    {
        private readonly ApplicationContext _context;

        public IngredientUnitsController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.IngredientUnits.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] IngredientUnit ingredientUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredientUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientUnit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientUnit = await _context.IngredientUnits.FindAsync(id);
            if (ingredientUnit == null)
            {
                return NotFound();
            }
            return View(ingredientUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] IngredientUnit ingredientUnit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredientUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientUnitExists(ingredientUnit.Id))
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
            return View(ingredientUnit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientUnit = await _context.IngredientUnits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingredientUnit == null)
            {
                return NotFound();
            }

            return View(ingredientUnit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientUnit = await _context.IngredientUnits.FindAsync(id);
            _context.IngredientUnits.Remove(ingredientUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientUnitExists(int id)
        {
            return _context.IngredientUnits.Any(e => e.Id == id);
        }
    }
}
