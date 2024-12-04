namespace VulnerableClientAdminUI.Features.TeamFeedback;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        TeamFeedbackModel = await TeamFeedbackHandler.GetTeamFeedbackAsync(TeamFeedbackId);
        TeamFeedbackDisplayModel.VulnerabilityInformationId = TeamFeedbackModel.VulnerabilityInformationId;
        TeamFeedbackDisplayModel.Feedback = TeamFeedbackModel.Feedback;

        MainLayout.SetHeaderValue("Edit Team Feedback");
    }

    private async void UpdateTeamFeedback()
    {
        PopulateModelFromDisplayModel();

        try
        {
            await TeamFeedbackHandler.UpdateTeamFeedbackAsync(TeamFeedbackModel, true);
            Snackbar.Add("Team Feedback successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"teamfeedbacks/{TeamFeedbackModel.VulnerabilityInformationId}/index");
        }
        catch (Exception ex)
        {
            //ex.ToExceptionless().Submit();
            Snackbar.Add("An error occurred updating the Team Feedback. Please try again.", Severity.Error);
        }
    }
}
