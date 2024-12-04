namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class TeamFeedbackBasePageClass : BasePageClass
{
    [Inject] protected ITeamFeedbackHandler TeamFeedbackHandler { get; set; } = null!;

    [Inject] protected IVulnerabilityInformationHandler VulnerabilityInformationHandler { get; set; } = null!;

    [Parameter] public int TeamFeedbackId { get; set; }

    [Parameter] public int VulnerabilityInformationId { get; set; }

    protected TeamFeedbackModel TeamFeedbackModel = new();

    protected TeamFeedbackDisplayModel TeamFeedbackDisplayModel = new();

    protected void PopulateModelFromDisplayModel()
    {
        TeamFeedbackModel.Feedback = TeamFeedbackDisplayModel.Feedback;
    }
}
