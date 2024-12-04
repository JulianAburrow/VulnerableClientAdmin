namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class ChangePasswordModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public ChangePasswordModel(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

    [BindProperty]
    public ChangePasswordInputModel ChangePasswordInput { get; set; } = null!;

    public class ChangePasswordInputModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (User.Identity == null)
        {
            ViewData[CommonValues.ErrorMessage] = "An error occurred trying to change the password.";
            return Page();
        }
        var user = await _userManager.FindByNameAsync(User.Identity?.Name);
        var result = await _userManager.ChangePasswordAsync(user, ChangePasswordInput.CurrentPassword, ChangePasswordInput.NewPassword);

        if (result.Succeeded)
        {
            ViewData[CommonValues.SuccessMessage] = "Password change successful";
            return Page();
        }

        ViewData[CommonValues.ErrorMessage] = "Password change unsuccessful. Please check your entries and try again.";
        return Page();
    }
}
