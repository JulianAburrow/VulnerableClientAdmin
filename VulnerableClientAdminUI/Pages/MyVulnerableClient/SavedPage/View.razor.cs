namespace VulnerableClientAdminUI.Pages.MyVulnerableClient.SavedPage;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SavedPageModel = await SavedPageHandler.GetSavedPageAsync(SavedPageId);

        MainLayout.SetHeaderValue($"View Saved Page {SavedPageModel.Title}");
    }
}
