namespace VulnerableClientAdminUI.Pages.Admin.PreferredContactMethod;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        PreferredContactMethodModel = await PreferredContactMethodHandler.GetPreferredContactMethodAsync(PreferredContactMethodId);
        // Is this PreferredContactMethod in use?
        PreventDeleting = PreferredContactMethodModel.Vulnerabilities.Any();
        MainLayout.SetHeaderValue($"Delete Preferred Contact Method {PreferredContactMethodModel.Method}");
    }

    private async Task DeletePreferredContactMethod()
    {
        try
        {
            await PreferredContactMethodHandler.DeletePreferredContactMethodAsync(PreferredContactMethodModel.PreferredContactMethodId, true);
            Snackbar.Add("Preferred Contact Method successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("preferredcontactmethods/index");
        }
        catch
        {
            Snackbar.Add("An error occurred deleting Preferred Contact Method. Please try again.", Severity.Error);
        }
    }
}
