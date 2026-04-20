namespace VulnerableClientAdminUI.Models;

public class UserDisplayModel
{
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    public string Password { get; set; } = default!;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = default!;

    [NotPlaceholder]
    public string Role { get; set; } = default!;
}
