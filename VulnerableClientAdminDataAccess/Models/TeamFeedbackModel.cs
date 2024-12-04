namespace VulnerableClientAdminDataAccess.Models;

public class TeamFeedbackModel : IAuditableObject
{
    public int TeamFeedbackId { get; set; }

    public int VulnerabilityInformationId { get; set; }

    public string Feedback { get; set; } = default!;

    public DateTime FeedbackDate { get; set; }

    public DateTime DateCreated { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime DateLastUpdated { get; set; }

    public string LastUpdatedBy { get; set; } = default!;

    public VulnerabilityInformationModel Vulnerability { get; set; } = null!;
}
