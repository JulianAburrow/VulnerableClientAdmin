
namespace VulnerableClientAdminDataAccess.Handlers;

public class TeamFeedbackHandler : ITeamFeedbackHandler
{
    private readonly VulnerableClientAdminContext _context;

    public TeamFeedbackHandler(VulnerableClientAdminContext context) =>
        _context = context;

    public async Task CreateTeamFeedbackAsync(TeamFeedbackModel teamFeedback, bool callSaveChanges)
    {
        _context.TeamFeedbacks.Add(teamFeedback);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<TeamFeedbackModel> GetTeamFeedbackAsync(int teamFeedbackId) =>
        await _context.TeamFeedbacks
        .AsNoTracking()
        .SingleOrDefaultAsync(t => t.TeamFeedbackId == teamFeedbackId);

    public async Task<List<TeamFeedbackModel>> GetTeamFeedbacksAsync(int vulnerabilityInformationId) =>
        await _context.TeamFeedbacks
            .AsNoTracking()
            .Include(t => t.Vulnerability)
                .ThenInclude(v => v.Contact)
            .Where(t => t.VulnerabilityInformationId == vulnerabilityInformationId)
            .OrderByDescending(t => t.FeedbackDate)
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateTeamFeedbackAsync(TeamFeedbackModel teamFeedback, bool callSaveChanges)
    {
        var teamFeedbackToUpdate = await _context.TeamFeedbacks.SingleOrDefaultAsync(t => t.TeamFeedbackId == teamFeedback.TeamFeedbackId);
        if (teamFeedbackToUpdate is null)
            return;

        teamFeedbackToUpdate.Feedback = teamFeedback.Feedback;

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
