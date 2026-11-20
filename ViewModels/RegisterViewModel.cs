using System.ComponentModel.DataAnnotations;

namespace WebApplicationRestaurant.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Некорректный адрес")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Имя")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Фамилия")]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = string.Empty;
}
