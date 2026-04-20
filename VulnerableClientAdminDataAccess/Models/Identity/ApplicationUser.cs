namespace VulnerableClientAdminDataAccess.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    public string Role { get; set; } = string.Empty;
}
