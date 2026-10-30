using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.PreferredContactMethod;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        PreferredContactMethodDisplayModel.MethodActive = true;
        MainLayout.SetHeaderValue("Create Preferred Contact Method");
    }

    private async Task CreatePreferredContactMethod()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await PreferredContactMethodHandler.CreatePreferredContactMethodAsync(PreferredContactMethodModel);
            Snackbar.Add($"Preferred Contact Method {PreferredContactMethodModel.Method} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("preferredcontactmethods/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the Preferred Contact Method {PreferredContactMethodModel.Method}. Please try again.", Severity.Error);
        }
    }
}
