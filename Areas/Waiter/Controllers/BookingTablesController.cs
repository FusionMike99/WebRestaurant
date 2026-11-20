using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Waiter.Controllers;

[Area("Waiter")]
[Authorize(Roles = "waiter")]
public class BookingTablesController(ApplicationContext context) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await context.Places.ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var place = await context.Places.FirstOrDefaultAsync(m => m.Id == id);
        return place is null ? NotFound() : View(place);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Number")] Place place)
    {
        if (ModelState.IsValid)
        {
            context.Add(place);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(place);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var place = await context.Places.FindAsync(id);
        return place is null ? NotFound() : View(place);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,Number,Available")] Place place)
    {
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(place);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(place.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(place);
    }

    public async Task<IActionResult> BookTable(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var place = await context.Places.FindAsync(id);
        return place is null ? NotFound() : View(place);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookTable([Bind("Id,Number,ReserverName,ReserveTime")] Place place)
    {
        if (ModelState.IsValid)
        {
            try
            {
                place.Available = false;
                context.Update(place);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(place.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(place);
    }

    public async Task<IActionResult> UnbookTable(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var place = await context.Places.FindAsync(id);
        if (place is null)
        {
            return NotFound();
        }

        place.Available = true;
        place.ReserverName = null;
        place.ReserveTime = null;

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(place);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(place.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(place);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var place = await context.Places.FirstOrDefaultAsync(m => m.Id == id);
        return place is null ? NotFound() : View(place);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var place = await context.Places.FindAsync(id);
        if (place is not null)
        {
            context.Places.Remove(place);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PlaceExists(int id) =>
        context.Places.Any(e => e.Id == id);
}
