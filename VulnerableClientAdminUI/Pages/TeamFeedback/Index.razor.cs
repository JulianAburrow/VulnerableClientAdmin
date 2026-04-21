namespace VulnerableClientAdminUI.Pages.TeamFeedback;

public partial class Index
{
    private List<TeamFeedbackModel> TeamFeedbacks { get; set; } = null!;

    protected int ContactId = new();

    protected override async Task OnInitializedAsync()
    {
        TeamFeedbacks = (await TeamFeedbackHandler.GetTeamFeedbacksAsync(VulnerabilityInformationId))
            .ToList();

        ContactId = VulnerabilityInformationHandler.GetContactIdFromVulnerabilityInformationId(VulnerabilityInformationId);
        Snackbar.Add($"{TeamFeedbacks.Count} {(TeamFeedbacks.Count == 1 ? "team feedback found" : "team feedbacks found")}",
            TeamFeedbacks.Count == 0 ? Severity.Error : Severity.Success);

        if (TeamFeedbacks.Count == 0)
        {
            MainLayout.SetHeaderValue("No Feedback found");
            return;
        }

        MainLayout.SetHeaderValue($"Feedback for {TeamFeedbacks[0].Vulnerability.Contact.FirstName} {TeamFeedbacks[0].Vulnerability.Contact.Surname} case");
    }

    private async Task CreateTeamFeedback()
    {
        TeamFeedbackModel.VulnerabilityInformationId = VulnerabilityInformationId;
        TeamFeedbackModel.Feedback = TeamFeedbackDisplayModel.Feedback;
        TeamFeedbackModel.FeedbackDate = DateTime.Now;

        try
        {
            await TeamFeedbackHandler.CreateTeamFeedbackAsync(TeamFeedbackModel, true);
            Snackbar.Add($"Team Feedback successfully created", Severity.Success);
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
        catch
        {
            Snackbar.Add("An error occurred creating the feedback. Please try again.", Severity.Error);
        }
    }
}
