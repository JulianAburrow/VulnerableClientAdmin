namespace VulnerableClientAdminDataAccess.Configuration;

public class SpecialRequirementConfiguration : IEntityTypeConfiguration<SpecialRequirementModel>
{
    public void Configure(EntityTypeBuilder<SpecialRequirementModel> builder)
    {
        builder.ToTable("SpecialRequirement");
        builder.HasKey(nameof(SpecialRequirementModel.SpecialRequirementId));
        builder.HasMany(e => e.Vulnerabilities)
            .WithOne(e => e.SpecialRequirement)
            .HasForeignKey(e => e.SpecialRequirementId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
