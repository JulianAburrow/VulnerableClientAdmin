namespace VulnerableClientAdminDataAccess.Models;

public class VulnerableClientNameOnlyModel
{
    public int VulnerabilityInformationId { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}
