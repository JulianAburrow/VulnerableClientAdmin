namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class PreferredContactMethodBasePageClass : BasePageClass
{
    [Inject] protected IPreferredContactMethodHandler PreferredContactMethodHandler { get; set; } = null!;

    protected PreferredContactMethodModel PreferredContactMethodModel = new();

    [Parameter] public int PreferredContactMethodId { get; set; }

    protected PreferredContactMethodDisplayModel PreferredContactMethodDisplayModel = new();

    protected void PopulateModelFromDisplayModel()
    {
        PreferredContactMethodModel.Method = PreferredContactMethodDisplayModel.Method;
        PreferredContactMethodModel.MethodActive = PreferredContactMethodDisplayModel.MethodActive;
        PreferredContactMethodModel.Description = PreferredContactMethodDisplayModel.Description;
    }
}
