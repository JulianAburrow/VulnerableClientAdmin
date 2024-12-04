namespace VulnerableClientAdminUI.Shared.BasePageClasses;

public class CDOutcomeBasePageClass : BasePageClass
{
    [Inject] protected ICDOutcomeHandler CDOutcomeHandler { get; set; } = null!;

    [Inject] protected IVulnerabilityInformationHandler VulnerabilityInformationHandler { get; set; } = null!;

    [Inject] protected IVulnerableClientHandler VulnerableClientHandler { get; set; } = null!;

    protected List<CDOutcomeModel> CDOutcomes { get; set; } = null!;
}
