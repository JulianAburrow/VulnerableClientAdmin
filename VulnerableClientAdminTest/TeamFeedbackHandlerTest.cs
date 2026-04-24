namespace VulnerableClientAdminTest;

public class TeamFeedbackHandlerTest : TestBase
{
    private readonly VulnerableClientAdminContext _context;
    private readonly ITeamFeedbackHandler _teamFeedbackHandler;

    public TeamFeedbackHandlerTest()
    {
        _context = CreateContext();
        _teamFeedbackHandler = new TeamFeedbackHandler(_context);
    }

    private TeamFeedbackModel CreateFeedback(
        int viId,
        string feedback,
        DateTime date)
    {
        return new TeamFeedbackModel
        {
            VulnerabilityInformationId = viId,
            Feedback = feedback,
            FeedbackDate = date
        };
    }

    // ---------------------------------------------------------
    // CREATE
    // ---------------------------------------------------------

    [Fact]
    public async Task CreateTeamFeedbackCreatesTeamFeedback()
    {
        var initialCount = _context.TeamFeedbacks.Count();

        var feedback = CreateFeedback(100, "Good work", DateTime.Now);

        await _teamFeedbackHandler.CreateTeamFeedbackAsync(feedback, true);

        _context.TeamFeedbacks.Count().Should().Be(initialCount + 1);
    }

    // ---------------------------------------------------------
    // GET BY ID
    // ---------------------------------------------------------

    [Fact]
    public async Task GetTeamFeedbackGetsTeamFeedback()
    {
        var feedback = CreateFeedback(100, "Initial feedback", DateTime.Now);

        _context.TeamFeedbacks.Add(feedback);
        _context.SaveChanges();

        var returned =
            await _teamFeedbackHandler.GetTeamFeedbackAsync(feedback.TeamFeedbackId);

        returned.Should().NotBeNull();
        returned.Feedback.Should().Be("Initial feedback");
        returned.VulnerabilityInformationId.Should().Be(100);
    }

    // ---------------------------------------------------------
    // GET ALL FOR VULNERABILITY (WITH INCLUDE + ORDERING)
    // ---------------------------------------------------------

    [Fact]
    public async Task GetTeamFeedbacksGetsFeedbacksForVulnerability()
    {
        // Seed client
        var client = new VulnerableClientModel
        {
            ContactId = 200,
            FirstName = "Alice",
            Surname = "Smith"
        };
        _context.VulnerableClients.Add(client);

        // Seed vulnerability info
        var vi = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 300,
            ContactId = 200
        };
        _context.VulnerabilityInformation.Add(vi);

        // Seed feedback entries
        var f1 = CreateFeedback(300, "Old feedback", DateTime.Now.AddDays(-2));
        var f2 = CreateFeedback(300, "New feedback", DateTime.Now);

        _context.TeamFeedbacks.Add(f1);
        _context.TeamFeedbacks.Add(f2);
        _context.SaveChanges();

        var results = await _teamFeedbackHandler.GetTeamFeedbacksAsync(300);

        results.Count.Should().Be(2);
        results.Should().BeInDescendingOrder(f => f.FeedbackDate);

        results.First().Feedback.Should().Be("New feedback");
    }

    // ---------------------------------------------------------
    // UPDATE
    // ---------------------------------------------------------

    [Fact]
    public async Task UpdateTeamFeedbackUpdatesTeamFeedback()
    {
        var feedback = CreateFeedback(400, "Original", DateTime.Now);

        _context.TeamFeedbacks.Add(feedback);
        _context.SaveChanges();

        var updated = new TeamFeedbackModel
        {
            TeamFeedbackId = feedback.TeamFeedbackId,
            VulnerabilityInformationId = 400,
            Feedback = "Updated feedback",
            FeedbackDate = feedback.FeedbackDate
        };

        await _teamFeedbackHandler.UpdateTeamFeedbackAsync(updated, true);

        var returned =
            _context.TeamFeedbacks.First(f => f.TeamFeedbackId == feedback.TeamFeedbackId);

        returned.Feedback.Should().Be("Updated feedback");
    }
}