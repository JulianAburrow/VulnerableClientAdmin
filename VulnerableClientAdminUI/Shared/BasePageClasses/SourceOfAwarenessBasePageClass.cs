namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class SourceOfAwarenessBasePageClass : BasePageClass
{
    [Inject] protected ISourceOfAwarenessHandler SourceOfAwarenessHandler { get; set; } = null!;

    protected SourceOfAwarenessModel SourceOfAwarenessModel = new();

    [Parameter] public int SourceOfAwarenessId { get; set; }

    protected SourceOfAwarenessDisplayModel SourceOfAwarenessDisplayModel = new();

    protected void PopulateModelFromDisplayModel()
    {
        SourceOfAwarenessModel.Source = SourceOfAwarenessDisplayModel.Source;
        SourceOfAwarenessModel.SourceActive = SourceOfAwarenessDisplayModel.SourceActive;
        SourceOfAwarenessModel.Description = SourceOfAwarenessDisplayModel.Description;
    }
}
