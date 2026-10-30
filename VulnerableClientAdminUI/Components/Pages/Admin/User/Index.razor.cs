using Microsoft.AspNetCore.Components.Authorization;
using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.User;

public partial class Index
{
    private List<UserListItemModel> Users = new();

    protected override async Task OnInitializedAsync()
    {
        var principal = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        // Get the current logged -in ApplicationUser
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        CurrentUser = await UserManager.GetUserAsync(authState.User);

        // Build the list of users including permission claims
        var allUsers = await UserManager.Users.ToListAsync();
        Users = new List<UserListItemModel>();

        foreach (var user in allUsers)
        {
            var claims = await UserManager.GetClaimsAsync(user);
            var permission = claims
                .FirstOrDefault(c => c.Type == PermissionNames.Permission)
                ?.Value ?? "None";

            Users.Add(new UserListItemModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                Permission = permission
            });
        }

        // Sort by last name
        Users = Users.OrderBy(u => u.LastName).ToList();

        // Snackbar summary
        var userCount = Users.Count;
        Snackbar.Add(
            userCount == 1 ? $"{userCount} user found" : $"{userCount} users found",
            userCount == 0 ? Severity.Error : Severity.Success);

        // Load audit records
        AuditObjects = await AuditObjectHandler.GetAuditRecordsAsync(
            Enums.ObjectType.ApplicationUser.ToString());

        // Set page header
        MainLayout.SetHeaderValue("Users");
    }
}