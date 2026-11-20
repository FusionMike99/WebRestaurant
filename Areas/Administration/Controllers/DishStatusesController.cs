using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class DishStatusesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.DishStatuses.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] DishStatus dishStatus)
    {
        if (ModelState.IsValid)
        {
            context.Add(dishStatus);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(dishStatus);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dishStatus = await context.DishStatuses.FindAsync(id);
        return dishStatus is null ? NotFound() : View(dishStatus);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name")] DishStatus dishStatus)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(dishStatus);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishStatusExists(dishStatus.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(dishStatus);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var dishStatus = await context.DishStatuses
            .FirstOrDefaultAsync(m => m.Id == id);
        return dishStatus is null ? NotFound() : View(dishStatus);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dishStatus = await context.DishStatuses.FindAsync(id);
        if (dishStatus is not null)
        {
            context.DishStatuses.Remove(dishStatus);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DishStatusExists(int id) =>
        context.DishStatuses.Any(e => e.Id == id);
}
