using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.Admin.SpecialRequirement;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        SpecialRequirementModel = await SpecialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementId);
        PreventDeleting = SpecialRequirementModel.Vulnerabilities is not null;

        MainLayout.SetHeaderValue($"Delete Special Requirement");
    }

    private async Task DeleteSpecialRequirement()
    {
        try
        {
            await SpecialRequirementHandler.DeleteSpecialRequirementAsync(SpecialRequirementModel.SpecialRequirementId);
            Snackbar.Add("Special Requirement successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("specialrequirements/index");
        }
        catch
        {
            Snackbar.Add("An error occurred deleting Special Requirement. Please try again", Severity.Error);
        }
    }
}
