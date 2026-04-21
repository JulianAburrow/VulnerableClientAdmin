using Microsoft.AspNetCore.Components.Authorization;

namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class Index
{
    private List<ApplicationUser> Users = new();

    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var principal = authState.User;

        CurrentUser = await UserManager.GetUserAsync(principal);

        Users = await UserManager.Users.ToListAsync();
        Users = Users.OrderBy(u => u.LastName).ToList();

        foreach (var user in Users)
        {
            var roles = await UserManager.GetRolesAsync(user);
            user.Role = roles.FirstOrDefault() ?? "None";
        }

        var userCount = Users.Count;
        Snackbar.Add(userCount == 1 ? $"{userCount} user found" : $"{userCount} users found",
            userCount == 0 ? Severity.Error : Severity.Success);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsAsync(Enums.ObjectType.ApplicationUser.ToString());

        MainLayout.SetHeaderValue("Users");
    }
}