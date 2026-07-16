namespace VulnerableClientAdminTest;

public class AuditObjectHandlerTest
{
    private AuditObjectModel CreateAudit(
        string objectType,
        string objectId,
        string columnName,
        string newValue,
        DateTime changedDate)
    {
        return new AuditObjectModel
        {
            ObjectType = objectType,
            ObjectId = objectId,
            ColumnName = columnName,
            PreviousValue = "Old",
            NewValue = newValue,
            ChangedBy = "UnitTestUser",
            ChangedDate = changedDate
        };
    }

    [Fact]
    public async Task CreateAuditObjectCreatesAuditRecord()
    {
        var factory= DbContextHelper.GetInMemoryFactory();
        var handler = new AuditObjectHandler(factory);

        var audit = CreateAudit(
            "TestType",
            "123",
            "ColumnA",
            "ValueA",
            DateTime.Now);

        await handler.CreateAuditObjectAsync(audit);

        // Note: Since we're using an in-memory database, we need to access the context through the factory
        using var assertContext = factory.CreateDbContext();
        assertContext.AuditObjects.Count().Should().Be(1);
        assertContext.AuditObjects.First().ColumnName.Should().Be("ColumnA");
    }

    [Fact]
    public async Task GetAuditRecordsReturnsRecordsForType()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new AuditObjectHandler(factory);

        var a1 = CreateAudit("TypeA", "1", "Col1", "Val1", DateTime.Now.AddDays(-2));
        var a2 = CreateAudit("TypeA", "2", "Col2", "Val2", DateTime.Now.AddDays(-1));
        var a3 = CreateAudit("TypeB", "3", "Col3", "Val3", DateTime.Now);

        using var seedContext = factory.CreateDbContext();
        seedContext.AuditObjects.AddRange(a1, a2, a3);
        seedContext.SaveChanges();

        var results = await handler.GetAuditRecordsAsync("TypeA");

        results.Count.Should().Be(2);
        results.Should().BeInAscendingOrder(a => a.ChangedDate);
    }

    [Fact]
    public async Task GetAuditRecordsForObjectReturnsCorrectRecords()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new AuditObjectHandler(factory);

        var a1 = CreateAudit("TypeA", "10", "Col1", "Val1", DateTime.Now.AddDays(-3));
        var a2 = CreateAudit("TypeA", "10", "Col2", "Val2", DateTime.Now.AddDays(-1));
        var a3 = CreateAudit("TypeA", "11", "Col3", "Val3", DateTime.Now);

        using var seedContext = factory.CreateDbContext();
        seedContext.AuditObjects.AddRange(a1, a2, a3);
        seedContext.SaveChanges();

        var results = await handler.GetAuditRecordsForObjectAsync("TypeA", "10");

        results.Count.Should().Be(2);
        results.Should().BeInAscendingOrder(a => a.ChangedDate);
        results.All(a => a.ObjectId == "10").Should().BeTrue();
    }

    [Fact]
    public async Task GetLastAuditRecordsForObjectReturnsDescendingOrder()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        var handler = new AuditObjectHandler(factory);

        var a1 = CreateAudit("TypeA", "20", "Col1", "Val1", DateTime.Now.AddDays(-3));
        var a2 = CreateAudit("TypeA", "20", "Col2", "Val2", DateTime.Now.AddDays(-1));
        var a3 = CreateAudit("TypeA", "20", "Col3", "Val3", DateTime.Now);

        using var seedContext = factory.CreateDbContext();
        seedContext.AuditObjects.AddRange(a1, a2, a3);
        seedContext.SaveChanges();

        var results = await handler.GetLastAuditRecordsForObjectAsync("TypeA", "20");

        results.Count.Should().Be(3);
        results.Should().BeInDescendingOrder(a => a.ChangedDate);
    }
}
