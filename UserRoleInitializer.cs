using WebApplicationRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplicationRestaurant
{
    public class UserRoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            /*List<string> roleNames = new List<string>() { "adminIS", "adminRes", "waiter", "cooker" };
            List<string> userNames = new List<string>(){ "adminIS1", "adminRes1", "waiter1", "cooker1" };
            List<string> userEmail = new List<string>(){ "adminIS1@mail.net", "adminRes1@mail.net",
                "waiter1@mail.net", "cooker1@mail.net" };
            List<string> userPasswords = new List<string>(){ "admin", "admin", "waiter", "cooker" };*/
            //Кортеж состоящий из (имя_пользователя, email, пароль, роль)
            List<(string, string, string, string)> users = new List<(string, string, string, string)> 
            { ("adminIS1", "adminIS1@mail.net", "admin", "adminIS"), ("adminRes1", "adminRes1@mail.net", "admin", "adminRes"),
                ("waiter1", "waiter1@mail.net", "waiter", "waiter"), ("cooker1", "cooker1@mail.net", "cooker", "cooker") };
           
            /*foreach(var item in roleNames)
            {
                if (await roleManager.FindByNameAsync(item) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
            for (int i = 0; i < userNames.Capacity; i++)
            {
                if (await userManager.FindByNameAsync(userNames[i]) == null)
                {
                    User user = new User { Email = userEmail[i], UserName = userNames[i] };
                    IdentityResult result = await userManager.CreateAsync(user, userPasswords[i]);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roleNames[i]);
                    }
                }
            }*/
            foreach(var item in users)
            {
                if (await roleManager.FindByNameAsync(item.Item4) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(item.Item4));
                }
                if (await userManager.FindByNameAsync(item.Item1) == null)
                {
                    User user = new User { Email = item.Item2, UserName = item.Item1 };
                    IdentityResult result = await userManager.CreateAsync(user, item.Item3);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, item.Item4);
                    }
                }
            }
        }
    }
}
