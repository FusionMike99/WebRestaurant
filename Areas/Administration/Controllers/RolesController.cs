using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationRestaurant.Areas.Administration.ViewModels;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant.Areas.Administration.Controllers;

[Area("Administration")]
public class RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : Controller
{
    public async Task<IActionResult> Index() => View(await roleManager.Roles.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var result = await roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(name);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is not null)
        {
            await roleManager.DeleteAsync(role);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UserList() => View(await userManager.Users.ToListAsync());

    [HttpGet]
    public async Task<IActionResult> Edit(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = await roleManager.Roles.ToListAsync();
            ChangeRoleViewModel model = new()
            {
                UserId = user.Id,
                UserEmail = user.Email ?? string.Empty,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);

            await userManager.AddToRolesAsync(user, addedRoles);
            await userManager.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("Index", "Users");
        }

        return NotFound();
    }
}
