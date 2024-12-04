namespace VulnerableClientAdminUI.Features.MyVulnerableClient.SavedPage;

public partial class Create
{
    protected override void OnInitialized() =>
        MainLayout.SetHeaderValue("Create Saved Page");

    private async Task CreateSavedPage()
    {
        PopulateModelFromDisplayModel();
        SavedPageModel.Owner = VulnerableClientAdminDataAccess.GlobalVariables.UserName;

        try
        {
            await SavedPageHandler.CreateSavedPageAsync(SavedPageModel, true);
            Snackbar.Add($"Saved Page {SavedPageModel.Title} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("savedpages/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add($"An error occurred creating the Saved Page {SavedPageModel.Title}. Please try again.", Severity.Error);
        }
    }
}
