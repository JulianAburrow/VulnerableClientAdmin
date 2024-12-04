namespace VulnerableClientAdminUI.Models;

public class TeamFeedbackDisplayModel
{
    public int TeamFeedbackId { get; set; }

    public int VulnerabilityInformationId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    public string Feedback { get; set; } = default!;

    public DateTime FeedbackDate { get; set; }
}
