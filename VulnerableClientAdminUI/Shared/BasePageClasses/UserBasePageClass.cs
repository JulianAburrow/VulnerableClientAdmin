using Microsoft.AspNetCore.Components.Authorization;

namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class UserBasePageClass : BasePageClass
{
    [Inject] protected UserManager<ApplicationUser> UserManager { get; set; } = null!;

    [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] protected RoleManager<IdentityRole> RoleManager { get; set; }

    protected ApplicationUser CurrentUser;

    protected ApplicationUser User { get; set; } = new();

    protected UserDisplayModel UserDisplayModel = new();

    [Parameter] public string Id { get; set; }

    protected List<string> Roles { get; set; }

    protected void PopulateModelFromDisplayModel()
    {
        User.FirstName = UserDisplayModel.FirstName;
        User.LastName = UserDisplayModel.LastName;
        User.Email = UserDisplayModel.Email;
        User.Role = UserDisplayModel.Role;
        User.UserName = UserDisplayModel.Email;
    }
}
