namespace VulnerableClientAdminTest;

public class CDOutcomeHandlerTest : TestBase
{
    private readonly VulnerableClientAdminContext _context;
    private readonly ICDOutcomeHandler _cdOutcomeHandler;

    public CDOutcomeHandlerTest()
    {
        _context = CreateContext();
        _cdOutcomeHandler = new CDOutcomeHandler(_context);
    }

    [Fact]
    public void GetCDOutcomesReturnsExpectedResults()
    {
        var client = new VulnerableClientModel
        {
            ContactId = 100,
            FirstName = "Alice",
            Surname = "Smith"
        };
        _context.VulnerableClients.Add(client);

        var vi = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 200,
            ContactId = 100
        };
        _context.VulnerabilityInformation.Add(vi);

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

        _context.AuditObjects.Add(audit1);
        _context.AuditObjects.Add(audit2);

        _context.SaveChanges();

        var outcomes = _cdOutcomeHandler.GetCDOutcomes();

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
    public void GetCDOutcomesFiltersByDateRange()
    {
        var client = new VulnerableClientModel
        {
            ContactId = 101,
            FirstName = "Bob",
            Surname = "Jones"
        };
        _context.VulnerableClients.Add(client);

        var vi = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 300,
            ContactId = 101
        };
        _context.VulnerabilityInformation.Add(vi);

        var oldAudit = new AuditObjectModel
        {
            ObjectId = "300",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeUnderstandingNeedsGoodOutcomes",
            NewValue = "OldOutcome",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now.AddMonths(-2)
        };

        var newAudit = new AuditObjectModel
        {
            ObjectId = "300",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeUnderstandingNeedsGoodOutcomes",
            NewValue = "NewOutcome",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        _context.AuditObjects.Add(oldAudit);
        _context.AuditObjects.Add(newAudit);
        _context.SaveChanges();

        var start = DateTime.Now.AddMonths(-1);

        var outcomes = _cdOutcomeHandler.GetCDOutcomes(startDate: start);

        outcomes.Count.Should().Be(1);
        outcomes.First().Outcome.Should().Be("NewOutcome");
    }

    [Fact]
    public void GetCDOutcomesFiltersByVulnerabilityInformationId()
    {
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

        _context.VulnerableClients.AddRange(client1, client2);

        var vi1 = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 400,
            ContactId = 102
        };

        var vi2 = new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = 401,
            ContactId = 103
        };

        _context.VulnerabilityInformation.AddRange(vi1, vi2);

        var audit1 = new AuditObjectModel
        {
            ObjectId = "400",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
            NewValue = "Outcome1",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        var audit2 = new AuditObjectModel
        {
            ObjectId = "401",
            ObjectType = "VulnerabilityInformationModel",
            ColumnName = "CDOutcomeMonitoringAndEvaluationGoodOutcomes",
            NewValue = "Outcome2",
            ChangedBy = "UnitTestUser",
            ChangedDate = DateTime.Now
        };

        _context.AuditObjects.AddRange(audit1, audit2);
        _context.SaveChanges();

        var outcomes = _cdOutcomeHandler.GetCDOutcomes(vulnerabilityInformationId: 400);

        outcomes.Count.Should().Be(1);
        outcomes.First().Outcome.Should().Be("Outcome1");
    }

}