
namespace VulnerableClientAdminDataAccess.Configuration;

public class SourceOfAwarenessConfiguration : IEntityTypeConfiguration<SourceOfAwarenessModel>
{
    public void Configure(EntityTypeBuilder<SourceOfAwarenessModel> builder)
    {
        builder.ToTable("SourceOfAwareness");
        builder.HasKey(nameof(SourceOfAwarenessModel.SourceOfAwarenessId));
        builder.HasMany(e => e.Vulnerabilities)
            .WithOne(e => e.SourceOfAwareness)
            .HasForeignKey(e => e.SourceOfAwarenessId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
