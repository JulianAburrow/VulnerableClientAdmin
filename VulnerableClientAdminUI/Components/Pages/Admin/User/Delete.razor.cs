using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.User;

public partial class Delete
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

        // Prevent deleting yourself
        PreventDeleting = appUser.Id == CurrentUser.Id;

        // Set header
        MainLayout.SetHeaderValue($"Delete User");
    }

    private async Task DeleteUser()
    {
        try
        {
            // Remove all roles first (explicit)
            var roles = await UserManager.GetRolesAsync(User);
            if (roles.Any())
            {
                await UserManager.RemoveFromRolesAsync(User, roles);
            }

            // Now delete the user
            var result = await UserManager.DeleteAsync(User);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    Snackbar.Add(error.Description, Severity.Error);

                return;
            }

            Snackbar.Add($"User {User.FirstName} {User.LastName} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("users/index");
        }
        catch
        {
            Snackbar.Add("An error occurred while deleting the user.", Severity.Error);
        }
    }


}
