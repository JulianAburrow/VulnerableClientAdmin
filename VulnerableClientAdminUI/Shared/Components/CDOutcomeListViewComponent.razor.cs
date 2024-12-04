namespace VulnerableClientAdminUI.Shared.Components;

public partial  class CDOutcomeListViewComponent
{
    [Parameter] public List<CDOutcomeModel> CDOutcomes { get; set; } = null!;

    [Parameter] public bool ShowClientNames { get; set; }
}
