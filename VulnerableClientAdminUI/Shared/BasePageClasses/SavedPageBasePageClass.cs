namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class SavedPageBasePageClass : BasePageClass
{
    [Inject] protected ISavedPageHandler SavedPageHandler { get; set; } = null!;

    protected SavedPageModel SavedPageModel = new();

    [Parameter] public int SavedPageId { get; set; }

    protected SavedPageDisplayModel SavedPageDisplayModel = new();

    protected void PopulateModelFromDisplayModel()
    {
        SavedPageModel.Title = SavedPageDisplayModel.Title;
        SavedPageModel.Url = SavedPageDisplayModel.Url;
        SavedPageModel.Notes = SavedPageDisplayModel.Notes;
        SavedPageModel.IsExternal = SavedPageDisplayModel.IsExternal;
    }
}
