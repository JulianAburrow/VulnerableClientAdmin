using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.MyVulnerableClient.SavedPage;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SavedPageModel = await SavedPageHandler.GetSavedPageAsync(SavedPageId);

        MainLayout.SetHeaderValue($"View Saved Page {SavedPageModel.Title}");
    }
}
