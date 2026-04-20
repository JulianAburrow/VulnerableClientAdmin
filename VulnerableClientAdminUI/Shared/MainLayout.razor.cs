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

    private bool _hasRendered;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            _hasRendered = true;
    }

    public void SetHeaderValue(string headerValue)
    {
        HeaderValue = headerValue;

        if (_hasRendered)
            InvokeAsync(StateHasChanged);
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
        catch
        {
            Snackbar.Add("An error occurred saving the page. Please try again", Severity.Error);
        }        
    }
}