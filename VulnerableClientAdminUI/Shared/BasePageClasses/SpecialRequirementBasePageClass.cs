namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class SpecialRequirementBasePageClass : BasePageClass
{
    [Inject] protected ISpecialRequirementHandler SpecialRequirementHandler { get; set; } = null!;

    protected SpecialRequirementModel SpecialRequirementModel = new();

    [Parameter] public int SpecialRequirementId { get; set; }

    protected SpecialRequirementDisplayModel SpecialRequirementDisplayModel = new();

    protected void PopulateModelFromDisplayModel()
    {
        SpecialRequirementModel.Requirement = SpecialRequirementDisplayModel.Requirement;
        SpecialRequirementModel.RequirementActive = SpecialRequirementDisplayModel.RequirementActive;
        SpecialRequirementModel.Description = SpecialRequirementDisplayModel.Description;
    }
}
