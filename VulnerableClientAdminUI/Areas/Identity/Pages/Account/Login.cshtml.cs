namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(SignInManager<IdentityUser> signInManager) =>
        _signInManager = signInManager;

    [BindProperty]
    public LoginInputModel LoginInput { get; set; } = null!;
    
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPost()
    {
        var returnUrl = !string.IsNullOrEmpty(Request.Query["ReturnUrl"].ToString())
            ? $"{GlobalVariables.RootUrl}/{Request.Query["ReturnUrl"]}"
            : GlobalVariables.RootUrl;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _signInManager.PasswordSignInAsync(LoginInput.Email, LoginInput.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                GlobalVariables.UserName = LoginInput.Email;
                return LocalRedirect(returnUrl);
            }
            
            ViewData[CommonValues.ErrorMessage] = "Credentials not found. Please check and try again or register if you have not already done so.";

            throw new Exception("Credentials not found.");
        }
        catch(Exception ex)
        {
            //ex.ToExceptionless().Submit();
            if (ViewData[CommonValues.ErrorMessage] == null)
            {
                ViewData[CommonValues.ErrorMessage] = "An error occurred whilst attempting to log in";
            }
        }

        return Page();
    }
}
