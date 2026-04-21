namespace VulnerableClientAdminUI.Pages.MyVulnerableClient.SavedPage;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        SavedPageModel = await SavedPageHandler.GetSavedPageAsync(SavedPageId);

        MainLayout.SetHeaderValue($"Delete Saved Page {SavedPageModel.Title}");
    }

    private async Task DeleteSavedPage()
    {
        try
        {
            await SavedPageHandler.DeleteSavedPage(SavedPageId, true);
            Snackbar.Add($"Save Page {SavedPageModel.Title} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("savedpages/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting Saved Page {SavedPageModel.Title}. Please try again", Severity.Error);
        }
    }
}
