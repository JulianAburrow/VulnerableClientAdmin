using Microsoft.AspNetCore.Identity;

namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public abstract class AuthBasePageClass : ComponentBase
{    
    [Inject] public SignInManager<ApplicationUser> SignInManager { get; set; } = default!;

    [Inject] public UserManager<ApplicationUser> UserManager { get; set; } = default!;

    [Inject] public NavigationManager Nav { get; set; } = default!;
    
}
