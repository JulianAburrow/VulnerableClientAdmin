namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account.Manage;

public class UserDetailsModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserDetailsModel(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

    public UserModel UserDetails { get; set; } = new();

    public async Task<IActionResult> OnGet(string id)
    {
        UserDetails.User = await _userManager.FindByIdAsync(id);
        UserDetails.CurrentRoles = await _userManager.GetRolesAsync(UserDetails.User);
        return Page();
    }
}
