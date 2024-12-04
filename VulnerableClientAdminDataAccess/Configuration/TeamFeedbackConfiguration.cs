
namespace VulnerableClientAdminDataAccess.Configuration;

public class TeamFeedbackConfiguration : IEntityTypeConfiguration<TeamFeedbackModel>
{
    public void Configure(EntityTypeBuilder<TeamFeedbackModel> builder)
    {
        builder.ToTable("TeamFeedback");
        builder.HasKey(nameof(TeamFeedbackModel.TeamFeedbackId));
        builder.HasOne(e => e.Vulnerability)
            .WithMany(e => e.TeamFeedbacks)
            .HasForeignKey(e => e.VulnerabilityInformationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
