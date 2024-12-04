namespace VulnerableClientAdminDataAccess.Configuration;

public class VulnerableClientConfiguration : IEntityTypeConfiguration<VulnerableClientModel>
{
    public void Configure(EntityTypeBuilder<VulnerableClientModel> builder)
    {
        builder.ToTable("Contacts", schema: "dbo");
        builder.HasKey(nameof(VulnerableClientModel.ContactId));
        builder.HasOne(e => e.VulnerabilityInformation)
            .WithOne(e => e.Contact)
            .HasForeignKey<VulnerabilityInformationModel>(e => e.ContactId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.VulnerabilityStatus)
            .WithMany(e => e.Vulnerabilities)
            .HasForeignKey(e => e.VulnerabilityStatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
