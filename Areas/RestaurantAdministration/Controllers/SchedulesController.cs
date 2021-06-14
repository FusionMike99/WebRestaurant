using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.RestaurantAdministration.ViewModels;
using WebApplicationRestaurant.Data;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.RestaurantAdministration.Controllers
{
    [Area("RestaurantAdministration")]
    public class SchedulesController : Controller
    {
        private readonly ApplicationContext _context;

        public SchedulesController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Schedules.Include(s => s.User);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.User)
                .Include(s => s.Places)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkingDate,UserId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditSchedulePlaces), new { scheduleId = schedule.Id });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", schedule.UserId);
            return View(schedule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", schedule.UserId);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,WorkingDate,UserId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", schedule.UserId);
            return View(schedule);
        }

        [HttpGet]
        public async Task<IActionResult> EditSchedulePlaces(int scheduleId)
        {
            // получаем график
            var schedule = await _context.Schedules.Include(m => m.Places)
                .FirstOrDefaultAsync(m => m.Id == scheduleId);
            if (schedule != null)
            {
                // получем столики графика
                var scheduleItems = schedule.Places.Select(i => i.Id).ToList();
                // получаем все столики
                var allPlaces = _context.Places.ToList();
                SchedulePlacesViewModel model = new SchedulePlacesViewModel
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
            // получаем меню
            var schedule = await _context.Schedules.Include(m => m.Places)
                .FirstOrDefaultAsync(m => m.Id == scheduleId);
            var places = _context.Places.
                Where(d => placesId.Contains(d.Id)).ToList();
            if (schedule != null)
            {
                // получем столики графика
                var scheduleItems = schedule.Places.ToList();
                // получаем все столики
                var allPlaces = _context.Places.ToList();
                // получаем элементы меню, которые были добавлены
                var addedScheduleItems = places.Except(scheduleItems);
                // получаем элементы меню, которые были удалены
                var removedScheduleItems = scheduleItems.Except(places);

                schedule.Places.AddRange(addedScheduleItems);
                schedule.Places.RemoveAll(a => removedScheduleItems.Any(b => b.Id == a.Id));

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

            var schedule = await _context.Schedules
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
