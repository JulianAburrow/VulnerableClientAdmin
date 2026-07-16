
namespace VulnerableClientAdminDataAccess.Handlers;

public class TeamFeedbackHandler(IDbContextFactory<VulnerableClientAdminContext> factory) : ITeamFeedbackHandler
{
    public async Task CreateTeamFeedbackAsync(TeamFeedbackModel teamFeedback)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.TeamFeedbacks.Add(teamFeedback);
        await context.SaveChangesAsync();
    }

    public async Task<TeamFeedbackModel> GetTeamFeedbackAsync(int teamFeedbackId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var teamFeedback = await context.TeamFeedbacks
            .AsNoTracking()
            .SingleOrDefaultAsync(t => t.TeamFeedbackId == teamFeedbackId);

        return teamFeedback ?? new TeamFeedbackModel();
    }

    public async Task<List<TeamFeedbackModel>> GetTeamFeedbacksAsync(int vulnerabilityInformationId)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.TeamFeedbacks
            .AsNoTracking()
            .Include(t => t.Vulnerability)
                .ThenInclude(v => v.Contact)
            .Where(t => t.VulnerabilityInformationId == vulnerabilityInformationId)
            .OrderByDescending(t => t.FeedbackDate)
            .ToListAsync();
    }

    public async Task UpdateTeamFeedbackAsync(TeamFeedbackModel teamFeedback)
    {
        await using var context = await factory.CreateDbContextAsync();
        var teamFeedbackToUpdate = await context.TeamFeedbacks.SingleOrDefaultAsync(t => t.TeamFeedbackId == teamFeedback.TeamFeedbackId);
        if (teamFeedbackToUpdate is null)
            return;

        teamFeedbackToUpdate.Feedback = teamFeedback.Feedback;

        await context.SaveChangesAsync();
    }
}
