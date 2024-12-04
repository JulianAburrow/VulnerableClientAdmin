namespace VulnerableClientAdminUI.Shared;

public partial class MainLayout
{
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    [Inject] ISavedPageHandler SavedPageHandler { get; set; } = default!;

    [Inject] ISnackbar Snackbar { get; set; } = default!;

    private string HeaderValue { get; set; } = null!;

    private bool @_drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    public void SetHeaderValue(string headerValue)
    {
        HeaderValue = headerValue;
        StateHasChanged();
    }

    private async Task SavePage()
    {
        var savedPage = new SavedPageModel
        {
            Title = "Saved Page",
            Url = NavigationManager.Uri,
            Notes = "Saved page in Vulnerable Client Admin",
            IsExternal = false,
            Owner = GlobalVariables.UserName,
        };

        try
        {
            await SavedPageHandler.CreateSavedPageAsync(savedPage, true);
            Snackbar.Add("Page successfully saved", Severity.Success);
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add("An error occurred saving the page. Please try again", Severity.Error);
        }        
    }
}