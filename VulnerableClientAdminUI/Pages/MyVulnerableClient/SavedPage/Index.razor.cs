using VulnerableClientAdminDataAccess;

namespace VulnerableClientAdminUI.Pages.MyVulnerableClient.SavedPage;

public partial class Index
{
    private List<SavedPageModel> SavedPages = new();

    protected override async Task OnInitializedAsync()
    {
        SavedPages = await SavedPageHandler.GetSavedPagesByUserAsync(GlobalVariables.UserName);
        var savedPageCount = SavedPages.Count;
        Snackbar.Add(savedPageCount == 1
            ? $"{savedPageCount} saved page found"
            : $"{savedPageCount} saved pages found",
            savedPageCount == 0 ? Severity.Error : Severity.Success);

        MainLayout.SetHeaderValue("Saved Pages");
    }
}
