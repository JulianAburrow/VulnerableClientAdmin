namespace VulnerableClientAdminTest;

public class CDOutcomeHandlerTest
{
    [Fact]
    public async Task GetCDOutcomesReturnsExpectedResults()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new CDOutcomeHandler(factory);

        var client = new VulnerableClientModel
        {
            ContactId = 100,
            FirstName = "Alice",
            Surname = "Smith"
        };
        using var seedContext = factory.CreateDbContext();
        seedContext.VulnerableClients.Add(client);

        var vi = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 200,
            ContactId = 100,
            ClientRequirementMonitoringNeed = "",
            RequiredActionByCompany = "",
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };
        seedContext.VulnerabilityInformation.Add(vi);

        var audit1 = new AuditObjectModel
        {
            ObjectId = "200",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeUnderstandingNeedsGoodOutcomes",
            PreviousValue = "Old",
            NewValue = "Good",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now.AddDays(-1)
        };

        var audit2 = new AuditObjectModel
        {
            ObjectId = "200",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeStaffSkillsAndCapabilityBadOutcomes",
            PreviousValue = "Old",
            NewValue = "Bad",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        seedContext.AuditObjects.Add(audit1);
        seedContext.AuditObjects.Add(audit2);

        seedContext.SaveChanges();

        var outcomes = await handler.GetCDOutcomesAsync();

        outcomes.Count.Should().Be(2);

        var latest = outcomes.First();
        latest.FirstName.Should().Be("Alice");
        latest.LastName.Should().Be("Smith");
        latest.VulnerabilityInformationId.Should().Be(200);
        latest.CompletedBy.Should().Be("UnitTestUser");
        latest.ColumnName.Should().Be("CDOutcomeStaffSkillsAndCapabilityBadOutcomes");
        latest.Outcome.Should().Be("Bad");
    }

    [Fact]
    public async Task GetCDOutcomesFiltersByDateRange()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new CDOutcomeHandler(factory);

        var client = new VulnerableClientModel
        {
            ContactId = 101,
            FirstName = "Bob",
            Surname = "Jones"
        };

        using var seedContext = factory.CreateDbContext();

        seedContext.VulnerableClients.Add(client);

        var vi = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 300,
            ContactId = 101,
            ClientRequirementMonitoringNeed = "",
            RequiredActionByCompany = "",
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        seedContext.VulnerabilityInformation.Add(vi);

        var oldAudit = new AuditObjectModel
        {
            ObjectId = "300",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeUnderstandingNeedsGoodOutcomes",
            PreviousValue = "",
            NewValue = "OldOutcome",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now.AddMonths(-2)
        };

        var newAudit = new AuditObjectModel
        {
            ObjectId = "300",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeUnderstandingNeedsGoodOutcomes",
            PreviousValue = "",
            NewValue = "NewOutcome",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        seedContext.AuditObjects.Add(oldAudit);
        seedContext.AuditObjects.Add(newAudit);
        seedContext.SaveChanges();

        var start = DateTime.Now.AddMonths(-1);

        var outcomes = await handler.GetCDOutcomesAsync(startDate: start);

        outcomes.Count.Should().Be(1);
        outcomes.First().Outcome.Should().Be("NewOutcome");
    }

    [Fact]
    public async Task GetCDOutcomesFiltersByVulnerabilityInformationId()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new CDOutcomeHandler(factory);

        var client1 = new VulnerableClientModel
        {
            ContactId = 102,
            FirstName = "Charlie",
            Surname = "Brown"
        };

        var client2 = new VulnerableClientModel
        {
            ContactId = 103,
            FirstName = "Charlie",
            Surname = "Brown"
        };

        using var seedContext = factory.CreateDbContext();
        seedContext.VulnerableClients.AddRange(client1, client2);

        var vi1 = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 400,
            ContactId = 102,
            ClientRequirementMonitoringNeed = "",
            RequiredActionByCompany = "",
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        var vi2 = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 401,
            ContactId = 103,
            ClientRequirementMonitoringNeed = "",
            RequiredActionByCompany = "",
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        seedContext.VulnerabilityInformation.AddRange(vi1, vi2);

        var audit1 = new AuditObjectModel
        {
            ObjectId = "400",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
            PreviousValue = "",
            NewValue = "Outcome1",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        var audit2 = new AuditObjectModel
        {
            ObjectId = "401",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
            PreviousValue = "",
            NewValue = "Outcome2",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        seedContext.AuditObjects.AddRange(audit1, audit2);
        seedContext.SaveChanges();

        var outcomes = await handler.GetCDOutcomesAsync(vulnerabilityInformationId: 400);

        outcomes.Count.Should().Be(1);
        outcomes.First().Outcome.Should().Be("Outcome1");
    }
}