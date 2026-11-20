using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class IngredientsController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var applicationContext = context.Ingredients.Include(i => i.IngredientUnit);
        return View(await applicationContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ingredient = await context.Ingredients
            .Include(i => i.IngredientUnit)
            .FirstOrDefaultAsync(m => m.Id == id);
        return ingredient is null ? NotFound() : View(ingredient);
    }

    public IActionResult Create()
    {
        ViewData["IngredientUnitId"] = new SelectList(context.IngredientUnits, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Count,DeliveryDate,IngredientUnitId")] Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            context.Add(ingredient);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["IngredientUnitId"] = new SelectList(context.IngredientUnits, "Id", "Name", ingredient.IngredientUnitId);
        return View(ingredient);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient is null)
        {
            return NotFound();
        }

        ViewData["IngredientUnitId"] = new SelectList(context.IngredientUnits, "Id", "Name", ingredient.IngredientUnitId);
        return View(ingredient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name,Count,DeliveryDate,IngredientUnitId")] Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(ingredient);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(ingredient.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["IngredientUnitId"] = new SelectList(context.IngredientUnits, "Id", "Name", ingredient.IngredientUnitId);
        return View(ingredient);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var ingredient = await context.Ingredients
            .Include(i => i.IngredientUnit)
            .FirstOrDefaultAsync(m => m.Id == id);
        return ingredient is null ? NotFound() : View(ingredient);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient is not null)
        {
            context.Ingredients.Remove(ingredient);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool IngredientExists(int id) =>
        context.Ingredients.Any(e => e.Id == id);
}
