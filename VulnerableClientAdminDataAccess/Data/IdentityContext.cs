namespace VulnerableClientAdminDataAccess.Data;

public class IdentityContext : IdentityDbContext
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("vcadminsecurity");
        base.OnModelCreating(builder);
    }
}
