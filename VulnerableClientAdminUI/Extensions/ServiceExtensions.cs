namespace VulnerableClientAdminUI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VulnerableClientAdminConnectionString");
        
        services.AddDbContext<VulnerableClientAdminContext>(
            options =>
                options.UseSqlServer(connectionString));
    }

    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IAuditObjectHandler, AuditObjectHandler>();
        services.AddTransient<ICDOutcomeHandler, CDOutcomeHandler>();
        services.AddTransient<IPreferredContactMethodHandler, PreferredContactMethodHandler>();
        services.AddTransient<ISavedPageHandler, SavedPageHandler>();
        services.AddTransient<ISourceOfAwarenessHandler, SourceOfAwarenessHandler>();
        services.AddTransient<ISpecialRequirementHandler, SpecialRequirementHandler>();
        services.AddTransient<ITeamFeedbackHandler, TeamFeedbackHandler>();
        services.AddTransient<IVulnerabilityHandler, VulnerabilityHandler>();
        services.AddTransient<IVulnerabilityNoteHandler, VulnerabilityNoteHandler>();
        services.AddTransient<IVulnerabilityInformationHandler, VulnerabilityInformationHandler>();
        services.AddTransient<IVulnerabilityReasonHandler, VulnerabilityReasonHandler>();
        services.AddTransient<IVulnerableClientHandler, VulnerableClientHandler>();
    }
}
