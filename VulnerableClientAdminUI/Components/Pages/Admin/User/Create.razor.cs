namespace VulnerableClientAdminUI.Components.Pages.Admin.User;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        // Provide the list of available permissions
        Permissions = PermissionNames.AllPermissions;
        Permissions.Insert(0, SelectValues.PleaseSelectText);

        // Default selection
        UserDisplayModel.Permission = SelectValues.PleaseSelectText;
    }

    private async Task CreateUser()
    {
        PopulateModelFromDisplayModel();

        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var passwordValidation = await passwordValidator.ValidateAsync(
            UserManager,
            User,
            UserDisplayModel.Password);

        if (!passwordValidation.Succeeded)
        {
            foreach (var error in passwordValidation.Errors)
                Snackbar.Add(error.Description, Severity.Error);

            return;
        }

        try
        {
            var createResult = await UserManager.CreateAsync(User, UserDisplayModel.Password);

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                    Snackbar.Add(error.Description, Severity.Error);

                return;
            }

            // Assign the selected permission claim
            await UserManager.AddClaimAsync(
                User,
                new Claim(PermissionNames.Permission, UserDisplayModel.Permission));

            Snackbar.Add($"User {User.FirstName} {User.LastName} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("users/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the user {User.FirstName} {User.LastName}. Please try again.", Severity.Error);
        }
    }
}
