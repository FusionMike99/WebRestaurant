using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.Controllers;

[Area("RestaurantAdministration")]
public class MenuPlansController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.MenuPlans.ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var menuPlan = await context.MenuPlans
            .Include(mp => mp.Dishes)
            .FirstOrDefaultAsync(m => m.Id == id);
        return menuPlan is null ? NotFound() : View(menuPlan);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
    {
        if (menuPlan.PlanStartDate > menuPlan.PlanEndDate)
        {
            ModelState.AddModelError("", "Дата початку не повина бути більшою ніж дата кінця");
        }

        SqlParameter paramStartDate = new("@StartDate", menuPlan.PlanStartDate);
        SqlParameter paramEndDate = new("@EndDate", menuPlan.PlanEndDate);
        SqlParameter paramResult = new()
        {
            ParameterName = "@Result",
            SqlDbType = SqlDbType.Bit,
            Direction = ParameterDirection.Output
        };
        await context.Database.ExecuteSqlRawAsync(
            "EXEC IsExistMenuPlan @StartDate, @EndDate, @Result OUTPUT",
            paramStartDate, paramEndDate, paramResult);
        var resultValue = paramResult.Value is bool b && b;
        if (resultValue)
        {
            ModelState.AddModelError("", "План меню перетинаєтся з існуючими планами");
        }

        if (ModelState.IsValid)
        {
            context.Add(menuPlan);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(EditMenuDishes), new { menuId = menuPlan.Id });
        }

        return View(menuPlan);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var menuPlan = await context.MenuPlans
            .Include(mp => mp.Dishes)
            .FirstOrDefaultAsync(m => m.Id == id);
        return menuPlan is null ? NotFound() : View(menuPlan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,PlanStartDate,PlanEndDate")] MenuPlan menuPlan)
    {
        if (menuPlan.PlanStartDate > menuPlan.PlanEndDate)
        {
            ModelState.AddModelError("", "Дата початку не повина бути більшою ніж дата кінця");
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(menuPlan);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuPlanExists(menuPlan.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(menuPlan);
    }

    [HttpGet]
    public async Task<IActionResult> EditMenuDishes(int menuId)
    {
        var menu = await context.MenuPlans.Include(m => m.Dishes)
            .FirstOrDefaultAsync(m => m.Id == menuId);
        if (menu is not null)
        {
            var menuItems = menu.Dishes.Select(i => i.Id).ToList();
            var allDishes = await context.Dishes.ToListAsync();
            MenuDishesViewModel model = new()
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
        var menu = await context.MenuPlans.Include(m => m.Dishes)
            .FirstOrDefaultAsync(m => m.Id == menuId);
        var dishes = await context.Dishes
            .Where(d => dishesId.Contains(d.Id)).ToListAsync();
        if (menu is not null)
        {
            var menuItems = menu.Dishes.ToList();
            var addedMenuItems = dishes.Except(menuItems);
            var removedMenuItems = menuItems.Except(dishes);

            menu.Dishes.AddRange(addedMenuItems);
            menu.Dishes.RemoveAll(a => removedMenuItems.Any(b => b.Id == a.Id));

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return NotFound();
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var menuPlan = await context.MenuPlans
            .FirstOrDefaultAsync(m => m.Id == id);
        return menuPlan is null ? NotFound() : View(menuPlan);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var menuPlan = await context.MenuPlans.FindAsync(id);
        if (menuPlan is not null)
        {
            context.MenuPlans.Remove(menuPlan);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool MenuPlanExists(int id) =>
        context.MenuPlans.Any(e => e.Id == id);
}
