using VulnerableClientAdminDataAccess.CommonValues;
using static VulnerableClientAdminDataAccess.CommonValues.Enums;

namespace VulnerableClientAdminTest;

public class VulnerableClientHandlerTests
{
    private VulnerableClientModel CreateClient(int id, string first, string last, int statusId)
    {
        return new VulnerableClientModel
        {
            ContactId = id,
            FirstName = first,
            Surname = last,
            VulnerabilityStatusId = statusId
        };
    }

    private VulnerabilityInformationModel CreateVI(int id, int contactId)
    {
        return new VulnerabilityInformationModel
        {
            VulnerabilityInformationId = id,
            ContactId = contactId,
            ClientRequirementMonitoringNeed = "",
            RequiredActionByCompany = "",
            Vulnerabilities = [],
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };
    }

    [Fact]
    public async Task GetVulnerableClientReturnsClient()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            var status = new VulnerabilityStatusModel { StatusName = "Active" };
            seedContext.VulnerabilityStatuses.Add(status);
            seedContext.SaveChanges();

            var client = CreateClient(100, "Alice", "Smith", status.VulnerabilityStatusId);
            client.VulnerabilityStatus = status;

            seedContext.VulnerableClients.Add(client);
            seedContext.SaveChanges();
        }

        var result = await handler.GetVulnerableClientAsync(100);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("Alice");
        result.VulnerabilityStatus.StatusName.Should().Be("Active");
    }

    [Fact]
    public async Task GetVulnerableClientReturnsEmptyModelWhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        var result = await handler.GetVulnerableClientAsync(999);

        result.Should().NotBeNull();
        result.ContactId.Should().Be(0);
    }

    [Fact]
    public async Task GetVulnerableClientsReturnsOnlyAssessedClientsWithFullGraph()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        using var seedContext = factory.CreateDbContext();

        var statusActive = new VulnerabilityStatusModel
        {
            VulnerabilityStatusId = (int)VulnerabilityAssessmentState.PreviouslyConsideredVulnerable,
            StatusName = "Active"
        };

        var statusInactive = new VulnerabilityStatusModel
        {
            VulnerabilityStatusId = (int)VulnerabilityAssessmentState.VulnerabilityNotAssessed,
            StatusName = "Not Assessed"
        };

        seedContext.VulnerabilityStatuses.AddRange(statusActive, statusInactive);
        seedContext.SaveChanges();

        // Active client
        var client1 = CreateClient(1, "Bob", "Jones", statusActive.VulnerabilityStatusId);
        client1.VulnerabilityStatus = statusActive;

        var vi1 = CreateVI(200, 1);
        client1.VulnerabilityInformation = vi1;

        var reason = new VulnerabilityReasonModel
        {
            Reason = "Reason A",
            Description = "Desc A",
            ReasonActive = true,
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        seedContext.VulnerabilityReasons.Add(reason);
        seedContext.SaveChanges();

        var v1 = new VulnerabilityModel
        {
            VulnerabilityInformationId = 200,
            VulnerabilityReasonId = reason.VulnerabilityReasonId,
            Explanation = "Exp1",
            CreatedBy = "UnitTest",
            LastUpdatedBy = "UnitTest"
        };

        vi1.Vulnerabilities.Add(v1);

        // Inactive client
        var client2 = CreateClient(2, "Charlie", "Brown", statusInactive.VulnerabilityStatusId);
        client2.VulnerabilityStatus = statusInactive;

        seedContext.VulnerableClients.AddRange(client1, client2);
        seedContext.VulnerabilityInformation.Add(vi1);
        seedContext.Vulnerabilities.Add(v1);
        seedContext.SaveChanges();

        var results = await handler.GetVulnerableClientsAsync();

        results.Count.Should().Be(1);
        results.First().Surname.Should().Be("Jones");
        results.First().VulnerabilityInformation.Vulnerabilities.Count.Should().Be(1);
        results.First().VulnerabilityInformation.Vulnerabilities.First().VulnerabilityReason.Reason.Should().Be("Reason A");
    }

    [Fact]
    public async Task GetClientsByContactIdReturnsPartialMatches()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        using var seedContext = factory.CreateDbContext();

        var status = new VulnerabilityStatusModel { StatusName = "Active" };
        seedContext.VulnerabilityStatuses.Add(status);
        seedContext.SaveChanges();

        var c1 = CreateClient(12345, "Alice", "Smith", status.VulnerabilityStatusId);
        var c2 = CreateClient(51234, "Bob", "Jones", status.VulnerabilityStatusId);
        var c3 = CreateClient(99999, "Charlie", "Brown", status.VulnerabilityStatusId);

        seedContext.VulnerableClients.AddRange(c1, c2, c3);
        seedContext.SaveChanges();

        var results = await handler.GetClientsByContactIdAsync(123);

        results.Count.Should().Be(2);
        results.Any(c => c.ContactId == 12345).Should().BeTrue();
        results.Any(c => c.ContactId == 51234).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateVulnerableClientUpdatesStatus()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        using (var seedContext = factory.CreateDbContext())
        {
            var status1 = new VulnerabilityStatusModel { StatusName = "OldStatus" };
            var status2 = new VulnerabilityStatusModel { StatusName = "NewStatus" };

            seedContext.VulnerabilityStatuses.AddRange(status1, status2);
            seedContext.SaveChanges();

            var client = CreateClient(500, "Test", "User", status1.VulnerabilityStatusId);
            client.VulnerabilityStatus = status1;

            seedContext.VulnerableClients.Add(client);
            seedContext.SaveChanges();
        }

        var updated = new VulnerableClientModel
        {
            ContactId = 500,
            VulnerabilityStatusId = 2 // new status
        };

        await handler.UpdateVulnerableClientAsync(updated);

        using var assertContext = factory.CreateDbContext();

        var returned = assertContext.VulnerableClients.First(c => c.ContactId == 500);
        returned.VulnerabilityStatusId.Should().Be(2);
    }

    [Fact]
    public async Task GetVulnerableClientsNameOnlyReturnsCorrectProjection()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new VulnerableClientHandler(factory);

        using var seedContext = factory.CreateDbContext();

        var statusActive = new VulnerabilityStatusModel
        {
            VulnerabilityStatusId = (int)VulnerabilityAssessmentState.PreviouslyConsideredVulnerable,
            StatusName = "Active"
        };

        var statusInactive = new VulnerabilityStatusModel
        {
            VulnerabilityStatusId = (int)VulnerabilityAssessmentState.VulnerabilityNotAssessed,
            StatusName = "Not Assessed"
        };

        seedContext.VulnerabilityStatuses.AddRange(statusActive, statusInactive);
        seedContext.SaveChanges();

        var c1 = CreateClient(700, "Alice", "Smith", statusActive.VulnerabilityStatusId);
        var c2 = CreateClient(701, "Bob", "Jones", statusInactive.VulnerabilityStatusId);

        var vi1 = CreateVI(800, 700);
        c1.VulnerabilityInformation = vi1;

        seedContext.VulnerableClients.AddRange(c1, c2);
        seedContext.VulnerabilityInformation.Add(vi1);
        seedContext.SaveChanges();

        var results = await handler.GetVulnerableClientsNameOnlyAsync();

        results.Count.Should().Be(1);
        results.First().FirstName.Should().Be("Alice");
        results.First().LastName.Should().Be("Smith");
        results.First().VulnerabilityInformationId.Should().Be(800);
    }
}