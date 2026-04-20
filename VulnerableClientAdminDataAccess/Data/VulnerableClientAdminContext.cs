namespace VulnerableClientAdminDataAccess.Data;

public class VulnerableClientAdminContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VulnerableClientAdminContext(DbContextOptions<VulnerableClientAdminContext> options)
    : base(options)
    {
        // Used only for tests
    }


    public VulnerableClientAdminContext(DbContextOptions<VulnerableClientAdminContext> options,
                                            IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string CurrentUserName =>
        _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

    public DbSet<AuditObjectModel> AuditObjects { get; set; }
    public DbSet<PreferredContactMethodModel> PreferredContactMethods { get; set; }
    public DbSet<SavedPageModel> SavedPages { get; set; }
    public DbSet<SourceOfAwarenessModel> SourcesOfAwareness { get; set; }
    public DbSet<SpecialRequirementModel> SpecialRequirements { get; set; }
    public DbSet<TeamFeedbackModel> TeamFeedbacks { get; set; }
    public DbSet<VulnerabilityModel> Vulnerabilities { get; set; }
    public DbSet<VulnerabilityNoteModel> VulnerabilityNotes { get; set; }
    public DbSet<VulnerabilityInformationModel> VulnerabilityInformation { get; set; }
    public DbSet<VulnerabilityReasonModel> VulnerabilityReasons { get; set; }
    public DbSet<VulnerabilityStatusModel> VulnerabilityStatuses { get; set; }
    public DbSet<VulnerableClientModel> VulnerableClients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("vcadminoperations");

        base.OnModelCreating(builder);

        ConfigureIdentityTables(builder);
        
        builder.ApplyConfiguration(new AuditObjectConfiguration());
        builder.ApplyConfiguration(new PreferredContactMethodConfiguration());
        builder.ApplyConfiguration(new SavedPageConfiguration());
        builder.ApplyConfiguration(new SourceOfAwarenessConfiguration());
        builder.ApplyConfiguration(new SpecialRequirementConfiguration());
        builder.ApplyConfiguration(new TeamFeedbackConfiguration());
        builder.ApplyConfiguration(new VulnerabilityConfiguration());
        builder.ApplyConfiguration(new VulnerabilityNoteConfiguration());
        builder.ApplyConfiguration(new VulnerabilityInformationConfiguration());
        builder.ApplyConfiguration(new VulnerabilityReasonConfiguration());
        builder.ApplyConfiguration(new VulnerabilityStatusConfiguration());
        builder.ApplyConfiguration(new VulnerableClientConfiguration());
    }

    private void ConfigureIdentityTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", "vcadminsecurity");
        modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "vcadminsecurity");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", "vcadminsecurity");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", "vcadminsecurity");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", "vcadminsecurity");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", "vcadminsecurity");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", "vcadminsecurity");
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var changedEntity in ChangeTracker.Entries())
        {
            switch (changedEntity.State)
            {
                case EntityState.Added:
                    if (changedEntity.Entity is IAuditableObject objAdded)
                    {
                        objAdded.DateCreated = DateTime.Now;
                        objAdded.CreatedBy = CurrentUserName;
                        objAdded.DateLastUpdated = DateTime.Now;
                        objAdded.LastUpdatedBy = CurrentUserName;
                    }
                    break;
                case EntityState.Modified:
                    RecordChanges(changedEntity);
                    if (changedEntity.Entity is IAuditableObject objModified)
                    {
                        objModified.DateLastUpdated = DateTime.Now;
                        objModified.LastUpdatedBy = CurrentUserName;
                    }
                    break;
            }
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess = true, cancellationToken);
    }

    private void RecordChanges(EntityEntry entityEntry)
    {
        var changedDate = DateTime.Now;

        foreach (var property in entityEntry.Properties)
        {
            try
            {
                if (property.CurrentValue?.ToString() == property.OriginalValue?.ToString())
                    continue;

                var objectFullType = entityEntry.Entity.ToString();
                var objectType = objectFullType?[(objectFullType.LastIndexOf(".") + 1)..];
                var objectPrimaryKeyProperty = $"{objectType?[..^5]}Id";
                var objectId = entityEntry.Entity.GetType().GetProperty(objectPrimaryKeyProperty)?.GetValue(entityEntry.Entity, null);

                if (objectId is null)
                    continue;

                var AuditObject = new AuditObjectModel
                {
                    ObjectId = objectId.ToString(),
                    ObjectType = objectType,
                    ColumnName = property.Metadata.Name,
                    PreviousValue = property.OriginalValue is not null ? property.OriginalValue.ToString() : string.Empty,
                    NewValue = property.CurrentValue is not null ? property.CurrentValue.ToString() : string.Empty,
                    ChangedDate = changedDate,
                    ChangedBy = CurrentUserName,
                };
                AuditObjects.Add(AuditObject);
            }
            catch
            {
                continue;
            }
        }
    }
}