namespace VulnerableClientAdminUI.Features.TeamFeedback;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        TeamFeedbackModel = await TeamFeedbackHandler.GetTeamFeedbackAsync(TeamFeedbackId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.TeamFeedbackModel.ToString(), TeamFeedbackId);
        MainLayout.SetHeaderValue("View Team Feedback");
    }
}
