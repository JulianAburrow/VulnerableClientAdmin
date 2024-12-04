using System.Web;

namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class ResetPasswordModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public ResetPasswordModel(UserManager<IdentityUser> userManager) =>
        _userManager = userManager;

    [BindProperty]
    public ResetPasswordInputModel ResetPasswordInput { get; set; } = null!;

    public class ResetPasswordInputModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;
    }

    public IActionResult OnGet(string? code = null)
    {
        if (code == null)
        {
            return BadRequest("A code must be supplied for password reset.");
        }

        ResetPasswordInput = new ResetPasswordInputModel
        {
            Code = code,
        };
        return Page();        
    }

    public async Task<IActionResult> OnPost()
    {
        var returnUrl = "~/Identity/Account/ResetPasswordConfirmation";

        if (!ModelState.IsValid)
        {
            return Page();
        }
        try
        {
            var user = await _userManager.FindByEmailAsync(ResetPasswordInput.Email);
            if (user == null)
            {
                return LocalRedirect(returnUrl);
            }
            
            // It is necessary to replace " " with "+" as these get stripped out in the url...
            var result = await _userManager.ResetPasswordAsync(user, ResetPasswordInput.Code.Replace(" ", "+"), ResetPasswordInput.NewPassword);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                //new Exception(error.Description).ToExceptionless().Submit();
            }

            ViewData[CommonValues.ErrorMessage] = "An error occurred attempting to reset your password. If this persists please raise a ticket for IT.";
        }
        catch(Exception ex)
        {
            //ex.ToExceptionless().Submit();           
        }

        return Page();
    }
}
