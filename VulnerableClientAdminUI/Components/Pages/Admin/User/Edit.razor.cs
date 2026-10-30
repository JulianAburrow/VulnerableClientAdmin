using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.User;

public partial class Edit
{
    private bool PreventEditing;

    protected override async Task OnInitializedAsync()
    {
        // Load the user being edited
        User = await UserManager.FindByIdAsync(Id);

        // Load the user's existing permission claim
        var claims = await UserManager.GetClaimsAsync(User);
        var permissionClaim = claims.FirstOrDefault(c => c.Type == PermissionNames.Permission);
        var userPermission = permissionClaim?.Value ?? string.Empty;

        // Build a local copy of the permission list
        Permissions = PermissionNames.AllPermissions.ToList();
        Permissions.Insert(0, SelectValues.PleaseSelectText);

        // Get the current logged-in ApplicationUser
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        CurrentUser = await UserManager.GetUserAsync(authState.User);

        // Prevent editing/deleting yourself
        PreventEditing = User.Id == CurrentUser.Id;
        PreventDeleting = User.Id == CurrentUser.Id;

        // Populate display model
        UserDisplayModel.FirstName = User.FirstName;
        UserDisplayModel.LastName = User.LastName;
        UserDisplayModel.Email = User.Email;
        UserDisplayModel.Permission = userPermission;

        // Set page header
        MainLayout.SetHeaderValue($"Edit User");
    }

    private async Task UpdateUser()
    {
        PopulateModelFromDisplayModel();

        try
        {
            // Load existing permission claim
            var claims = await UserManager.GetClaimsAsync(User);
            var oldPermissionClaim = claims.FirstOrDefault(c => c.Type == PermissionNames.Permission);
            var oldPermission = oldPermissionClaim?.Value ?? "None";

            // Remove old permission claim
            if (oldPermissionClaim != null)
            {
                await UserManager.RemoveClaimAsync(User, oldPermissionClaim);
            }

            // Add new permission claim (unless "Please select")
            if (UserDisplayModel.Permission != SelectValues.PleaseSelectText)
            {
                await UserManager.AddClaimAsync(
                    User,
                    new Claim(PermissionNames.Permission, UserDisplayModel.Permission));
            }

            // Audit the permission change
            var auditObject = new AuditObjectModel
            {
                ObjectId = User.Id,
                ColumnName = PermissionNames.Permission,
                ObjectType = Enums.ObjectType.ApplicationUser.ToString(),
                PreviousValue = oldPermission,
                NewValue = UserDisplayModel.Permission,
                ChangedDate = DateTime.Now,
                ChangedBy = CurrentUser.Email,
            };

            await AuditObjectHandler.CreateAuditObjectAsync(auditObject);

            Snackbar.Add($"User {User.FirstName} {User.LastName} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("users/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating the user {User.FirstName} {User.LastName}. Please try again.", Severity.Error);
        }
    }
}