using Microsoft.AspNetCore.Identity;

namespace WebApplicationRestaurant.Areas.Administration.ViewModels;

public class ChangeRoleViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<IdentityRole> AllRoles { get; set; } = [];
    public IList<string> UserRoles { get; set; } = [];
}
