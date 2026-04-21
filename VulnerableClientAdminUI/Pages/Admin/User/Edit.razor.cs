namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class Edit
{
    private bool PreventEditing;

    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        User = await UserManager.FindByIdAsync(Id);
        User.Role = (await UserManager.GetRolesAsync(User)).FirstOrDefault() ?? "None";

        Roles = await RoleManager.Roles
            .Select(r => r.Name)
            .ToListAsync();
        Roles = Roles.OrderBy(r => r).ToList();
        Roles.Insert(0, "None");

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var principal = authState.User;

        CurrentUser = await UserManager.GetUserAsync(principal);

        PreventEditing = User.Id == CurrentUser.Id;
        PreventDeleting = User.Id == CurrentUser.Id;

        UserDisplayModel.FirstName = User.FirstName;
        UserDisplayModel.LastName = User.LastName;
        UserDisplayModel.Email = User.Email;
        UserDisplayModel.Role = User.Role;

        MainLayout.SetHeaderValue($"Edit User {User.FirstName} {User.LastName}");
    }

    private async Task UpdateUser()
    {
        PopulateModelFromDisplayModel();

        try
        {
            var oldRoles = await UserManager.GetRolesAsync(User);
            if (oldRoles.Any())
            {
                await UserManager.RemoveFromRolesAsync(User, oldRoles);
            }
            if (User.Role != "None")
            {
                await UserManager.AddToRoleAsync(User, User.Role);
            }

            var auditObject = new AuditObjectModel
            {
                ObjectId = User.Id,
                ColumnName = "Role",
                ObjectType = Enums.ObjectType.ApplicationUser.ToString(),
                PreviousValue = oldRoles.FirstOrDefault() ?? "None",
                NewValue = User.Role,
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