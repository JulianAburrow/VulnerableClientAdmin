namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

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

        PreferredContactMethodModel = await PreferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodId);
        PreferredContactMethodDisplayModel.Method = PreferredContactMethodModel.Method;
        PreferredContactMethodDisplayModel.MethodActive = PreferredContactMethodModel.MethodActive;
        PreferredContactMethodDisplayModel.Description = PreferredContactMethodModel.Description;

        // Is this PreferredContactMethod in use?
        PreventEditing = PreferredContactMethodModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"Edit Preferred Contact Method {PreferredContactMethodModel.Method}");
    }

    private async Task UpdatePreferredContactMethod()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await PreferredContactMethodHandler.UpdatePreferredContactMethodAsync(PreferredContactMethodModel, true);
            Snackbar.Add($"Preferred Contact Method {PreferredContactMethodModel.Method} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("preferredcontactmethods/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating the Preferred Contact Method {PreferredContactMethodModel.Method}. Please try again.", Severity.Error);
        }
    }
}
