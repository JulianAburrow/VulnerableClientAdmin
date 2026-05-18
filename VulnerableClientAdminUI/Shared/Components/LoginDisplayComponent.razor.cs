using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace VulnerableClientAdminUI.Shared.Components;

public partial class LoginDisplayComponent
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private ClaimsPrincipal? authUser;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        authUser = authState.User;
    }
}