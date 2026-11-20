using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class IngredientUnitsController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.IngredientUnits.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] IngredientUnit ingredientUnit)
    {
        if (ModelState.IsValid)
        {
            context.Add(ingredientUnit);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ingredientUnit);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ingredientUnit = await context.IngredientUnits.FindAsync(id);
        return ingredientUnit is null ? NotFound() : View(ingredientUnit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name")] IngredientUnit ingredientUnit)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(ingredientUnit);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientUnitExists(ingredientUnit.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(ingredientUnit);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ingredientUnit = await context.IngredientUnits
            .FirstOrDefaultAsync(m => m.Id == id);
        return ingredientUnit is null ? NotFound() : View(ingredientUnit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ingredientUnit = await context.IngredientUnits.FindAsync(id);
        if (ingredientUnit is not null)
        {
            context.IngredientUnits.Remove(ingredientUnit);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool IngredientUnitExists(int id) =>
        context.IngredientUnits.Any(e => e.Id == id);
}
