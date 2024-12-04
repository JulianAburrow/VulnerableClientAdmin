namespace VulnerableClientAdminUI.Areas.Identity.Models
{
    public class UserModel
    {
        public IdentityUser User { get; set; } = new();

        public IList<string> CurrentRoles { get; set; } = null!;
    }
}
