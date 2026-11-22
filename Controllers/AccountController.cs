using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationRestaurant.Models;
using WebApplicationRestaurant.Observability;
using WebApplicationRestaurant.ViewModels;

namespace WebApplicationRestaurant.Controllers;

public class AccountController(SignInManager<User> signInManager, ILogger<AccountController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                logger.UserLoginSucceeded(model.UserName);
                RestaurantTelemetry.AuthLogins.Add(1, new KeyValuePair<string, object?>("outcome", "succeeded"));

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            logger.UserLoginFailed(model.UserName, result.IsLockedOut, result.IsNotAllowed);
            RestaurantTelemetry.AuthLogins.Add(1, new KeyValuePair<string, object?>("outcome", "failed"));
            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name ?? "unknown";
        await signInManager.SignOutAsync();
        logger.UserLoggedOut(userName);
        return RedirectToAction("Index", "Home");
    }
}
