namespace VulnerableClientAdminDataAccess.Configuration;

public class PreferredContactMethodConfiguration : IEntityTypeConfiguration<PreferredContactMethodModel>
{
    public void Configure(EntityTypeBuilder<PreferredContactMethodModel> builder)
    {
        builder.ToTable("PreferredContactMethod");
        builder.HasKey(nameof(PreferredContactMethodModel.PreferredContactMethodId));
        builder.HasMany(e => e.Vulnerabilities)
            .WithOne(e => e.PreferredContactMethod)
            .HasForeignKey(e => e.PreferredContactMethodId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
