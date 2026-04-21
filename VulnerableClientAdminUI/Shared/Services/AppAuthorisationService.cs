namespace VulnerableClientAdminUI.Shared.Services;

public class AppAuthorizationService : IAppAuthorizationService
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public AppAuthorizationService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    private async Task<ClaimsPrincipal> GetUserAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<bool> UserIsAuthorisedAsync(string requiredRole)
    {
        var user = await GetUserAsync();

        if (!user.Identity?.IsAuthenticated ?? false)
            return false;

        return user.IsInRole(requiredRole);
    }

    public async Task<bool> UserIsInAnyRoleAsync(params string[] roles)
    {
        var user = await GetUserAsync();

        if (!user.Identity?.IsAuthenticated ?? false)
            return false;

        foreach (var role in roles)
        {
            if (user.IsInRole(role))
                return true;
        }

        return false;
    }

    public async Task<bool> UserIsAdminAsync()
    {
        return await UserIsAuthorisedAsync(RoleNames.AdminRoleName);
    }

    public async Task<bool> UserIsSuperUserAsync()
    {
        return await UserIsAuthorisedAsync(RoleNames.SuperUserRoleName);
    }

    public async Task<bool> UserIsAdminOrSuperUserAsync()
    {
        return await UserIsInAnyRoleAsync(
            RoleNames.AdminRoleName,
            RoleNames.SuperUserRoleName);
    }
}
