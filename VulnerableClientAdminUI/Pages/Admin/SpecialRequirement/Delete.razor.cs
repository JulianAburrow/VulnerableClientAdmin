namespace VulnerableClientAdminUI.Pages.Admin.SpecialRequirement;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

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
        catch
        {
            Snackbar.Add("An error occurred deleting Special Requirement. Please try again", Severity.Error);
        }
    }
}
