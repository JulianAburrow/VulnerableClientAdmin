namespace VulnerableClientAdminUI.Shared.Services;

public interface IAppAuthorizationService
{
    /// <summary>
    /// Returns true if the current user is authenticated AND in the specified role.
    /// </summary>
    Task<bool> UserIsAuthorisedAsync(string requiredRole);

    /// <summary>
    /// Returns true if the current user is authenticated AND in ANY of the specified roles.
    /// </summary>
    Task<bool> UserIsInAnyRoleAsync(params string[] roles);

    /// <summary>
    /// Returns true if the current user is authenticated AND is an Admin.
    /// </summary>
    Task<bool> UserIsAdminAsync();

    /// <summary>
    /// Returns true if the current user is authenticated AND is a SuperUser.
    /// </summary>
    Task<bool> UserIsSuperUserAsync();

    /// <summary>
    /// Returns true if the current user is authenticated AND is either Admin or SuperUser.
    /// </summary>
    Task<bool> UserIsAdminOrSuperUserAsync();
}


