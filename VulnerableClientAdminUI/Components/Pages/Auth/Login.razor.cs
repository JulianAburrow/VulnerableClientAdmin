using Microsoft.AspNetCore.Identity;

namespace VulnerableClientAdminUI.Components.Pages.Auth;

public partial class Login
{
    private string Message = "";
    private string Email { get; set; } = "";
    private string Password { get; set; } = "";

    protected override void OnInitialized()
    {
        // Preserve existing behavior of showing error messages based on query string
        var uri = Nav.Uri;
        if (uri.Contains("error=1"))
            Message = "Invalid email or password";

        if (uri.Contains("error=pending"))
            Message = "Your account is pending approval. Please contact the administrator.";

        if (uri.Contains("error=inactive"))
            Message = "Your account is inactive. Please contact the administrator.";
    }
}
