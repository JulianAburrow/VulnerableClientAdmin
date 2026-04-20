using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace VulnerableClientAdminUI.Shared.Components;

public partial class LoginDisplayComponent
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] public IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] public Microsoft.AspNetCore.Antiforgery.IAntiforgery Antiforgery { get; set; } = default!;

    private ClaimsPrincipal? authUser;
    private string AntiForgeryToken = "";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        authUser = authState.User;

        // Generate antiforgery token for logout POST
        var tokens = Antiforgery.GetAndStoreTokens(HttpContextAccessor.HttpContext);
        AntiForgeryToken = tokens.RequestToken;
    }
}