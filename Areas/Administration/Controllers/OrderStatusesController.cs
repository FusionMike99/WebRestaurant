using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class OrderStatusesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.OrderStatuses.ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var orderStatus = await context.OrderStatuses
            .FirstOrDefaultAsync(m => m.Id == id);
        return orderStatus is null ? NotFound() : View(orderStatus);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] OrderStatus orderStatus)
    {
        if (ModelState.IsValid)
        {
            context.Add(orderStatus);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(orderStatus);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var orderStatus = await context.OrderStatuses.FindAsync(id);
        return orderStatus is null ? NotFound() : View(orderStatus);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Name")] OrderStatus orderStatus)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(orderStatus);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(orderStatus.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(orderStatus);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var orderStatus = await context.OrderStatuses
            .FirstOrDefaultAsync(m => m.Id == id);
        return orderStatus is null ? NotFound() : View(orderStatus);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var orderStatus = await context.OrderStatuses.FindAsync(id);
        if (orderStatus is not null)
        {
            context.OrderStatuses.Remove(orderStatus);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool OrderStatusExists(int id) =>
        context.OrderStatuses.Any(e => e.Id == id);
}
