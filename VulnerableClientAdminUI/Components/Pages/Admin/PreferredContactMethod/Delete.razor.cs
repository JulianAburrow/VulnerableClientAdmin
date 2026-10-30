using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.PreferredContactMethod;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        PreferredContactMethodModel = await PreferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodId);
        // Is this PreferredContactMethod in use?
        PreventDeleting = PreferredContactMethodModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"Delete Preferred Contact Method");
    }

    private async Task DeletePreferredContactMethod()
    {
        try
        {
            await PreferredContactMethodHandler.DeletePreferredContactMethodAsync(PreferredContactMethodModel.PreferredContactMethodId);
            Snackbar.Add("Preferred Contact Method successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("preferredcontactmethods/index");
        }
        catch
        {
            Snackbar.Add("An error occurred deleting Preferred Contact Method. Please try again.", Severity.Error);
        }
    }
}
