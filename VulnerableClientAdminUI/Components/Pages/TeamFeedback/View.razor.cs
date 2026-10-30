using VulnerableClientAdminUI.Components.Layout;

namespace VulnerableClientAdminUI.Components.Pages.TeamFeedback;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        TeamFeedbackModel = await TeamFeedbackHandler.GetTeamFeedbackAsync(TeamFeedbackId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.TeamFeedbackModel.ToString(), TeamFeedbackId.ToString());
        
        MainLayout.SetHeaderValue("View Team Feedback");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
        }
    }
}
