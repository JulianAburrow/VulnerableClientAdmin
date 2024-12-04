namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account.Manage;

public class DeleteUserModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public DeleteUserModel(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

    public UserModel UserDetails { get; set; } = new();

    public async Task<IActionResult> OnGet(string id)
    {
        await PopulateUserDetails(id);
        return Page();
    }

    public async Task OnPost(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            ViewData[CommonValues.ErrorMessage] = "User not found";
            await PopulateUserDetails(id);
            return;
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        try
        {
            if (userRoles.Any())
            {
                foreach (var userRole in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                }
            }

            await _userManager.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            ViewData[CommonValues.ErrorMessage] = "An error occurred deleting this user";
            await PopulateUserDetails(id);
            return;
        }

        Response.Redirect("./ListUsers");
    }

    private async Task PopulateUserDetails(string id)
    {
        UserDetails.User = await _userManager.FindByIdAsync(id);
        UserDetails.CurrentRoles = await _userManager.GetRolesAsync(UserDetails.User);
    }
}
