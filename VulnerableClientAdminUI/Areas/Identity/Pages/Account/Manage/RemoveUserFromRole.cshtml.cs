namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account.Manage;

public class RemoveUserFromRoleModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RemoveUserFromRoleModel(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public UserAndRoleModel Input { get; set; } = new();
    public IdentityRole Role { get; set; }
    public string UserName { get; set; } = string.Empty!;
    public IList<string> CurrentRoles { get; set; } = null!;

    public class UserAndRoleModel
    {
        public string UserId { get; set; } = string.Empty!;
        public string RoleId { get; set; } = string.Empty!;
        public SelectList Roles { get; set; } = null!;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        Input.UserId = id;
        PopulateRoles();
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return Page();
        }
        UserName = user.UserName;
        CurrentRoles = await _userManager.GetRolesAsync(user);
        return Page();
    }

    private void PopulateRoles()
    {
        var roles = _roleManager.Roles
            .OrderBy(r => r.Name)
            .ToList();
        var pleaseSelect = new IdentityRole
        {
            Id = CommonValues.PleaseSelectValue,
            Name = CommonValues.PleaseSelectText,
        };
        roles.Insert(0, pleaseSelect);
        Input.Roles = new SelectList(
            roles,
            nameof(IdentityRole.Id),
            nameof(IdentityRole.Name));
    }

    public async Task OnPost()
    {
        ModelState.Clear();

        var user = await _userManager.FindByIdAsync(Input.UserId);
        UserName = user.UserName;
        if (user == null)
        {
            ViewData[CommonValues.ErrorMessage] = "User not found";
            PopulateRoles();
            return;
        }

        CurrentRoles = await _userManager.GetRolesAsync(user);

        if (Input.RoleId == CommonValues.PleaseSelectValue)
        {
            ModelState.AddModelError("Input.RoleId", "Role is required");
        }

        if (!ModelState.IsValid)
        {
            PopulateRoles();
            return;
        }
        var role = await _roleManager.FindByIdAsync(Input.RoleId);
        if (role == null)
        {
            ViewData[CommonValues.ErrorMessage] = "Role not found";
            PopulateRoles();
            return;
        }
        var userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Contains(role.Name))
        {
            ViewData[CommonValues.ErrorMessage] = $"User {user.UserName} is not in role {role.Name}.";
            PopulateRoles();
            return;
        }

        if (UserName == CommonValues.SystemLordUserName && role.Name == CommonValues.SuperUserRoleName)
        {
            ViewData[CommonValues.ErrorMessage] = $"Cannot remove {CommonValues.SuperUserRoleName} role from {CommonValues.SystemLordUserName}";
            PopulateRoles();
            return;
        }

        // User is in role, and we are not trying to remove the SuperUser role from SystemLord...
        try
        {
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            Response.Redirect($"./RemoveUserFromRole?Id={Input.UserId}");
        }
        catch(Exception ex)
        {
            //ex.ToExceptionless().Submit();
            ViewData[CommonValues.ErrorMessage] = "An error occurred removing user from role";
        }
        
        return;        
    }
}
