namespace VulnerableClientAdminUI.Shared.Components;

public partial class RedirectToLoginComponent
{
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public string ReturnUrl { get; set; } = default!;

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;
        var queryString = !string.IsNullOrWhiteSpace(ReturnUrl)
            ? $"?ReturnUrl={ReturnUrl}"
            : string.Empty;
        NavigationManager.NavigateTo($"{GlobalVariables.RootUrl}/Identity/Account/Login{queryString}", true);
    }
}
