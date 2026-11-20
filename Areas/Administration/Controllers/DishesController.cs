using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.Administration.ViewModels;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class DishesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var applicationContext = context.Dishes.Include(d => d.Category).Include(d => d.DishUnit);
        return View(await applicationContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dish = await context.Dishes
            .Include(d => d.Category)
            .Include(d => d.DishUnit)
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(m => m.Id == id);
        return dish is null ? NotFound() : View(dish);
    }

    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name");
        ViewData["DishUnitId"] = new SelectList(context.DishUnits, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Weight,Cost,CategoryId,DishUnitId")] Dish dish)
    {
        if (ModelState.IsValid)
        {
            context.Dishes.Add(dish);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(EditRecipe), new { dishId = dish.Id });
        }

        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", dish.CategoryId);
        ViewData["DishUnitId"] = new SelectList(context.DishUnits, "Id", "Name", dish.DishUnitId);
        return View(dish);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dish = await context.Dishes.FindAsync(id);
        if (dish is null)
        {
            return NotFound();
        }

        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", dish.CategoryId);
        ViewData["DishUnitId"] = new SelectList(context.DishUnits, "Id", "Name", dish.DishUnitId);
        return View(dish);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name,Weight,Cost,CategoryId,DishUnitId")] Dish dish)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(dish);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(dish.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", dish.CategoryId);
        ViewData["DishUnitId"] = new SelectList(context.DishUnits, "Id", "Name", dish.DishUnitId);
        return View(dish);
    }

    [HttpGet]
    public async Task<IActionResult> EditRecipe(int dishId)
    {
        var dish = await context.Dishes.Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish is not null)
        {
            var dishRecipe = dish.Ingredients.Select(i => i.Id).ToList();
            var allIngredients = await context.Ingredients.ToListAsync();
            DishRecipeViewModel model = new()
            {
                DishId = dish.Id,
                Recipe = dishRecipe,
                Ingredients = allIngredients
            };
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> EditRecipe(int dishId, List<int> ingredientsId)
    {
        var dish = await context.Dishes.Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == dishId);
        var ingredients = await context.Ingredients
            .Where(i => ingredientsId.Contains(i.Id)).ToListAsync();
        if (dish is not null)
        {
            var dishRecipe = dish.Ingredients.ToList();
            var addedRecipe = ingredients.Except(dishRecipe);
            var removedRecipe = dishRecipe.Except(ingredients);

            dish.Ingredients.AddRange(addedRecipe);
            dish.Ingredients.RemoveAll(a => removedRecipe.Any(b => b.Id == a.Id));

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

        var dish = await context.Dishes
            .Include(d => d.Category)
            .Include(d => d.DishUnit)
            .FirstOrDefaultAsync(m => m.Id == id);
        return dish is null ? NotFound() : View(dish);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dish = await context.Dishes.FindAsync(id);
        if (dish is not null)
        {
            context.Dishes.Remove(dish);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DishExists(int id) =>
        context.Dishes.Any(e => e.Id == id);
}
