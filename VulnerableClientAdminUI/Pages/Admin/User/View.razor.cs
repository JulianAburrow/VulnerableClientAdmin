namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        User = await UserManager.FindByIdAsync(Id);
        User.Role = (await UserManager.GetRolesAsync(User)).FirstOrDefault() ?? string.Empty;
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.ApplicationUser.ToString(), Id);
        MainLayout.SetHeaderValue($"View User '{User?.FirstName} {User?.LastName}'");

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var principal = authState.User;

        CurrentUser = await UserManager.GetUserAsync(principal);

        PreventDeleting = User.Id == CurrentUser.Id;
    }
}