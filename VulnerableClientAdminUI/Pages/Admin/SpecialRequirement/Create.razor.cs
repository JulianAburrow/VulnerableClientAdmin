namespace VulnerableClientAdminUI.Pages.Admin.SpecialRequirement;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        SpecialRequirementDisplayModel.RequirementActive = true;

        MainLayout.SetHeaderValue("Create Special Requirement");
    }

    private async Task CreateSpecialRequirement()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SpecialRequirementHandler.CreateSpecialRequirementAsync(SpecialRequirementModel, true);
            Snackbar.Add($"Special Requirement {SpecialRequirementModel.Requirement} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("specialrequirements/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the Special Requirement {SpecialRequirementModel.Requirement}. Please try again.", Severity.Error);
        }
    }
}
