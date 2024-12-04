namespace VulnerableClientAdminDataAccess.Models;

public class SpecialRequirementModel : IAuditableObject
{
    public int SpecialRequirementId { get; set; }

    public string Requirement { get; set; } = default!;

    public bool RequirementActive { get; set; }

    public string? Description { get; set; } = default!;

    public DateTime DateCreated { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime DateLastUpdated { get; set; }

    public string LastUpdatedBy { get; set; } = default!;

    public List<VulnerabilityInformationModel> Vulnerabilities { get; set; } = null!;
}
