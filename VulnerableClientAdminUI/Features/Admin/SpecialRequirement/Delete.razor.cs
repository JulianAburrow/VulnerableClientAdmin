namespace VulnerableClientAdminUI.Features.Admin.SpecialRequirement;

public partial class Delete
{

    protected override async Task OnInitializedAsync()
    {
        SpecialRequirementModel = await SpecialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementId);
        PreventDeleting = SpecialRequirementModel.Vulnerabilities.Any();
        MainLayout.SetHeaderValue($"Delete Special Requirement {SpecialRequirementModel.Requirement}");
    }

    private async Task DeleteSpecialRequirement()
    {
        try
        {
            await SpecialRequirementHandler.DeleteSpecialRequirementAsync(SpecialRequirementModel.SpecialRequirementId, true);
            Snackbar.Add("Special Requirement successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("specialrequirements/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add("An error occurred deleting Special Requirement. Please try again", Severity.Error);
        }
    }
}
