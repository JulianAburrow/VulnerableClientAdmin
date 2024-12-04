namespace VulnerableClientAdminUI.Shared.Components;

public partial class VulnerableClientListViewComponent
{
    [Parameter] public List<VulnerableClientModel> Clients { get; set; } = null!;
}
