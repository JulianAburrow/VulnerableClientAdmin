namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ITeamFeedbackHandler
{
    Task<TeamFeedbackModel> GetTeamFeedbackAsync(int teamFeedbackId);

    Task<List<TeamFeedbackModel>> GetTeamFeedbacksAsync(int vulnerabilityInformationId);

    Task CreateTeamFeedbackAsync(TeamFeedbackModel teamFeedback);

    Task UpdateTeamFeedbackAsync(TeamFeedbackModel teamFeedback);
}
