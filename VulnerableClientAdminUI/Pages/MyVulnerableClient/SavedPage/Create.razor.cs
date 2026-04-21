namespace VulnerableClientAdminUI.Pages.MyVulnerableClient.SavedPage;

public partial class Create
{    
    protected override async Task OnInitializedAsync()
    {
        MainLayout.SetHeaderValue("Create Saved Page");
    }

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
        catch
        {
            Snackbar.Add($"An error occurred creating the Saved Page {SavedPageModel.Title}. Please try again.", Severity.Error);
        }
    }
}
