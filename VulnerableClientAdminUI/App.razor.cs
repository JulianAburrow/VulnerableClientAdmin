namespace VulnerableClientAdminUI;

public partial class App
{
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    private string ReturnUrl { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        ReturnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
    }
}
