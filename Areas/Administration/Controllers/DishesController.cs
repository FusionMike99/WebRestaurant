using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.Administration.ViewModels;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class DishesController : Controller
    {
        private readonly ApplicationContext _context;

        public DishesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Dishes.Include(d => d.Category).Include(d => d.DishUnit);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.DishUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["DishUnitId"] = new SelectList(_context.DishUnits, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Weight,Cost,CategoryId,DishUnitId")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Dishes.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditRecipe), new { dishId = dish.Id });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dish.CategoryId);
            ViewData["DishUnitId"] = new SelectList(_context.DishUnits, "Id", "Name", dish.DishUnitId);
            return View(dish);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dish.CategoryId);
            ViewData["DishUnitId"] = new SelectList(_context.DishUnits, "Id", "Name", dish.DishUnitId);
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
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", dish.CategoryId);
            ViewData["DishUnitId"] = new SelectList(_context.DishUnits, "Id", "Name", dish.DishUnitId);
            return View(dish);
        }

        [HttpGet]
        public async Task<IActionResult> EditRecipe(int dishId)
        {
            // получаем блюдо
            var dish = await _context.Dishes.Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish != null)
            {
                // получем рецепт блюда
                var dishRecipe = dish.Ingredients.Select(i => i.Id).ToList();
                // получаем все ингредиенты
                var allIngredients = _context.Ingredients.ToList();
                DishRecipeViewModel model = new DishRecipeViewModel
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
            // получаем блюдо
            var dish = await _context.Dishes.Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == dishId);
            var ingredients = _context.Ingredients.
                Where(i => ingredientsId.Contains(i.Id)).ToList();
            if (dish != null)
            {
                // получем рецепт блюда
                var dishRecipe = dish.Ingredients/*.Select(i => i.Id)*/.ToList();
                // получаем все ингредиенты
                var allIngredients = _context.Ingredients.ToList();
                // получаем элементы рецепта, которые были добавлены
                var addedRecipe = ingredients.Except(dishRecipe);
                // получаем элементы рецепта, которые были удалены
                var removedRecipe = dishRecipe.Except(ingredients);

                dish.Ingredients.AddRange(addedRecipe);
                dish.Ingredients.RemoveAll(a => removedRecipe.Any(b => b.Id == a.Id));

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

            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.DishUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
