namespace VulnerableClientAdminTest.Helpers;

public static class DbContextHelper
{
    public static IDbContextFactory<VulnerableClientAdminContext> GetInMemoryFactory()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var httpContextAccessor = FakeHttpContextAccessor.Create("UnitTestUser");

        return new TestDbContextFactory(options, httpContextAccessor);
    }
}
