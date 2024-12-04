
namespace VulnerableClientAdminDataAccess.Models;

public class SourceOfAwarenessModel : IAuditableObject
{
    public int SourceOfAwarenessId { get; set; }

    public string Source { get; set; } = default!;

    public bool SourceActive { get; set; }

    public string? Description { get; set; } = default!;

    public DateTime DateCreated { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime DateLastUpdated { get; set; }

    public string LastUpdatedBy { get; set; } = default!;

    public ICollection<VulnerabilityInformationModel> Vulnerabilities { get; set; } = null!;
}
