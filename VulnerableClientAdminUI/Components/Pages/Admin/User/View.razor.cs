using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.User;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        var appUser = await UserManager.FindByIdAsync(Id);

        // Load the user's permission claim
        var claims = await UserManager.GetClaimsAsync(appUser);
        var permission = claims
            .FirstOrDefault(c => c.Type == PermissionNames.Permission)
            ?.Value ?? "None";

        // Build the view model for the component
        UserListItemModel = new UserListItemModel
        {
            Id = appUser.Id,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Email = appUser.Email,
            IsActive = appUser.IsActive,
            Permission = permission
        };

        // Load audit records
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(
            Enums.ObjectType.ApplicationUser.ToString(),
            Id);

        // Get the current logged-in ApplicationUser
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        CurrentUser = await UserManager.GetUserAsync(authState.User);

        // Prevent viewing yourself as deletable/editable
        PreventDeleting = appUser.Id == CurrentUser.Id;

        // Set header
        MainLayout.SetHeaderValue($"View User");
    }
}