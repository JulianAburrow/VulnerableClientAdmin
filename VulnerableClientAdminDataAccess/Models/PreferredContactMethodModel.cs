namespace VulnerableClientAdminDataAccess.Models;

public class PreferredContactMethodModel : IAuditableObject
{
    public int PreferredContactMethodId { get; set; }

    public string Method { get; set; } = default!;

    public bool MethodActive { get; set; }

    public string? Description { get; set; } = default!;

    public DateTime DateCreated { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime DateLastUpdated { get; set; }

    public string LastUpdatedBy { get; set; } = default!;

    public List<VulnerabilityInformationModel> Vulnerabilities { get; set; } = null!;
}
