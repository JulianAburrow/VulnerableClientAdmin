namespace VulnerableClientAdminUI.Pages.MyVulnerableClient.SavedPage;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        SavedPageModel = await SavedPageHandler.GetSavedPageAsync(SavedPageId);
        SavedPageDisplayModel.Title = SavedPageModel.Title;
        SavedPageDisplayModel.Url = SavedPageModel.Url;
        SavedPageDisplayModel.Notes = SavedPageModel.Notes;
        SavedPageDisplayModel.IsExternal = SavedPageModel.IsExternal;

        MainLayout.SetHeaderValue($"Edit Saved Page {SavedPageModel.Title}");
    }

    private async Task UpdateSavedPage()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await SavedPageHandler.UpdateSavedPageAsync(SavedPageModel, true);
            Snackbar.Add($"Saved Page {SavedPageModel.Title} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("savedpages/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating Saved Page {SavedPageModel.Title}. Please try again.", Severity.Error);
        }
    }
}
