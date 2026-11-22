using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.Administration.ViewModels;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Observability;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class UsersController(UserManager<User> userManager, ILogger<UsersController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index() => View(await userManager.Users.ToListAsync());

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Surname = model.Surname
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                logger.UserCreated(user.Id, model.Email);
                RestaurantTelemetry.AdminUserEvents.Add(1, new KeyValuePair<string, object?>("action", "user_created"));
                return RedirectToAction("Index");
            }

            logger.UserCreateFailed(model.Email, string.Join("; ", result.Errors.Select(e => e.Description)));
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        EditUserViewModel model = new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            Name = user.Name,
            Surname = user.Surname
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user is not null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    logger.UserUpdated(user.Id, model.Email);
                    RestaurantTelemetry.AdminUserEvents.Add(1, new KeyValuePair<string, object?>("action", "user_updated"));
                    return RedirectToAction("Index");
                }

                logger.UserUpdateFailed(user.Id, string.Join("; ", result.Errors.Select(e => e.Description)));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Delete(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is not null)
        {
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                logger.UserDeleted(user.Id);
                RestaurantTelemetry.AdminUserEvents.Add(1, new KeyValuePair<string, object?>("action", "user_deleted"));
            }
            else
            {
                logger.UserDeleteFailed(user.Id, string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        ChangePasswordViewModel model = new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user is not null)
            {
                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    logger.UserPasswordChanged(user.Id);
                    RestaurantTelemetry.AdminUserEvents.Add(1, new KeyValuePair<string, object?>("action", "password_changed"));
                    return RedirectToAction("Index");
                }

                logger.UserPasswordChangeFailed(user.Id, string.Join("; ", result.Errors.Select(e => e.Description)));
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
        }

        return View(model);
    }
}
