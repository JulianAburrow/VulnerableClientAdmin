namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public RegisterInputModel RegisterInput { get; set; } = null!;

    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public string ReturnUrl { get; set; } = string.Empty;

    public void OnGet()
    {
        ReturnUrl = Url.Content("~/");
    }

    public async Task<IActionResult> OnPost()
    {
        ReturnUrl = Url.Content("~/Identity/Account/Registered");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var errorMessage = "An error occurred attempting to register. Please try again.";

        try
        {
            var identity = new IdentityUser { UserName = RegisterInput.Email, Email = RegisterInput.Email };
            var result = await _userManager.CreateAsync(identity, RegisterInput.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identity, isPersistent: false);
                return LocalRedirect(ReturnUrl);
            }

            ViewData[CommonValues.ErrorMessage] = errorMessage;

            throw new Exception(result.Errors.FirstOrDefault() != null
                ? $"{result.Errors.First().Code}" +
                $": {result.Errors.First().Description}" : "Unknown error");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            ViewData[CommonValues.ErrorMessage] = errorMessage;
        }        

        return Page();
    }
}
