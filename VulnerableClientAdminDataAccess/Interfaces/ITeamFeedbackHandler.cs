namespace VulnerableClientAdminDataAccess.Interfaces;

public interface ITeamFeedbackHandler
{
    Task<TeamFeedbackModel> GetTeamFeedbackAsync(int teamFeedbackId);

    Task<List<TeamFeedbackModel>> GetTeamFeedbacksAsync(int vulnerabilityInformationId);

    Task CreateTeamFeedbackAsync(TeamFeedbackModel teamFeedback, bool callSaveChanges);

    Task UpdateTeamFeedbackAsync(TeamFeedbackModel teamFeedback, bool callSaveChanges);

    Task SaveChangesAsync();
}
