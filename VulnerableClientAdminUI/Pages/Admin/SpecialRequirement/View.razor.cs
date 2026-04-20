namespace VulnerableClientAdminUI.Pages.Admin.SpecialRequirement;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        SpecialRequirementModel = await SpecialRequirementHandler.GetSpecialRequirementAsync(SpecialRequirementId);
        AuditObjects = await AuditObjectHandler.GetAuditRecordsForObjectAsync(Enums.ObjectType.SpecialRequirementModel.ToString(), SpecialRequirementId.ToString());
        PreventDeleting = SpecialRequirementModel.Vulnerabilities.Any();
        MainLayout.SetHeaderValue($"View Special Requirement '{SpecialRequirementModel?.Requirement}'");
    }
}
