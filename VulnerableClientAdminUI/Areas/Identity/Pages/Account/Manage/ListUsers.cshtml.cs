namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account.Manage;

public class ListUsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public ListUsersModel(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;        

    public List<IdentityUser> Users { get; set; } = null!;

    public void OnGet()
    {
        GetUsers();
    }

    private void GetUsers()
    {
        Users = _userManager.Users
            .OrderBy(u => u.UserName)
            .ToList();
    }
}
