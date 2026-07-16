namespace VulnerableClientAdminTest.Helpers;

public class TestDbContextFactory : IDbContextFactory<VulnerableClientAdminContext>
{
    private readonly DbContextOptions<VulnerableClientAdminContext> _options;
    private readonly IHttpContextAccessor _accessor;

    public TestDbContextFactory(DbContextOptions<VulnerableClientAdminContext> options,
                                IHttpContextAccessor accessor)
    {
        _options = options;
        _accessor = accessor;
    }

    public VulnerableClientAdminContext CreateDbContext()
        => new VulnerableClientAdminContext(_options, _accessor);
}