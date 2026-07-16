namespace VulnerableClientAdminTest;

public class TeamFeedbackHandlerTest
{
    private TeamFeedbackModel CreateFeedback(
        int viId,
        string feedback,
        DateTime date,
        string createdBy,
        string lastUpdatedBy)
    {
        return new TeamFeedbackModel
        {
            VulnerabilityInformationId = viId,
            Feedback = feedback,
            FeedbackDate = date,
            CreatedBy = createdBy,
            LastUpdatedBy = lastUpdatedBy,
        };
    }

    // ---------------------------------------------------------
    // CREATE
    // ---------------------------------------------------------

    [Fact]
    public async Task CreateTeamFeedbackCreatesTeamFeedback()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new TeamFeedbackHandler(factory);

        var feedback = CreateFeedback(100, "Good work", DateTime.Now, "UnitTest", "UnitTest");

        await handler.CreateTeamFeedbackAsync(feedback);

        // Assert using a *fresh* context
        using var assertContext = factory.CreateDbContext();

        assertContext.TeamFeedbacks.Count().Should().Be(1);
    }

    // ---------------------------------------------------------
    // GET BY ID
    // ---------------------------------------------------------

    [Fact]
    public async Task GetTeamFeedbackGetsTeamFeedback()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new TeamFeedbackHandler(factory);

        var feedback = CreateFeedback(100, "Initial feedback", DateTime.Now, "UnitTest", "UnitTest");

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.TeamFeedbacks.Add(feedback);
            seedContext.SaveChanges();
        }

        var returnedTeamFeedback =
            await handler.GetTeamFeedbackAsync(feedback.TeamFeedbackId);

        returnedTeamFeedback.Should().NotBeNull();
        returnedTeamFeedback.Feedback.Should().Be("Initial feedback");
        returnedTeamFeedback.VulnerabilityInformationId.Should().Be(100);
    }

    // ---------------------------------------------------------
    // GET ALL FOR VULNERABILITY (WITH INCLUDE + ORDERING)
    // ---------------------------------------------------------

    [Fact]
    public async Task GetTeamFeedbacksGetsFeedbacksForVulnerability()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new TeamFeedbackHandler(factory);

        // Seed client
        var client = new VulnerableClientModel
        {
            ContactId = 200,
            FirstName = "Alice",
            Surname = "Smith"
        };

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.VulnerableClients.Add(client);
            seedContext.SaveChanges();

            // Seed vulnerability info
            var vi = new VulnerabilityInformationModel
            {
                VulnerabilityInformationId = 300,
                ContactId = 200,
                RequiredActionByCompany = "",
                ClientRequirementMonitoringNeed = "",
                CreatedBy = "UnitTest",
                LastUpdatedBy = "UnitTest"
            };
            seedContext.VulnerabilityInformation.Add(vi);

            // Seed feedback entries
            var f1 = CreateFeedback(300, "Old feedback", DateTime.Now.AddDays(-2), "UnitTest", "UnitTest");
            var f2 = CreateFeedback(300, "New feedback", DateTime.Now, "UnitTest", "UnitTest");

            seedContext.TeamFeedbacks.Add(f1);
            seedContext.TeamFeedbacks.Add(f2);
            seedContext.SaveChanges();
        }

        var results = await handler.GetTeamFeedbacksAsync(300);

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
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new TeamFeedbackHandler(factory);

        var feedback = CreateFeedback(400, "Original", DateTime.Now, "UnitTest", "UnitTest");

        using (var seedContext = factory.CreateDbContext())
        {
            seedContext.TeamFeedbacks.Add(feedback);
            seedContext.SaveChanges();
        }

        var updatedModel = new TeamFeedbackModel
        {
            TeamFeedbackId = feedback.TeamFeedbackId,
            VulnerabilityInformationId = 400,
            Feedback = "Updated feedback",
            FeedbackDate = feedback.FeedbackDate,
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        await handler.UpdateTeamFeedbackAsync(updatedModel);

        // Assert using a fresh context
        using var assertContext = factory.CreateDbContext();
        var updatedTeamFeedback =
            assertContext.TeamFeedbacks.First(f => f.TeamFeedbackId == feedback.TeamFeedbackId);

        updatedTeamFeedback.Feedback.Should().Be("Updated feedback");
    }
}