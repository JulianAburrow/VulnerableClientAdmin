
namespace VulnerableClientAdminDataAccess.Configuration;

public class AuditObjectConfiguration : IEntityTypeConfiguration<AuditObjectModel>
{
    public void Configure(EntityTypeBuilder<AuditObjectModel> builder)
    {
        builder.ToTable("AuditObject");
        builder.HasKey(nameof(AuditObjectModel.AuditObjectId));
    }
}
