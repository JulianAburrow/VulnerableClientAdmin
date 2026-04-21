namespace VulnerableClientAdminUI.Pages.Admin.SourceOfAwareness;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        if (!await AppAuthorizationService.UserIsAdminAsync())
        {
            Snackbar.Add("You are not authorised to view this page.", Severity.Error);
            return;
        }

        SourceOfAwarenessDisplayModel.SourceActive = true;

        MainLayout.SetHeaderValue("Create Source Of Awareness");
    }

    private async Task CreateSourceOfAwareness()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SourceOfAwarenessHandler.CreateSourceOfAwarenessAsync(SourceOfAwarenessModel, true);
            Snackbar.Add($"Source Of Awareness {SourceOfAwarenessModel.Source} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("sourcesofawareness/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating the Source Of Awareness {SourceOfAwarenessModel.Source}. Please try again.", Severity.Error);
        }
    }
}
