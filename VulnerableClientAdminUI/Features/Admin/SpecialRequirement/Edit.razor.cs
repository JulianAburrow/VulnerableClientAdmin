namespace VulnerableClientAdminUI.Features.Admin.SpecialRequirement;

public partial class Edit
{
    private bool PreventEditing;

    protected override async Task OnInitializedAsync()
    {
        SpecialRequirementModel = await SpecialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementId);
        SpecialRequirementDisplayModel.Requirement = SpecialRequirementModel.Requirement;
        SpecialRequirementDisplayModel.RequirementActive = SpecialRequirementModel.RequirementActive;
        SpecialRequirementDisplayModel.Description = SpecialRequirementModel.Description;

        PreventEditing = SpecialRequirementModel.Vulnerabilities.Any();

        MainLayout.SetHeaderValue($"Edit Special Requirement '{SpecialRequirementModel.Requirement}'");
    }

    private async Task UpdateSpecialRequirement()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SpecialRequirementHandler.UpdateSpecialRequirementAsync(SpecialRequirementModel, true);
            Snackbar.Add($"Special Requirement {SpecialRequirementModel.Requirement} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("specialrequirements/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add($"An error occurred updating the Special Requirement {SpecialRequirementModel.Requirement}. Please try again.", Severity.Error);
        }
    }
}
