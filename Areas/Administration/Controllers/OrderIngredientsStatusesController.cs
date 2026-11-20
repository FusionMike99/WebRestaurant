using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class OrderIngredientsStatusesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.OrderIngredientsStatuses.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] OrderIngredientsStatus orderIngredientsStatus)
    {
        if (ModelState.IsValid)
        {
            context.Add(orderIngredientsStatus);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(orderIngredientsStatus);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var orderIngredientsStatus = await context.OrderIngredientsStatuses.FindAsync(id);
        return orderIngredientsStatus is null ? NotFound() : View(orderIngredientsStatus);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name")] OrderIngredientsStatus orderIngredientsStatus)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(orderIngredientsStatus);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderIngredientsStatusExists(orderIngredientsStatus.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(orderIngredientsStatus);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var orderIngredientsStatus = await context.OrderIngredientsStatuses
            .FirstOrDefaultAsync(m => m.Id == id);
        return orderIngredientsStatus is null ? NotFound() : View(orderIngredientsStatus);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var orderIngredientsStatus = await context.OrderIngredientsStatuses.FindAsync(id);
        if (orderIngredientsStatus is not null)
        {
            context.OrderIngredientsStatuses.Remove(orderIngredientsStatus);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool OrderIngredientsStatusExists(int id) =>
        context.OrderIngredientsStatuses.Any(e => e.Id == id);
}
