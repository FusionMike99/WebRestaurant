using Microsoft.AspNetCore.Identity;
using WebApplicationRestaurant.Models;

namespace WebApplicationRestaurant;

public static class UserRoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        (string UserName, string Email, string Password, string Role, string Name, string Surname)[] users =
        [
            ("adminIS1", "adminIS1@mail.net", "admin", "adminIS", "Админ", "Админов"),
            ("adminRes1", "adminRes1@mail.net", "admin", "adminRes", "Админ", "Админов"),
            ("waiter1", "waiter1@mail.net", "waiter", "waiter", "Вайтер", "Вайтеров"),
            ("cooker1", "cooker1@mail.net", "cooker", "cooker", "Кукер", "Кукеров")
        ];

        foreach (var (userName, email, password, role, name, surname) in users)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await userManager.FindByNameAsync(userName) is null)
            {
                User user = new()
                {
                    Email = email,
                    UserName = userName,
                    Name = name,
                    Surname = surname
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
