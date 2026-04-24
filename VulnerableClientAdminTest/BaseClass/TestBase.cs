namespace VulnerableClientAdminTest.BaseClass;

public abstract class TestBase
{
    protected readonly IHttpContextAccessor HttpContextAccessor;

    protected TestBase()
    {
        HttpContextAccessor = FakeHttpContextAccessor.Create("UnitTestUser");
    }

    protected VulnerableClientAdminContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<VulnerableClientAdminContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new VulnerableClientAdminContext(options, HttpContextAccessor);
    }
}

