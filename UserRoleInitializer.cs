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
            List<string> roleNames = new List<string>() { "adminIS", "adminRes", "waiter", "cooker" };
            List<string> userNames = new List<string>(){ "adminIS1", "adminRes1", "waiter1", "cooker1" };
            List<string> userEmail = new List<string>(){ "adminIS1@mail.net", "adminRes1@mail.net",
                "waiter1@mail.net", "cooker1@mail.net" };
            List<string> userPasswords = new List<string>(){ "admin", "admin", "waiter", "cooker" };
            foreach(var item in roleNames)
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
            }
        }
    }
}
