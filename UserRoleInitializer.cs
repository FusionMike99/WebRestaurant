using WebApplicationRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplicationRestaurant
{
    public static class UserRoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Кортеж состоящий из (имя_пользователя, email, пароль, роль)
            List<(string, string, string, string)> users = new List<(string, string, string, string)> 
            { ("adminIS1", "adminIS1@mail.net", "admin", "adminIS"), ("adminRes1", "adminRes1@mail.net", "admin", "adminRes"),
                ("waiter1", "waiter1@mail.net", "waiter", "waiter"), ("cooker1", "cooker1@mail.net", "cooker", "cooker") };

            List<(string, string)> userNameSurname = new List<(string, string)> { ("Админ", "Админов"), ("Админ", "Админов"),
                ("Вайтер", "Вайтеров"), ("Кукер", "Кукеров") };

            for (int i = 0; i < users.Count; i++)
            {
                if (await roleManager.FindByNameAsync(users[i].Item4) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(users[i].Item4));
                }
                if (await userManager.FindByNameAsync(users[i].Item1) == null)
                {
                    User user = new User { Email = users[i].Item2, UserName = users[i].Item1,
                        Name = userNameSurname[i].Item1, Surname = userNameSurname[i].Item2 };
                    IdentityResult result = await userManager.CreateAsync(user, users[i].Item3);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, users[i].Item4);
                    }
                }
            }
        }
    }
}
