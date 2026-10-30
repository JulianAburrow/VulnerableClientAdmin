namespace VulnerableClientAdminUI.Models;

public class UserListItemModel
{
    public string Id { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public bool IsActive { get; set; }

    public string Permission { get; set; } = default!;
}