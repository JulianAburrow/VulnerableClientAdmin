namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    private SignInManager<IdentityUser> _signInManager { get; set; } = null!;

    public LogoutModel(SignInManager<IdentityUser> signInManager) =>
        _signInManager = signInManager;

    public async Task<IActionResult> OnPost()
    {
        if (_signInManager.IsSignedIn(User))
        {
            await _signInManager.SignOutAsync();
        }

        return Redirect("~/Identity/Account/Login");
    }
}
