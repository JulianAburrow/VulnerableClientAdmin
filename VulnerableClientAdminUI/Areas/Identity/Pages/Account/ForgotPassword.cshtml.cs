namespace VulnerableClientAdminUI.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public ForgotPasswordModel(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }   

    [BindProperty]
    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; } = string.Empty;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        var returnUrl = "~/Identity/Account/ForgotPasswordConfirmation";

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userManager.FindByNameAsync(EmailAddress);
            if (user == null)
            {
                return LocalRedirect(returnUrl);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordUrl = $"{Request.Host.Host}{GlobalVariables.RootUrl}/Identity/Account/ResetPassword?userid={user.Id}&code={code}";
            var fromAddress = _configuration.GetValue<string>("Smtp:FromAddress") ?? throw new Exception("From Address is required");
            var MailMessage = new MailMessage(
                fromAddress,
                EmailAddress,
                "Reset Password",
                "Please reset your password by clicking <a href=\"" + resetPasswordUrl + "\">here</a>")
            {
                IsBodyHtml = true
            };

            // Get PickupDirectoryLocation and check that directory exists. If not, create it.

            using (var SmtpClient = new SmtpClient())
            {
                SmtpClient.UseDefaultCredentials = true;
                SmtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                var pickupDirectoryLocation = _configuration.GetValue<string>("Smtp:SpecifiedPickupDirectory:PickupDirectoryLocation");
                if (pickupDirectoryLocation == null)
                {
                    throw new Exception("Pickup Directory cannot be null");
                }

                SmtpClient.PickupDirectoryLocation = pickupDirectoryLocation;
                if (!Directory.Exists(pickupDirectoryLocation))
                {
                    Directory.CreateDirectory(pickupDirectoryLocation);
                }
                try
                {
                    SmtpClient.Send(MailMessage);
                }
                catch (Exception ex)
                {
                    //ex.ToExceptionless().Submit();
                }
            }

            return LocalRedirect(returnUrl);
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
        }

        return LocalRedirect(returnUrl);
    }
}
