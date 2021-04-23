using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.Controllers
{
    [Area("RestaurantAdministration")]
    public class MenuPlansController : Controller
    {
        private readonly ApplicationContext _context;

        public MenuPlansController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuPlans.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPlan = await _context.MenuPlans
                .Include(mp => mp.Dishes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuPlan == null)
            {
                return NotFound();
            }

            return View(menuPlan);
        }

        public IActionResult Create()
        {
            ViewData["Dishes"] = new MultiSelectList(_context.Dishes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dishes"] = new MultiSelectList(_context.Dishes, "Id", "Name", menuPlan.Dishes);
            return View(menuPlan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPlan = await _context.MenuPlans
                .Include(mp => mp.Dishes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuPlan == null)
            {
                return NotFound();
            }
            ViewData["Dishes"] = new MultiSelectList(_context.Dishes, "Id", "Name", menuPlan.Dishes);
            return View(menuPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuPlanExists(menuPlan.Id))
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
            ViewData["Dishes"] = new MultiSelectList(_context.Dishes, "Id", "Name", menuPlan.Dishes);
            return View(menuPlan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPlan = await _context.MenuPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuPlan == null)
            {
                return NotFound();
            }

            return View(menuPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuPlan = await _context.MenuPlans.FindAsync(id);
            _context.MenuPlans.Remove(menuPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuPlanExists(int id)
        {
            return _context.MenuPlans.Any(e => e.Id == id);
        }
    }
}
