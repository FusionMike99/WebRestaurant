using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
        {
            if (menuPlan.PlanStartDate > menuPlan.PlanEndDate)
                ModelState.AddModelError("", "Дата початку не повина бути більшою ніж дата кінця");

            var paramStartDate = new SqlParameter("@StartDate", menuPlan.PlanStartDate);
            var paramEndDate = new SqlParameter("@EndDate", menuPlan.PlanEndDate);
            var paramResult = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Output
            };
            _context.Database.ExecuteSqlRaw("EXEC IsExistMenuPlan @StartDate, @EndDate, @Result OUTPUT", paramStartDate, paramEndDate, paramResult);
            var resultValue = (bool) paramResult.Value;
            if(resultValue)
                ModelState.AddModelError("", "План меню перетинаєтся з існуючими планами");

            if (ModelState.IsValid)
            {
                _context.Add(menuPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditMenuDishes), new { menuId = menuPlan.Id });
            }
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
            return View(menuPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
        {
            if (menuPlan.PlanStartDate > menuPlan.PlanEndDate)
                ModelState.AddModelError("", "Дата початку не повина бути більшою ніж дата кінця");

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
            return View(menuPlan);
        }

        [HttpGet]
        public async Task<IActionResult> EditMenuDishes(int menuId)
        {
            // получаем меню
            var menu = await _context.MenuPlans.Include(m => m.Dishes)
                .FirstOrDefaultAsync(m => m.Id == menuId);
            if (menu != null)
            {
                // получем элементы меню
                var menuItems = menu.Dishes.Select(i => i.Id).ToList();
                // получаем все блюда
                var allDishes = _context.Dishes.ToList();
                MenuDishesViewModel model = new MenuDishesViewModel
                {
                    MenuId = menu.Id,
                    MenuItems = menuItems,
                    Dishes = allDishes
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuDishes(int menuId, List<int> dishesId)
        {
            // получаем меню
            var menu = await _context.MenuPlans.Include(m => m.Dishes)
                .FirstOrDefaultAsync(m => m.Id == menuId);
            var dishes = _context.Dishes.
                Where(d => dishesId.Contains(d.Id)).ToList();
            if (menu != null)
            {
                // получем элементы меню
                var menuItems = menu.Dishes.ToList();
                // получаем все блюда
                var allDishes = _context.Dishes.ToList();
                // получаем элементы меню, которые были добавлены
                var addedMenuItems = dishes.Except(menuItems);
                // получаем элементы меню, которые были удалены
                var removedMenuItems = menuItems.Except(dishes);

                menu.Dishes.AddRange(addedMenuItems);
                menu.Dishes.RemoveAll(a => removedMenuItems.Any(b => b.Id == a.Id));

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return NotFound();
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
