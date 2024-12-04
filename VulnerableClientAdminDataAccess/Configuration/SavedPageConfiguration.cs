namespace VulnerableClientAdminDataAccess.Configuration;

public class SavedPageConfiguration : IEntityTypeConfiguration<SavedPageModel>
{
    public void Configure(EntityTypeBuilder<SavedPageModel> builder)
    {
        builder.ToTable("SavedPage");
        builder.HasKey(nameof(SavedPageModel.SavedPageId));
    }
}
