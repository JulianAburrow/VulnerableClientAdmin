using Microsoft.AspNetCore.Identity;
// No-op patch: ensure file saved. JSInterop will be added if needed.

namespace VulnerableClientAdminUI.Components.Pages.Auth;

public partial class Register
{
    [Inject] public NavigationManager Nav { get; set; } = default!;

    private string Message = "";

    public string FirstName = "";

    public string LastName = "";

    private string Email = "";

    private string Password = "";

    private string ConfirmPassword = "";

    protected override void OnInitialized()
    {
        var uri = Nav.Uri;

        if (uri.Contains("error=email"))
            Message = "Please enter a valid email address.";

        if (uri.Contains("error=name"))
            Message = "First name and last name are required.";

        if (uri.Contains("error=identity"))
            Message = "One or more fields was not completed";

        if (uri.Contains("error=confirm"))
            Message = "Passwords do not match.";

        if (uri.Contains("error=duplicate"))
            Message = "This email address is already registered.";
    }

    // Form is submitted directly by the browser to /auth/register-http
}