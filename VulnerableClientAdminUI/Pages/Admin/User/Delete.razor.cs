namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        User = await UserManager.FindByIdAsync(Id);
        User.Role = (await UserManager.GetRolesAsync(User)).FirstOrDefault() ?? string.Empty;
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.ApplicationUser.ToString(), Id);
        MainLayout.SetHeaderValue($"Delete User '{User?.FirstName} {User?.LastName}'");
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var principal = authState.User;

        CurrentUser = await UserManager.GetUserAsync(principal);

        PreventDeleting = User.Id == CurrentUser.Id;
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
