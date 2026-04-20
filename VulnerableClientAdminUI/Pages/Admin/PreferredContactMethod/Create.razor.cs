namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

public partial class Create
{
    protected override void OnInitialized()
    {
        PreferredContactMethodDisplayModel.MethodActive = true;
        MainLayout.SetHeaderValue("Create Preferred Contact Method");
    }

    private async Task CreatePreferredContactMethod()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await PreferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel, true);
            Snackbar.Add($"Preferred Contact Method {PreferredContactMethodModel.Method} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("preferredcontactmethods/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the Preferred Contact Method {PreferredContactMethodModel.Method}. Please try again.", Severity.Error);
        }
    }
}
