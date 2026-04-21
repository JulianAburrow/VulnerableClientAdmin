namespace VulnerableClientAdminUI.Shared;

public partial class NavMenu
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private IVulnerabilityInformationHandler VulnerabilityInformationHandler { get; set; } = null!;

    private int ActiveVulnerabilityCount { get; set; }

    private int InactiveVulnerabilityCount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ActiveVulnerabilityCount = VulnerabilityInformationHandler.GetActiveVulnerabilitiesCount();
        InactiveVulnerabilityCount = VulnerabilityInformationHandler.GetInactiveVulnerabilitiesCount();
    }

    private void DoNavigation(int caseState)
    {
        switch (caseState)
        {
            case (int)Enums.CaseState.Active:
                NavigationManager.NavigateTo($"vulnerabilityinformation/index/{(int)Enums.CaseState.Active}", true);
                break;
            case (int)Enums.CaseState.Inactive:
                NavigationManager.NavigateTo($"vulnerabilityinformation/index/{(int)Enums.CaseState.Inactive}", true);
                break;
        };
    }
}
