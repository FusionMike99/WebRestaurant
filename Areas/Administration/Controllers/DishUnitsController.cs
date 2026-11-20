using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class DishUnitsController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.DishUnits.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] DishUnit dishUnit)
    {
        if (ModelState.IsValid)
        {
            context.Add(dishUnit);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(dishUnit);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dishUnit = await context.DishUnits.FindAsync(id);
        return dishUnit is null ? NotFound() : View(dishUnit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name")] DishUnit dishUnit)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(dishUnit);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishUnitExists(dishUnit.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(dishUnit);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dishUnit = await context.DishUnits
            .FirstOrDefaultAsync(m => m.Id == id);
        return dishUnit is null ? NotFound() : View(dishUnit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dishUnit = await context.DishUnits.FindAsync(id);
        if (dishUnit is not null)
        {
            context.DishUnits.Remove(dishUnit);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DishUnitExists(int id) =>
        context.DishUnits.Any(e => e.Id == id);
}
