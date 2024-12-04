namespace VulnerableClientAdminDataAccess.Data;

public class VulnerableClientAdminContext : DbContext
{
    public VulnerableClientAdminContext(DbContextOptions<VulnerableClientAdminContext> options)
        : base(options)
    { }

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

        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

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
                        objAdded.CreatedBy = GlobalVariables.UserName;
                        objAdded.DateLastUpdated = DateTime.Now;
                        objAdded.LastUpdatedBy = GlobalVariables.UserName;
                    }
                    break;
                case EntityState.Modified:
                    RecordChanges(changedEntity);
                    if (changedEntity.Entity is IAuditableObject objModified)
                    {
                        objModified.DateLastUpdated = DateTime.Now;
                        objModified.LastUpdatedBy = GlobalVariables.UserName;
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

                if (objectId == null)
                    continue;

                var AuditObject = new AuditObjectModel
                {
                    ObjectId = (int)objectId,
                    ObjectType = objectType,
                    ColumnName = property.Metadata.Name,
                    PreviousValue = property.OriginalValue != null ? property.OriginalValue.ToString() : string.Empty,
                    NewValue = property.CurrentValue != null ? property.CurrentValue.ToString() : string.Empty,
                    ChangedDate = changedDate,
                    ChangedBy = GlobalVariables.UserName,
                };
                AuditObjects.Add(AuditObject);
            }
            catch(Exception ex)
            {
                //ex.ToExceptionless().Submit();
                continue;
            }
        }
    }
}