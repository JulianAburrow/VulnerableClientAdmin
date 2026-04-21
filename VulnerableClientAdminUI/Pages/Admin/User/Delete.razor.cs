namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        User = await UserManager.FindByIdAsync(Id);
        User.Role = (await UserManager.GetRolesAsync(User)).FirstOrDefault() ?? string.Empty;
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.ApplicationUser.ToString(), Id);
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var principal = authState.User;

        CurrentUser = await UserManager.GetUserAsync(principal);

        PreventDeleting = User.Id == CurrentUser.Id;

        MainLayout.SetHeaderValue($"Delete User {User.FirstName} {User.LastName}");
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
