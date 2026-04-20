namespace VulnerableClientAdminUI.Pages.Admin.User;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        Roles = await RoleManager.Roles
            .Select(r => r.Name)
            .ToListAsync();
        Roles = Roles.OrderBy(r => r).ToList();
        Roles.Insert(0, "None");
        Roles.Insert(0, SelectValues.PleaseSelectText);
        UserDisplayModel.Role = SelectValues.PleaseSelectText;
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
            {
                Snackbar.Add(error.Description, Severity.Error);
            }
            return;
        }

        try
        {
            var createResult = await UserManager.CreateAsync(User, UserDisplayModel.Password);

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    Snackbar.Add(error.Description, Severity.Error);
                }
                return;
            }

            if (UserDisplayModel.Role != SelectValues.PleaseSelectText && UserDisplayModel.Role != "None")
            {
                await UserManager.AddToRoleAsync(User, UserDisplayModel.Role);
            }
            Snackbar.Add($"User {User.FirstName} {User.LastName} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("users/index");

        }
        catch
        {
            Snackbar.Add($"An error occurred creating the user {User.FirstName} {User.LastName}. Please try again.", Severity.Error);
        }
    }
}