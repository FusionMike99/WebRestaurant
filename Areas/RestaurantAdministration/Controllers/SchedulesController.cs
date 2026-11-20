using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.Controllers;

[Area("RestaurantAdministration")]
public class SchedulesController(ApplicationContext context, UserManager<User> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var applicationContext = context.Schedules.Include(s => s.User);
        return View(await applicationContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var schedule = await context.Schedules
            .Include(s => s.User)
            .Include(s => s.Places)
            .FirstOrDefaultAsync(m => m.Id == id);
        return schedule is null ? NotFound() : View(schedule);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["UserId"] = new SelectList(await userManager.GetUsersInRoleAsync("waiter"), "Id", "Surname");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,WorkingDate,UserId")] Schedule schedule)
    {
        if (context.Schedules.Any(s => s.WorkingDate == schedule.WorkingDate && s.UserId == schedule.UserId))
        {
            ModelState.AddModelError("", $"Для даного співробітника вже існує графік на {schedule.WorkingDate.ToShortDateString()}");
        }

        if (ModelState.IsValid)
        {
            context.Add(schedule);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(EditSchedulePlaces), new { scheduleId = schedule.Id });
        }

        ViewData["UserId"] = new SelectList(await userManager.GetUsersInRoleAsync("waiter"), "Id", "Surname");
        return View(schedule);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var schedule = await context.Schedules.FindAsync(id);
        if (schedule is null)
        {
            return NotFound();
        }

        ViewData["UserId"] = new SelectList(await userManager.GetUsersInRoleAsync("waiter"), "Id", "Surname", schedule.UserId);
        return View(schedule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id,WorkingDate,UserId")] Schedule schedule)
    {
        if (context.Schedules.Any(s => s.WorkingDate == schedule.WorkingDate && s.UserId == schedule.UserId))
        {
            ModelState.AddModelError("", $"Для даного співробітника вже існує графік на {schedule.WorkingDate.ToShortDateString()}");
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(schedule);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["UserId"] = new SelectList(await userManager.GetUsersInRoleAsync("waiter"), "Id", "Surname", schedule.UserId);
        return View(schedule);
    }

    [HttpGet]
    public async Task<IActionResult> EditSchedulePlaces(int scheduleId)
    {
        var schedule = await context.Schedules.Include(m => m.Places)
            .FirstOrDefaultAsync(m => m.Id == scheduleId);
        if (schedule is not null)
        {
            var scheduleItems = schedule.Places.Select(i => i.Id).ToList();
            var allPlaces = await context.Places.ToListAsync();
            SchedulePlacesViewModel model = new()
            {
                ScheduleId = schedule.Id,
                ScheduleItems = scheduleItems,
                Places = allPlaces
            };
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> EditSchedulePlaces(int scheduleId, List<int> placesId)
    {
        var schedule = await context.Schedules.Include(m => m.Places)
            .FirstOrDefaultAsync(m => m.Id == scheduleId);
        var places = await context.Places
            .Where(d => placesId.Contains(d.Id)).ToListAsync();
        if (schedule is not null)
        {
            var scheduleItems = schedule.Places.ToList();
            var addedScheduleItems = places.Except(scheduleItems);
            var removedScheduleItems = scheduleItems.Except(places);

            schedule.Places.AddRange(addedScheduleItems);
            schedule.Places.RemoveAll(a => removedScheduleItems.Any(b => b.Id == a.Id));

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

        var schedule = await context.Schedules
            .Include(s => s.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        return schedule is null ? NotFound() : View(schedule);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schedule = await context.Schedules.FindAsync(id);
        if (schedule is not null)
        {
            context.Schedules.Remove(schedule);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ScheduleExists(int id) =>
        context.Schedules.Any(e => e.Id == id);
}
