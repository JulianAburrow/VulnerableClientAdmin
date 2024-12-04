namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class RegisteredModel : PageModel
{
    private SignInManager<IdentityUser> _signInManager;

    public RegisteredModel(SignInManager<IdentityUser> signInManager) =>
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
